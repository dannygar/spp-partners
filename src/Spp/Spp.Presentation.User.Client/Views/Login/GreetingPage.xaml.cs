/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.ProjectOxford.Common.Contract;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Helpers;
using Spp.Presentation.User.Client.Models;
using Spp.Presentation.User.Client.Services;
using Spp.Presentation.User.Client.ViewModels;
using Spp.Presentation.User.Shared.CognitiveServiceHelpers;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Spp.Presentation.User.Client.Views
{

    public sealed partial class GreetingPage : Page
    {
        private Task processingLoopTask;
        private bool isProcessingLoopInProgress;
        private bool isProcessingPhoto;
        SplitView rootPage = Shell.Current;


        public MotivationalImagesViewModel ImagesModel { get; set; }
        DispatcherTimer timer;

        public GreetingPage()
        {
            this.InitializeComponent();

            rootPage.CompactPaneLength = 0;
            ImagesModel = new MotivationalImagesViewModel();

            Window.Current.Activated += CurrentWindowActivationStateChanged;
            this.cameraControl.FilterOutSmallFaces = true;
            this.cameraControl.HideCameraControls();
            this.cameraControl.CameraAspectRatioChanged += CameraControl_CameraAspectRatioChanged;
            this.Loaded += GreetingPage_Loaded;
        }

        private async void GreetingPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Load configuration settings
            await AzureADv2AuthService.LoadConfig();

            //Load motivation images
            await ImagesModel.Load();

            if (ImagesModel.MotivationalImages != null)
            {
                timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(5)
                };
                timer.Tick += Timer_Tick;
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            timer.Stop();
            SwitchImage();
        }

        private void SwitchImage()
        {
            fadeOut.Begin();
        }

        private void FadeOut_Completed(object sender, object e)
        {
            ImagesModel.MoveNext();
            fadeIn.Begin();
        }

        private void FadeIn_Completed(object sender, object e)
        {
            timer.Start();
        }

        private void CameraControl_CameraAspectRatioChanged(object sender, EventArgs e)
        {
            this.UpdateCameraHostSize();
        }

        private void StartProcessingLoop()
        {
            this.isProcessingLoopInProgress = true;

            if (this.processingLoopTask == null || this.processingLoopTask.Status != TaskStatus.Running)
            {
                this.processingLoopTask = Task.Run(() => this.ProcessingLoop());
            }
        }


        private async void ProcessingLoop()
        {
            while (this.isProcessingLoopInProgress)
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    if (!this.isProcessingPhoto)
                    {
                        this.isProcessingPhoto = true;
                        if (this.cameraControl.NumFacesOnLastFrame == 0)
                        {
                            await this.ProcessCameraCapture(null);
                        }
                        else
                        {
                            await this.ProcessCameraCapture(await this.cameraControl.CaptureFrameAsync());
                        }
                    }
                });

                await Task.Delay(this.cameraControl.NumFacesOnLastFrame == 0 ? 100 : 1000);
            }
        }

        private async void CurrentWindowActivationStateChanged(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if ((e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.CodeActivated ||
                e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.PointerActivated) &&
                this.cameraControl.CameraStreamState == Windows.Media.Devices.CameraStreamState.Shutdown)
            {
                // When our Window loses focus due to user interaction Windows shuts it down, so we 
                // detect here when the window regains focus and trigger a restart of the camera.
                await this.cameraControl.StartStreamAsync(isForRealTimeProcessing: true);
            }
        }

        private async Task ProcessCameraCapture(ImageAnalyzer e)
        {
            if (e == null)
            {
                this.UpdateUIForNoFacesDetected();
                this.isProcessingPhoto = false;
                return;
            }

            DateTime start = DateTime.Now;

            await e.DetectFacesAsync();

            if (e.DetectedFaces != null && e.DetectedFaces.Any())
            {
                await e.IdentifyFacesAsync();
                this.greetingTextBlock.Text = this.GetGreettingFromFaces(e);

                if (e.IdentifiedPersons.Any())
                {
                    this.greetingTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.GreenYellow);
                    this.greetingSymbol.Foreground = new SolidColorBrush(Windows.UI.Colors.GreenYellow);
                    this.greetingSymbol.Symbol = Symbol.Comment;

                    //Authenticate identified people
                    await AuthenticateIdentifedPerson(e.IdentifiedPersons.ToList(), e.DetectedEmotion?.ToList());
                }
                else
                {
                    this.greetingTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Yellow);
                    this.greetingSymbol.Foreground = new SolidColorBrush(Windows.UI.Colors.Yellow);
                    this.greetingSymbol.Symbol = Symbol.View;
                }
            }
            else
            {
                this.UpdateUIForNoFacesDetected();
            }

            if (CSSettingsHelper.Instance.ShowDebugInfo)
            {
                TimeSpan latency = DateTime.Now - start;
                this.faceLantencyDebugText.Text = string.Format("Face API latency: {0}ms", (int)latency.TotalMilliseconds);
            }

            this.isProcessingPhoto = false;
        }

        private string GetGreettingFromFaces(ImageAnalyzer img)
        {
            if (img.IdentifiedPersons.Any())
            {
                string names = img.IdentifiedPersons.Count() > 1 ? string.Join(", ", img.IdentifiedPersons.Select(p => p.Person.Name)) : img.IdentifiedPersons.First().Person.Name;

                if (img.DetectedFaces.Count() > img.IdentifiedPersons.Count())
                {
                    return string.Format("Welcome back, {0} and the team!", names);
                }
                else
                {
                    return string.Format("Welcome back, {0}!", names);
                }
            }
            else
            {
                if (img.DetectedFaces.Count() > 1)
                {
                    return "Hi everyone! If I knew any of you by name I would say it...";
                }
                else
                {
                    return "Hi there! If I knew you by name I would say it...";
                }
            }
        }

        private void UpdateUIForNoFacesDetected()
        {
            this.greetingTextBlock.Text = "Step in front of the camera to start";
            this.greetingTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
            this.greetingSymbol.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
            this.greetingSymbol.Symbol = Symbol.Contact;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            EnterFullScreenMode();

            //Init Cognitive Services API settings
            InitSettings();


            if (string.IsNullOrEmpty(CSSettingsHelper.Instance.FaceApiKey))
            {
                await new MessageDialog("Missing Face API Key. Please set up the key in the Face API Trainer application.", "Missing API Key").ShowAsync();
            }
            else
            {
                await this.cameraControl.StartStreamAsync(isForRealTimeProcessing: true);
                this.StartProcessingLoop();
            }

            base.OnNavigatedTo(e);
        }


        private void InitSettings()
        {
            FaceServiceHelper.ApiKey = CSSettingsHelper.Instance.FaceApiKey;
            FaceServiceHelper.ApiRoot = CSSettingsHelper.Instance.Location;
            EmotionServiceHelper.ApiKey = CSSettingsHelper.Instance.EmotionApiKey;
            ImageAnalyzer.PeopleGroupsUserDataFilter = CSSettingsHelper.Instance.WorkspaceKey;
            FaceListManager.FaceListsUserDataFilter = CSSettingsHelper.Instance.WorkspaceKey;
            CoreUtil.MinDetectableFaceCoveragePercentage = CSSettingsHelper.Instance.MinDetectableFaceCoveragePercentage;
        }


        private void EnterFullScreenMode()
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            if (!view.IsFullScreenMode)
            {
                view.TryEnterFullScreenMode();
            }
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            this.isProcessingLoopInProgress = false;
            Window.Current.Activated -= CurrentWindowActivationStateChanged;
            this.cameraControl.CameraAspectRatioChanged -= CameraControl_CameraAspectRatioChanged;

            await this.cameraControl.StopStreamAsync();
            base.OnNavigatingFrom(e);
        }

        private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateCameraHostSize();
        }

        private void UpdateCameraHostSize()
        {
            this.cameraHostGrid.Width = this.cameraHostGrid.ActualHeight * (this.cameraControl.CameraAspectRatio != 0 ? this.cameraControl.CameraAspectRatio : 1.777777777777);
        }

        private void AppSessionModel_Authenticated(object sender, int e)
        {
            this.cameraControl.RestartAutoCaptureCycle();

            var appSessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
            appSessionModel.Authenticated -= AppSessionModel_Authenticated;

            //Go to the SignIn page with the assigned Team ID (the AAD authentication will be skipped)
            (rootPage.Content as Frame).Navigate(typeof(SignIn), this);
        }

        /// <summary>
        /// Authenticate the person's identity
        /// </summary>
        /// <param name="people"></param>
        /// <param name="emotions"></param>
        private async Task AuthenticateIdentifedPerson(IList<IdentifiedPerson> people, IList<Emotion> emotions)
        {
            //Update facial emotions of the recognized person and 
            //get the person's team Id by authenticating the recognized person with TPP DB
            var appSessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();

            appSessionModel.Authenticated += AppSessionModel_Authenticated;

            //Map IdentifiedPerson to the UserIdentity
            var peopleIdentities = new List<UserIdentity>();
            for (var i = 0; i < people.Count; i++)
            {
                var user = new UserIdentity()
                {
                    FullName = people[i].Person.Name,
                    Confidence = people[i].Confidence,
                };
                if (emotions != null && emotions.Count > i)
                {
                    user.Emotions = new Emotions()
                    {
                        Anger = (emotions.Count < i) ? emotions[i].Scores.Anger : 0,
                        Contempt = (emotions.Count < i) ? emotions[i].Scores.Contempt : 0,
                        Disgust = (emotions.Count < i) ? emotions[i].Scores.Disgust : 0,
                        Fear = (emotions.Count < i) ? emotions[i].Scores.Fear : 0,
                        Happiness = (emotions.Count < i) ? emotions[i].Scores.Happiness : 0,
                        Neutral = (emotions.Count < i) ? emotions[i].Scores.Neutral : 0,
                        Sadness = (emotions.Count < i) ? emotions[i].Scores.Sadness : 0,
                        Surprise = (emotions.Count < i) ? emotions[i].Scores.Surprise : 0
                    };
                }
                else
                    user.Emotions = new Emotions();

                peopleIdentities.Add(user);
            }

            var logService = SimpleIoc.Default.GetInstance<ILogService>();
            //Athenticate via AAD
            appSessionModel.TeamId = await appSessionModel.AuthenticateFaciallyRecognizedPerson(peopleIdentities);
            if (appSessionModel.TeamId == 0)
            {
                var ex = new Exception("Unable to authenticate the user. The Face Recognition Feature will be disabled until the next time this application is restarted.");
                logService.Error(ex.Message, this);
                await FaceRecognitionHelper.GenericApiCallExceptionHandler(ex, "Access Denied");
                //Go to the AccessDenied page with the assigned Team ID (the AAD authentication will be skipped)
                (rootPage.Content as Frame).Navigate(typeof(AccessDenied));
            }

        }



        private async void RestartButton_Clicked(object sender, TappedRoutedEventArgs e)
        {
            UpdateUIForNoFacesDetected();
            await this.cameraControl.StartStreamAsync(isForRealTimeProcessing: true);
            this.StartProcessingLoop();
        }


    }
}
