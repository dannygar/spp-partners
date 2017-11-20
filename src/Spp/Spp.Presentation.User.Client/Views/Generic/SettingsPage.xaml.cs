/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: http://www.microsoft.com/cognitive
// 
// Microsoft Cognitive Services Github:
// https://github.com/Microsoft/Cognitive
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Helpers;
using MicrosoftSportsScience.Models;
using MicrosoftSportsScience.Services;
using MicrosoftSportsScience.ViewModels;
using MicrosoftSportsScience.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MicrosoftSportsScience
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public ConfigurationSettingsViewModel AppSettingsViewModel;
        private Type _parentPageType;


        #region Dependency Properties

        //public string APIEndpointUrl
        //{
        //    get => AppSettingsViewModel.AppConfigurationSettings.APIEndpointUrl;
        //    set => AppSettingsViewModel.AppConfigurationSettings.APIEndpointUrl = value;
        //}

        //// Using a DependencyProperty as the backing store for APIEndpointUrl.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty APIEndpointUrlProperty =
        //    DependencyProperty.Register("APIEndpointUrl", typeof(string), typeof(SettingsPage), new PropertyMetadata(null));


        public bool EnableFaceRecognition
        {
            get => CSSettingsHelper.Instance.EnableFaceRecognition;
            set => CSSettingsHelper.Instance.EnableFaceRecognition = value;
        }

        // Using a DependencyProperty as the backing store for EnableFaceRecognition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableFaceRecognitionProperty =
            DependencyProperty.Register("EnableFaceRecognition", typeof(bool), typeof(SettingsPage), new PropertyMetadata(null));


        public uint MinDetectableFaceCoveragePercentage
        {
            get => CSSettingsHelper.Instance.MinDetectableFaceCoveragePercentage;
            set => CSSettingsHelper.Instance.MinDetectableFaceCoveragePercentage = value;
        }

        // Using a DependencyProperty as the backing store for MinDetectableFaceCoveragePercentage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinDetectableFaceCoveragePercentageProperty =
            DependencyProperty.Register("MinDetectableFaceCoveragePercentage", typeof(uint), typeof(SettingsPage), new PropertyMetadata(null));



        #endregion

        public SettingsPage()
        {
            this.AppSettingsViewModel = new ConfigurationSettingsViewModel();
            this.InitializeComponent();
            //this.DataContext = CSSettingsHelper.Instance;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this._parentPageType = e.Parameter?.GetType();

            await LoadData();
            base.OnNavigatedFrom(e);
        }


        private async Task LoadData()
        {
            await this.AppSettingsViewModel.Load();

            this.cameraSourceComboBox.ItemsSource = await FaceRecognitionHelper.GetAvailableCameraNamesAsync();
            this.cameraSourceComboBox.SelectedItem = CSSettingsHelper.Instance.CameraName;

            this.EnableFaceRecognition = CSSettingsHelper.Instance.EnableFaceRecognition;
            this.MinDetectableFaceCoveragePercentage = CSSettingsHelper.Instance.MinDetectableFaceCoveragePercentage;

            this.SessionDatePicker.Date = DateTime.SpecifyKind(this.AppSettingsViewModel.AppConfigurationSettings.SessionDate, DateTimeKind.Local);
            this.DataContext = this;
        }


        private async void OnResetSettingsClick(object sender, RoutedEventArgs e)
        {
            await FaceRecognitionHelper.ConfirmActionAndExecute("This will reset all local settings and erase your changes. Confirm?",
                async () =>
                {
                    await Task.Run(() => CSSettingsHelper.Instance.RestoreAllSettings());
                    await LoadData();
                    await new MessageDialog("Settings restored. Please restart the application to load the default settings.").ShowAsync();
                });
        }

        private void OnCameraSourceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cameraSourceComboBox.SelectedItem != null)
            {
                CSSettingsHelper.Instance.CameraName = this.cameraSourceComboBox.SelectedItem.ToString();
            }
        }

        private async void OnEraseRealtimeDataClicked(object sender, RoutedEventArgs e)
        {
            var rootFolder = await ApplicationData.Current.LocalFolder.TryGetItemAsync("FaceRecognition") as IStorageFolder;
            if (rootFolder != null)
            {
                foreach (var item in await rootFolder.GetItemsAsync())
                {
                    try
                    {
                        await item.DeleteAsync();
                    }
                    catch
                    { }
                }
            }
        }

        private void OnSaveSettingsClicked(object sender, RoutedEventArgs e)
        {
            //Update Session Date
            var sessionDate = SessionDatePicker.Date;

            this.AppSettingsViewModel.Save(sessionDate.DateTime);

            var authService = SimpleIoc.Default.GetInstance<IApiAuthService>();
            authService.SignOut();


            var sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();

            (sessionModel.ContentView.Content as Frame).Navigate(CSSettingsHelper.Instance.EnableFaceRecognition
                ? typeof(GreetingPage)
                : typeof(SignIn));
        }

        private async void OnClearCacheClicked(object sender, TappedRoutedEventArgs e)
        {
            var authService = SimpleIoc.Default.GetInstance<IApiAuthService>();
            authService.SignOut();
            await new MessageDialog("Sign-in Cache was cleared.", "Sign-in").ShowAsync();
        }

        private void OnExitClicked(object sender, TappedRoutedEventArgs e)
        {
            var sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();

            if (this._parentPageType != null)
            {
                (sessionModel.ContentView.Content as Frame).Navigate(this._parentPageType);
            }
            else
            {
                (sessionModel.ContentView.Content as Frame).Navigate(CSSettingsHelper.Instance.EnableFaceRecognition
                    ? typeof(GreetingPage)
                    : typeof(SignIn));
            }
        }
    }
}
