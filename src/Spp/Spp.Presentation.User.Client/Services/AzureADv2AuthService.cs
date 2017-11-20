/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Identity.Client;
using MicrosoftSportsScience.Models;
using MicrosoftSportsScience.ViewModels;

namespace MicrosoftSportsScience.Services
{
    // The following using statements were added for this sample.
    using AuthenticationResult = Microsoft.Identity.Client.AuthenticationResult;

    public class AzureADv2AuthService : IApiAuthService
    {
        private static string[] ApiScopes = { "user.read" };

        public static PublicClientApplication PublicClientApp;


        private string userId { get; set; }

        private readonly ILogService logService;

        private AppSessionModel sessionModel;

        public static ConfigurationSettingsViewModel AppSettings;

        public AzureADv2AuthService(ILogService logService)
        {
            this.logService = logService;
            AppSettings = new ConfigurationSettingsViewModel();
        }

        public async Task<string> GetAuthToken()
        {
            var authResult = await AcquireAuthToken(ApiScopes);
            return authResult.IdToken;
        }

        public string GetUserId()
        {
            return this.userId;
        }



        public static async Task LoadConfig()
        {
            //Load configuration settings
            await AppSettings.Load();

            //Assign the API endpoint to a static public variable
            Defines.API_BASE_URL = AppSettings.AppConfigurationSettings.APIEndpointUrl;

            // Authority is the URL for authority, composed by Azure Active Directory v2 endpoint and the tenant name (e.g. https://login.microsoftonline.com/contoso.onmicrosoft.com/v2.0)
            var authority = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "https://login.microsoftonline.com/{0}/v2.0",
                AppSettings.AppConfigurationSettings.TenantId);
            
            //Public Client Application
            PublicClientApp = new PublicClientApplication(AppSettings.AppConfigurationSettings.ClientId, authority)
                { RedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient" };

        }


        public async Task<int> AuthenticateUserAsync()
        {
            this.sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
            //If the user has already been authenticated and the TeamID was cached, return it without the sign-in
            if (this.sessionModel.TeamId > 0) return this.sessionModel.TeamId;

            //Authenticate against Azure AD
            try
            {
                //Reload configuration settings
                await LoadConfig();

                //Authenticate at the backend DB via the API's call
                return await this.sessionModel.AuthenticateADUserAsync();
            }
            catch (Exception ex)
            {
                // An unexpected error occurred.
                this.logService.Error(ex.Message, this);
                if (ex.InnerException != null)
                {
                    this.logService.Error(ex.InnerException.Message, this);
                }
            }

            return 0;

        }




        public void SignOut()
        {
            //Sign out
            PublicClientApp.Remove(PublicClientApp.Users.FirstOrDefault());

            //Clear cookie cache
            const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
        }


        /// <summary>
        /// Try to silently acquire auth token
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns></returns>
        private async Task<AuthenticationResult> AcquireAuthToken(string[] scopes)
        {
            AuthenticationResult authResult = null;

            try
            {
                authResult = await PublicClientApp.AcquireTokenSilentAsync(scopes, PublicClientApp.Users.FirstOrDefault());
                return authResult;
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                this.logService.Error($"MsalUiRequiredException: {ex.Message}", this);

                try
                {
                    authResult = await PublicClientApp.AcquireTokenAsync(scopes);
                    return authResult;
                }
                catch (MsalException msalex)
                {
                    this.logService.Error($"Error Acquiring Token:{System.Environment.NewLine}{msalex}", this);
                }
            }
            catch (Exception ex)
            {
                this.logService.Error($"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}", this);
                return null;
            }

            return null;
        }


        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

    }
}