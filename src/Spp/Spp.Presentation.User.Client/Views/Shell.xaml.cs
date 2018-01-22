/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Helpers;
using Spp.Presentation.User.Client.Models;
using Spp.Presentation.User.Client.Services;
using Spp.Presentation.User.Client.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Spp.Presentation.User.Client
{
    public sealed partial class Shell : Page
    {
        public static SplitView Current;

        public Shell(Frame frame)
        {
            InitializeComponent();
            var sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();

            Current = ShellSplitView;
            sessionModel.ContentView = Current;
            sessionModel.AppShell = this;

            ShellSplitView.Content = frame;

            //Load configuration settings
            AzureADv2AuthService.LoadConfig().ConfigureAwait(false).GetAwaiter().GetResult();

            if (AzureADv2AuthService.AppSettings.IsValid)
            {
                //If Face Recognition is enabled, switch to the Face Recognition Sign in option
                if (CSSettingsHelper.Instance.EnableFaceRecognition && CSSettingsHelper.Instance.IsValidSettings())
                    (ShellSplitView.Content as Frame).Navigate(typeof(GreetingPage));
                else
                    (ShellSplitView.Content as Frame).Navigate(typeof(SignIn));
            }
            else
                (ShellSplitView.Content as Frame).Navigate(typeof(SettingsPage), this);


        }

        private void HamburgerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ShellSplitView.IsPaneOpen = !ShellSplitView.IsPaneOpen;
        }

        public void HideSideMenu()
        {
            PaneGrid.Visibility = Visibility.Collapsed;
        }

        private void HamburgerRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.ShellSplitView.IsPaneOpen)
                this.ShellSplitView.IsPaneOpen = true;
        }

        private void MenuButton1_Click(object sender, RoutedEventArgs e)
        {
            var sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
            (Current.Content as Frame).Navigate(typeof(SignIn));
        }

        private void MenuButton6_Click(object sender, RoutedEventArgs e)
        {
            (Current.Content as Frame).Navigate(typeof(Workouts));
        }

        private void MenuButton5_Click(object sender, RoutedEventArgs e)
        {
            (Current.Content as Frame).Navigate(typeof(Calendar));
        }
        private void MenuButton2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuButton4_Click(object sender, RoutedEventArgs e)
        {
            (ShellSplitView.Content as Frame).Navigate(typeof(SessionPlanner));
        }

        public void SetUserPhoto(string url)
        {
            ProfileImage.Source = new BitmapImage(new Uri(url));
        }

        private void MenuButton3_Click(object sender, RoutedEventArgs e)
        {
            (ShellSplitView.Content as Frame).Navigate(typeof(Workouts));
        }

        private void OpenWorkouts(object sender, RoutedEventArgs e)
        {
            (ShellSplitView.Content as Frame).Navigate(typeof(Workouts));
        }

        private void MenuButton7_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (ShellSplitView.Content as Frame).Navigate(typeof(WorkoutPlanManager));
        }

        private void MenuButton8_Click(object sender, RoutedEventArgs e)
        {
            (Current.Content as Frame).Navigate(typeof(SettingsPage), this);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var frame = Current.Content as Frame;
            if (frame?.CanGoBack == true)
                frame.GoBack();
        }
    }
}
