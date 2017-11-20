/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using Spp.Application.Core.Models;

namespace Spp.Application.Core.Contracts
{
    public interface IQuestionnaireService : IEntityConfiguration
    {
        Task<int> CreateQuestionnaire(AthleteQuestionnaireDto questionnaireDto);

        Task<AthleteQuestionnaireDto> GetAthleteQuestions(int sessionId);

        Task<List<AthleteQuestionHistoryDto>> GetPlayerResponse(int sessionId, int playerId);

        Task<AthleteQuestionnaireDto> GetQuestionnaire(int questionnaireId);

        Task<List<AthleteQuestionnaireDto>> GetSessionQuestionnaires(int sessionId);

        Task<bool> UpdateQuestionnaire(AthleteQuestionnaireDto questionnaireDto);

        Task<bool> DeleteQuestionnaire(int questionnaireId);

    }
}
