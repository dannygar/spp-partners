/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Data.Services
{
    using System.Threading.Tasks;

    public interface IApiAuthService
    {
        Task<int> AuthenticateUserAsync();

        Task<string> GetAuthToken();

        string GetUserId();

        void SignOut();
    }
}
