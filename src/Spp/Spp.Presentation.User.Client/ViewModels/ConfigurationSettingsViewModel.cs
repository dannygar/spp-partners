/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using MicrosoftSportsScience.Helpers;
using MicrosoftSportsScience.Models;

namespace MicrosoftSportsScience.ViewModels
{
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:BracesMustNotBeOmitted", Justification = "Reviewed.")]
    public class ConfigurationSettingsViewModel : NotificationBase
    {
        public ConfigurationSettings AppConfigurationSettings;


        public bool IsValid => !string.IsNullOrEmpty(AppConfigurationSettings.APIEndpointUrl) && 
                               !string.IsNullOrEmpty(AppConfigurationSettings.ClientId) && 
                               !string.IsNullOrEmpty(AppConfigurationSettings.TenantId) &&
                               !string.IsNullOrEmpty(AppConfigurationSettings.MLEndpointUrl) &&
                               !string.IsNullOrEmpty(AppConfigurationSettings.MLClientKey);

        public override async Task Load()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            this.AppConfigurationSettings = new ConfigurationSettings()
            {
                APIEndpointUrl = (string)localSettings.Values["APIEndpointUrl"],
                ClientId = (string)localSettings.Values["ClientId"],
                TenantId = (string)localSettings.Values["TenantId"],
                MLEndpointUrl = (string)localSettings.Values["MLEndpointUrl"],
                MLClientKey = (string)localSettings.Values["MLClientKey"],
                SessionDate = DateTime.TryParse((string)localSettings.Values["SessionDate"],
                    out DateTime sessionDate) ? sessionDate : DateTime.UtcNow
            };

            //this.AppConfigurationSettings.SessionDate = sessionDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionDate"></param>
        /// <returns></returns>
        public void Save(DateTime sessionDate)
        {
            if (AppConfigurationSettings == null) return;

            this.AppConfigurationSettings.SessionDate = sessionDate;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            localSettings.Values["APIEndpointUrl"] = this.AppConfigurationSettings.APIEndpointUrl;
            localSettings.Values["ClientId"] = this.AppConfigurationSettings.ClientId;
            localSettings.Values["TenantId"] = this.AppConfigurationSettings.TenantId;
            localSettings.Values["MLEndpointUrl"] = this.AppConfigurationSettings.MLEndpointUrl;
            localSettings.Values["MLClientKey"] = this.AppConfigurationSettings.MLClientKey;
            localSettings.Values["SessionDate"] = this.AppConfigurationSettings.SessionDate.ToString("d");
        }
    }
}