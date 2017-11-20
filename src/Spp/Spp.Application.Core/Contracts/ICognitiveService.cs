/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using Spp.Application.Core.Models;

namespace Spp.Application.Core.Contracts
{
    public interface ICognitiveService : IEntityConfiguration
    {
        Task<bool> CreateCognitiveServiceKeys(CognitiveServiceKeysDto newKeysDto);

        Task<bool> AddUserEmotions(EmotionsDto emotionsDto);

        Task<CognitiveServiceKeysDto> GetCognitiveServiceKeys(int teamId);

        Task<int> AuthenticateUserByFace(IList<UserIdentityDto> userIdentities);

        Task<bool> UpdateCognitiveServiceKeys(CognitiveServiceKeysDto newKeysDto);

        Task<bool> DeleteCognitiveServiceKeys(int keyId);
    }
}
