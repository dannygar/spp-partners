/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Data.Services;
using MicrosoftSportsScience.Models;
using MicrosoftSportsScience.UserControls;
using MicrosoftSportsScience.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CognitiveServices;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Helpers;
using MicrosoftSportsScience.Views;

namespace MicrosoftSportsScience
{
    public sealed partial class FaceRecognition : Page
    {
        SplitView rootPage = Shell.Current;
        public FaceRecognition()
        {
            this.InitializeComponent();


            rootPage.CompactPaneLength = 0;
            Window.Current.Activated += CurrentWindowActivationStateChanged;
            this.cameraControl.EnableAutoCaptureMode = true;
            this.cameraControl.FilterOutSmallFaces = true;
            this.cameraControl.AutoCaptureStateChanged += CameraControl_AutoCaptureStateChanged;
            this.cameraControl.CameraAspectRatioChanged += CameraControl_CameraAspectRatioChanged;
            this.cameraControl.ImageCaptured += CameraControl_ImageCaptured;
            this.cameraControl.CameraRestarted += CameraControl_CameraRestarted;
            this.imageWithFacesControl.FaceRecognitionCompleted += ImageWithFacesControl_FaceRecognitionCompleted;
            this.imageFromCameraWithFaces.FaceRecognitionCompleted += ImageWithFacesControl_FaceRecognitionCompleted;

        }


        private void CameraControl_CameraAspectRatioChanged(object sender, EventArgs e)
        {
            this.UpdateWebCamHostGridSize();
        }


        private async void CurrentWindowActivationStateChanged(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if ((e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.CodeActivated ||
                e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.PointerActivated) &&
                this.cameraControl.CameraStreamState == Windows.Media.Devices.CameraStreamState.Shutdown)
            {
                // When our Window loses focus due to user interaction Windows shuts it down, so we 
                // detect here when the window regains focus and trigger a restart of the camera.
                await this.cameraControl.StartStreamAsync();
            }
        }

        private async void CameraControl_AutoCaptureStateChanged(object sender, AutoCaptureState e)
        {
            switch (e)
            {
                case AutoCaptureState.WaitingForFaces:
                    this.cameraGuideBallon.Opacity = 1;
                    this.cameraGuideText.Text = "Step in front of the camera to log in!";
                    this.cameraGuideHost.Opacity = 1;
                    await Task.Delay(1500);
                    break;
                case AutoCaptureState.WaitingForStillFaces:
                    this.cameraGuideText.Text = "Hold still...";
                    await Task.Delay(750);
                    break;
                case AutoCaptureState.ShowingCountdownForCapture:
                    this.cameraGuideText.Text = "";
                    this.cameraGuideBallon.Opacity = 0;

                    this.cameraGuideCountdownHost.Opacity = 1;
                    this.countDownTextBlock.Text = "3";
                    await Task.Delay(1500);
                    this.countDownTextBlock.Text = "2";
                    await Task.Delay(1500);
                    this.countDownTextBlock.Text = "1";
                    await Task.Delay(1500);
                    this.cameraGuideCountdownHost.Opacity = 0;

                    this.CameraControl_ImageCaptured(sender, await this.cameraControl.TakeAutoCapturePhoto());
                    //this.ProcessCameraCapture(await this.cameraControl.TakeAutoCapturePhoto());
                    break;
                case AutoCaptureState.ShowingCapturedPhoto:
                    this.cameraGuideHost.Opacity = 0;
                    break;
                default:
                    break;
            }
        }



