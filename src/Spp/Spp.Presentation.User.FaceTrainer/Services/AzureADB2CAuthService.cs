/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Identity.Client;

namespace FaceAPITrainer.Services
{
    public class AzureADB2CAuthService : IB2CAuthService
    {
        public event EventHandler<int> Authenticated;

        private string userId { get; set; }

        public int TeamId { get; set; }

        private ITypedDataService _dataService;


        private static string[] ApiScopes = { $"{Defines.Audience}/demo.read" };
        private static string BaseAuthority = "https://login.microsoftonline.com/tfp/{tenant}/{policy}/oauth2/v2.0/authorize";
        private static string Authority = BaseAuthority.Replace("{tenant}", Defines.B2CTenant).Replace("{policy}", Defines.SignInPolicy);

        private PublicClientApplication PublicClientApp { get; } = new PublicClientApplication(Defines.B2CClientId, Authority);


        public AzureADB2CAuthService()
        {
            this._dataService = new HttpClientService(new AzureADApiAuthService());
        }

        public string GetUserId()
        {
            return this.userId;
        }

        public async Task<int> AuthenticateUserAsync()
        {
            if (Defines.SkipAuthentication)
            {
                //Bubble up the authentication event
                this.Authenticated?.Invoke(this, 1);
                return 1;
            }


            // Always force user to enter credentials
            try
            {
                var result = await this.SignInUp();

                if (result != null)
                {
                    //Store the userId object in the local cache
                    this.userId = result.UniqueId;
                    //ApplicationData.Current.RoamingSettings.Values[Defines.UserId] = result.UniqueId;

                    //Authenticate at the backend DB via the API's call
                    var teamId = await GetTeamIdAsync(this.userId);

                    //Bubble up the authentication event
                    this.Authenticated?.Invoke(this, teamId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return 0;

        }


        private async Task<AuthenticationResult> SignInUp()
        {
            try
            {
                return await this.PublicClientApp.AcquireTokenAsync(
                    ApiScopes,
                    GetUserByPolicy(PublicClientApp.Users, Defines.SignInPolicy),
                    UIBehavior.ForceLogin,
                    null,
                    null,
                    Authority);
            }
            catch (MsalServiceException msex)
            {
                if (msex.Message.StartsWith(MSALErrorCodes.USER_CANCELED)) // User canceled
                {
                    //Return to the main window
                    return null;
                }

                if (msex.Message.StartsWith(MSALErrorCodes.SIGNUP_CANCELED)) // Canceled Sign-up
                {
                    //Return to the previous dialog window
                    return await this.SignInUp();
                }

                if (msex.Message.StartsWith(MSALErrorCodes.PASSWORD_RESET)) // Password Reset
                {
                    //Provide Password Reset form
                    return await this.PasswordReset();
                }

                throw new MsalException(msex.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private async Task<AuthenticationResult> PasswordReset()
        {
            try
            {
                return await this.PublicClientApp.AcquireTokenAsync(
                    ApiScopes,
                    GetUserByPolicy(PublicClientApp.Users, Defines.PasswordResetPolicy),
                    UIBehavior.ForceLogin,
                    null,
                    null,
                    Authority);
            }
            catch (MsalServiceException)
            {
                //Return to the main window
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private IUser GetUserByPolicy(IEnumerable<IUser> users, string policy)
        {
            foreach (var user in users)
            {
                string userIdentifier = Base64UrlDecode(user.Identifier.Split('.')[0]);
                if (userIdentifier.EndsWith(policy.ToLower())) return user;
            }

            return null;
        }


        private string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }



        public async Task<int> GetTeamIdAsync(string userId)
        {
            var url = $"{Defines.API_BASE_URL}{Defines.API_AUTHENTICATION_ENDPOINT}/{userId}";

            this.TeamId = await this._dataService.GetItemAsync<int>(url);

            return this.TeamId;
        }

    }
}