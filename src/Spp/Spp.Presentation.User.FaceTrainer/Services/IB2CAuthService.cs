/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Threading.Tasks;

namespace FaceAPITrainer.Services
{
    public interface IB2CAuthService
    {
        Task<int> AuthenticateUserAsync();
        string GetUserId();

        Task<int> GetTeamIdAsync(string userId);
    }
}