        private void ProcessCameraCapture(ImageAnalyzer e)
        {
            if (e == null)
            {
                return;
            }

            this.imageFromCameraWithFaces.DataContext = e;

            e.FaceRecognitionCompleted += async (s, args) =>
            {
                this.photoCaptureBalloonHost.Opacity = 1;

                int photoDisplayDuration = 10;
                double decrementPerSecond = 100.0 / photoDisplayDuration;
                for (double i = 100; i >= 0; i -= decrementPerSecond)
                {
                    this.resultDisplayTimerUI.Value = i;
                    await Task.Delay(1000);
                }

                this.photoCaptureBalloonHost.Opacity = 0;
                this.imageFromCameraWithFaces.DataContext = null;

                this.cameraControl.RestartAutoCaptureCycle();
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="faces"></param>
        private async void ImageWithFacesControl_FaceRecognitionCompleted(object sender, List<FaceCharacteristics> faces)
        {
            //Iterate thru all recognized people
            List<FaceCharacteristics> recognizedPeople = faces.Where(face => face?.Person?.Confidence > Defines.FACE_RECOGNITION_MIN_SCORE)
                .OrderBy(x => x.Person.Confidence).ToList();

            //Create the list of identities
            var peopleIdenties = recognizedPeople.Select(person => new UserIdentity()
            {
                Confidence = person.Person.Confidence,
                FullName = person.Person.Person.Name,
                Emotions = new Emotions()
                {
                    Anger = person.Emotion.Scores.Anger,
                    Contempt = person.Emotion.Scores.Contempt,
                    Disgust = person.Emotion.Scores.Disgust,
                    Fear = person.Emotion.Scores.Fear,
                    Happiness = person.Emotion.Scores.Happiness,
                    Neutral = person.Emotion.Scores.Neutral,
                    Sadness = person.Emotion.Scores.Sadness,
                    Surprise = person.Emotion.Scores.Surprise
                }
            }).OrderByDescending(o => o.Confidence).ToList();


            //Update facial emotions of the recognized person and 
            //get the person's team Id by authenticating the recognized person with TPP DB
            var appSessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
            appSessionModel.Authenticated += (s, args) =>
            {
                this.photoCaptureBalloonHost.Opacity = 0;
                this.imageFromCameraWithFaces.DataContext = null;

                this.cameraControl.RestartAutoCaptureCycle();

                //Go to the SignIn page with the assigned Team ID (the AAD authentication will be skipped)
                (rootPage.Content as Frame).Navigate(typeof(SignIn));

            };

            appSessionModel.TeamId = await appSessionModel.AuthenticateFaciallyRecognizedPerson(peopleIdenties);
            if (appSessionModel.TeamId == 0)
            {
                var logService = SimpleIoc.Default.GetInstance<ILogService>();
                var ex = new Exception("Unable to authenticate the user. The Face Recognition Feature will be disabled until the next time this application is restarted.");
                logService.Error(ex.Message, this);
                await FaceRecognitionHelper.GenericApiCallExceptionHandler(ex, "Access Denied");
                //Go to the AccessDenied page with the assigned Team ID (the AAD authentication will be skipped)
                (rootPage.Content as Frame).Navigate(typeof(AccessDenied));
            };
        }


        private async void CameraControl_CameraRestarted(object sender, EventArgs e)
        {
            // We induce a delay here to give the camera some time to start rendering before we hide the last captured photo.
            // This avoids a black flash.
            await Task.Delay(500);

            this.imageFromCameraWithFaces.Visibility = Visibility.Collapsed;
        }

        private async void CameraControl_ImageCaptured(object sender, ImageAnalyzer e)
        {
            this.imageFromCameraWithFaces.DataContext = e;
            this.imageFromCameraWithFaces.Visibility = Visibility.Visible;

            await this.cameraControl.StopStreamAsync();
        }


        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

            Window.Current.Activated -= CurrentWindowActivationStateChanged;
            this.cameraControl.AutoCaptureStateChanged -= CameraControl_AutoCaptureStateChanged;
            this.cameraControl.CameraAspectRatioChanged -= CameraControl_CameraAspectRatioChanged;

            await this.cameraControl.StopStreamAsync();
            base.OnNavigatingFrom(e);
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await this.cameraControl.StopStreamAsync();

            //Init Cognitive Services API settings
            InitSettings();

            //Start camera
            this.OnWebCamButtonClicked(this, new RoutedEventArgs());

        }





        private void InitSettings()
       {
            FaceServiceHelper.ApiKey = CSSettingsHelper.Instance.FaceApiKey;
            EmotionServiceHelper.ApiKey = CSSettingsHelper.Instance.EmotionApiKey;
            ImageAnalyzer.PeopleGroupsUserDataFilter = CSSettingsHelper.Instance.WorkspaceKey;
            FaceListManager.FaceListsUserDataFilter = CSSettingsHelper.Instance.WorkspaceKey;
            CoreUtil.MinDetectableFaceCoveragePercentage = CSSettingsHelper.Instance.MinDetectableFaceCoveragePercentage;

            //FOR DEBUG PURPOSES ONLY!!!
            //EmotionServiceHelper.ApiKey = "837be46e535547ec8892f7fdf5cf1645";
            //FaceServiceHelper.ApiKey = "73a191f1899e44ff9f14be1c81e9e7b7";
            //ImageAnalyzer.PeopleGroupsUserDataFilter = "b798ac78-acf5-44e9-a405-46a85736fd8e";
            //FaceListManager.FaceListsUserDataFilter = "b798ac78-acf5-44e9-a405-46a85736fd8e";
            //CoreUtil.MinDetectableFaceCoveragePercentage = 7;

        }

        private async void OnWebCamButtonClicked(object sender, RoutedEventArgs e)
        {
            webCamHostGrid.Visibility = Visibility.Visible;
            imageWithFacesControl.Visibility = Visibility.Collapsed;

            await this.cameraControl.StartStreamAsync();
            await Task.Delay(250);
            this.imageFromCameraWithFaces.Visibility = Visibility.Collapsed;

            UpdateWebCamHostGridSize();
        }


        private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateWebCamHostGridSize();
        }



        private void UpdateWebCamHostGridSize()
        {
            this.webCamHostGrid.Width = this.webCamHostGrid.ActualHeight * (this.cameraControl.CameraAspectRatio != 0 ? this.cameraControl.CameraAspectRatio : 1.777777777777);
        }


    }
}
