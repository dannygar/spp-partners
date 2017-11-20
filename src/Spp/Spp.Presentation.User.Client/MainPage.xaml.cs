/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.Activation;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Spp.Presentation.User.Shared.CognitiveServiceHelpers;
using MicrosoftSportsScience.Helpers;

namespace MicrosoftSportsScience
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        public string LaunchParam { get; set; }
        partial void GetScenarioIdForLaunch(string launchParam, ref int index);

        public MainPage()
        {
            this.InitializeComponent();

            // This is a static public property that allows downstream pages to get a handle to the MainPage instance
            // in order to call methods that are in this class.
            Current = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Cache Data
                //var cacheModel = SimpleIoc.Default.GetInstance<CacheModel>();
                //await cacheModel.CacheData();


                // Cognitive Services Face Recognition API Initialization
                // propogate settings to the core library
                CSSettingsHelper.Instance.SettingsChanged += (target, args) =>
                {
                    EmotionServiceHelper.ApiKey = CSSettingsHelper.Instance.EmotionApiKey;
                    FaceServiceHelper.ApiKey = CSSettingsHelper.Instance.FaceApiKey;
                    FaceServiceHelper.ApiRoot = CSSettingsHelper.Instance.Location;
                    ImageAnalyzer.PeopleGroupsUserDataFilter = CSSettingsHelper.Instance.WorkspaceKey;
                    FaceListManager.FaceListsUserDataFilter = CSSettingsHelper.Instance.WorkspaceKey;
                    CoreUtil.MinDetectableFaceCoveragePercentage =
                        CSSettingsHelper.Instance.MinDetectableFaceCoveragePercentage;
                };

                // callbacks for core library
                FaceServiceHelper.Throttled = () => ShowThrottlingToast("Face");
                EmotionServiceHelper.Throttled = () => ShowThrottlingToast("Emotion");
                ErrorTrackingHelper.TrackException = LogException;
                ErrorTrackingHelper.GenericApiCallExceptionHandler =
                    FaceRecognitionHelper.GenericApiCallExceptionHandler;

                //Read API keys from the local storage
                CSSettingsHelper.Instance.Initialize();


                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
                Window.Current.Content = new Shell(rootFrame);

            // Ensure the current window is active
            Window.Current.Activate();

        }


        private static void LogException(Exception ex, string message)
        {
            Debug.WriteLine("Error detected! Exception: \"{0}\", More info: \"{1}\".", ex.Message, message);
        }

        private static void ShowThrottlingToast(string api)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode("Face Recognition"));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode("The " + api + " API is throttling your requests. Consider upgrading to a Premium Key."));

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

    }
}
