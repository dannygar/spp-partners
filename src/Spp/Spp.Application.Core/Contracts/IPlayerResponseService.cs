/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Threading.Tasks;
using Spp.Application.Core.Models;

namespace Spp.Application.Core.Contracts
{
    public interface IPlayerResponseService : IEntityConfiguration
    {
        Task<bool> CreatePlayerResponse(PlayerResponseDto playerResponseDto);

        Task<PlayerResponseDto> GetPlayerResponse(PlayerResponseDto playerQuestionnaire);

        Task<bool> GetQuestionnairePlayerResponse(int playerId, int questionnaireId);

        Task<bool> UpdatePlayerResponse(PlayerResponseDto playerResponseDto);

        Task<bool> DeletePlayerResponse(int questionId);

        Task<bool> DeleteAllPlayerResponses(int playerId);
    }
}
