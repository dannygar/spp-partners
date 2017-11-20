/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace FaceAPITrainer.Services
{
    internal class AzureADApiAuthService : IApiAuthService
    {
        private static string _authority = string.Format(CultureInfo.InvariantCulture,
            Defines.AADInstance, Defines.B2BTenant);

        static Uri redirectURI;

        private AuthenticationResult _authResult;

        public AzureADApiAuthService()
        {
            redirectURI = Windows.Security.Authentication.Web.WebAuthenticationBroker.GetCurrentApplicationCallbackUri();
        }

        public async Task<bool> AuthenticateAsync()
        {
            try
            {
                TokenCache.DefaultShared.Clear();
                var authContext = new AuthenticationContext(_authority, TokenCache.DefaultShared);
                var credentials = new ClientCredential(Defines.B2BClientId, Defines.B2BClientKey);
                this._authResult = await authContext.AcquireTokenAsync(Defines.Audience, credentials);
                return true;
            }
            catch (Exception)
            {
                var userObjectId = GetHardwareId();
                var authContext = new AuthenticationContext(_authority, new AuthSessionCache(userObjectId));
                var credentials = new ClientCredential(Defines.B2BClientId, Defines.B2BClientKey);

                this._authResult = await authContext.AcquireTokenAsync(Defines.Audience, credentials);
                return true;
            }

            return false;
        }

        public async Task<string> GetAuthToken()
        {
            if (this._authResult != null)
                return this._authResult.AccessToken;

            // Try to re-auth
            try
            {
                await this.AuthenticateAsync();

                if (this._authResult != null && this._authResult.AccessToken != null)
                    return this._authResult.AccessToken;
            }
            catch (Exception ex)
            {
                // Unable to auth
                throw;
            }

            return null;
        }


        private string GetHardwareId()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["uniqueDeviceId"];
            if (value == null)
            {
                value = Guid.NewGuid();
                localSettings.Values["uniqueDeviceId"] = value;
            }
            return value.ToString();
        }

    }
}
