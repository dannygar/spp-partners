/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing.OptionDetails;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FaceAPITrainer.Services;

namespace FaceAPITrainer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LandingPage : Page
    {
        public static string Title { get; } = "Overview";
        public static bool IsAuthenticated;

        private AzureADB2CAuthService _authSvc;
        private ITypedDataService _dataService;
        public event EventHandler<int> Authenticated;


        public LandingPage()
        {
            this.InitializeComponent();
            this._authSvc = new AzureADB2CAuthService();
            this._authSvc.Authenticated += LandingPage_Authenticated;
            this._dataService = new HttpClientService(new AzureADApiAuthService());

        }


        private async void LandingPage_Authenticated(object sender, int teamId)
        {
            if (teamId <= 0)
            {
                //Failed to authenticate
                this.Frame.Navigate(typeof(AccessDenied), string.Empty,
                    new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
                return;
            }

            LandingPage.IsAuthenticated = true;

            //Update Team's Cognitive Services Keys
            await UpdateSettings(teamId);

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(!LandingPage.IsAuthenticated)
                await this._authSvc.AuthenticateUserAsync();

        }



        public async Task UpdateSettings(int teamId)
        {
            var url = $"{Defines.API_BASE_URL}{Defines.API_COGNITIVESERVICES_ENDPOINT}/keys/{teamId}";

            var csKeys = await this._dataService.GetItemAsync<CognitiveServiceKeys>(url);

            if (!string.IsNullOrEmpty(csKeys?.WorkspaceKey))
            {
                SettingsHelper.Instance.Id = (int)csKeys?.Id;
                if(csKeys?.WorkspaceKey != SettingsHelper.Instance.WorkspaceKey) //Update the workspace key only if it is changed
                    SettingsHelper.Instance.WorkspaceKey = csKeys?.WorkspaceKey;

                SettingsHelper.Instance.TeamId = teamId;
                SettingsHelper.Instance.FaceApiKey = csKeys?.FaceApiKey;
                SettingsHelper.Instance.EmotionApiKey = csKeys?.EmotionApiKey;
                SettingsHelper.Instance.BingApiKey = csKeys?.BingApiKey;
                SettingsHelper.Instance.CameraName = csKeys?.CameraName;
                SettingsHelper.Instance.Location = csKeys?.Location;
                SettingsHelper.Instance.MinDetectableFaceCoveragePercentage = (int)
                    csKeys?.MinDetectableFaceCoveragePercentage;
            }
        }

    }


}
