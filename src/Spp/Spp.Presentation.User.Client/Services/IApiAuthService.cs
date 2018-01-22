/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Threading.Tasks;

namespace Spp.Presentation.User.Client.Services
{
    public interface IApiAuthService
    {
        Task<int> AuthenticateUserAsync();

        Task<string> GetAuthToken();

        string GetUserId();

        void SignOut();

    }
}
