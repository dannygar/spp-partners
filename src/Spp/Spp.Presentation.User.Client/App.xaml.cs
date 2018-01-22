/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Diagnostics;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.WindowsAzure.MobileServices;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Helpers;
using Spp.Presentation.User.Client.Models;
using Spp.Presentation.User.Client.Services;
using Spp.Presentation.User.Shared.CognitiveServiceHelpers;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Spp.Presentation.User.Client
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            SimpleIoc.Default.Register<RecommendedLoadModel, RecommendedLoadModel>();
            SimpleIoc.Default.Register<TeamModel, TeamModel>();
            SimpleIoc.Default.Register<AthleteQuestionModel, AthleteQuestionModel>();
            SimpleIoc.Default.Register<AthleteAnswerModel, AthleteAnswerModel>();
            SimpleIoc.Default.Register<AthleteQuestionHistory, AthleteQuestionHistory>();
            SimpleIoc.Default.Register<AppSessionModel, AppSessionModel>();
            SimpleIoc.Default.Register<AthleteSessionModel, AthleteSessionModel>();
            SimpleIoc.Default.Register<AthleteQuestionHistoryModel, AthleteQuestionHistoryModel>();
            SimpleIoc.Default.Register<AthleteMessageModel, AthleteMessageModel>();
            SimpleIoc.Default.Register<CacheModel, CacheModel>();
            SimpleIoc.Default.Register<AthletePracticeModel, AthletePracticeModel>();
            SimpleIoc.Default.Register<AthleteWorkoutModel, AthleteWorkoutModel>();
            SimpleIoc.Default.Register<MotivationalImagesModel, MotivationalImagesModel>();
            SimpleIoc.Default.Register<CoachModel, CoachModel>();
            SimpleIoc.Default.Register<ILogService, LocalLogService>();
            SimpleIoc.Default.Register<ICacheService, InMemoryCacheService>();
            SimpleIoc.Default.Register<IApiAuthService, AzureADv2AuthService>();
            SimpleIoc.Default.Register<IHttpClientService, ApiClientService>();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
                this.DebugSettings.EnableFrameRateCounter = true;
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {

                // Cognitive Services Face Recognition API Initialization
                // propogate settings to the core library
                CSSettingsHelper.Instance.SettingsChanged += (target, args) =>
                {
                    EmotionServiceHelper.ApiKey = CSSettingsHelper.Instance.EmotionApiKey;
                    FaceServiceHelper.ApiKey = CSSettingsHelper.Instance.FaceApiKey;
                    FaceServiceHelper.ApiRoot = CSSettingsHelper.Instance.Location;
                    ImageAnalyzer.PeopleGroupsUserDataFilter = CSSettingsHelper.Instance.WorkspaceKey;
                    FaceListManager.FaceListsUserDataFilter = CSSettingsHelper.Instance.WorkspaceKey;
                    CoreUtil.MinDetectableFaceCoveragePercentage = CSSettingsHelper.Instance.MinDetectableFaceCoveragePercentage;
                };

                // callbacks for core library
                FaceServiceHelper.Throttled = () => ShowThrottlingToast("Face");
                EmotionServiceHelper.Throttled = () => ShowThrottlingToast("Emotion");
                ErrorTrackingHelper.TrackException = LogException;
                ErrorTrackingHelper.GenericApiCallExceptionHandler = FaceRecognitionHelper.GenericApiCallExceptionHandler;

                //Read API keys from the local storage
                CSSettingsHelper.Instance.Initialize();


                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame
                {
                    // Set the default language
                    Language = Windows.Globalization.ApplicationLanguages.Languages[0]
                };

                rootFrame.NavigationFailed += OnNavigationFailed;

                //  Display an extended splash screen if app was not previously running.
                if (e.PreviousExecutionState != ApplicationExecutionState.Running)
                {
                    //bool loadState = (e.PreviousExecutionState == ApplicationExecutionState.Terminated);
                    //ExtendedSplash extendedSplash = new ExtendedSplash(e.SplashScreen, loadState);
                    //rootFrame.Content = extendedSplash;
                    //Window.Current.Content = rootFrame;
                    Window.Current.Content = new Shell(rootFrame);

                }
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                    Window.Current.Content = new Shell(rootFrame);

                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        private static void LogException(Exception ex, string message)
        {
            Debug.WriteLine("Error detected! Exception: \"{0}\", More info: \"{1}\".", ex.Message, message);
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var logger = SimpleIoc.Default.GetInstance<ILogService>();
            if (logger != null)
                logger.FlushLogs();

            //Save application state and stop any background activity
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://contoso.azurewebsites.net" // URL of the Mobile App
        );

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
