/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftSportsScience.Models
{
    class AthleteAnswerModel : BaseModel
    {
        private IHttpClientService _dataService;
        private ILogService _logService;
        private InMemoryCacheService _cache;

        public AthleteAnswerModel(IHttpClientService dataService, ILogService logService)
        {
            _dataService = dataService;
            _logService = logService;
            _cache = new InMemoryCacheService(_logService);
        }

        public async Task<List<AthleteQuestionHistoryEntry>> GetAnswers(AthleteQuestionHistory question)
        {
            if(question != null)
                return (List<AthleteQuestionHistoryEntry>) await Task.Run(() => question.Responses);

            return null;
        }

        public async Task<bool> GetPlayerResponded(int playerId, int questionnaireId = 1)
        {
            // Check cache
            var cacheKey = string.Format(Defines.CACHE_KEY_PLAYER_RESPONDED, playerId);
            if (_cache.IsCached(cacheKey))
                return _cache.GetItem<bool>(cacheKey);


            var url = string.Format(Defines.API_PLAYER_RESPONDED, playerId, questionnaireId);

            _logService.Info($"Checking if player {playerId} responded to questionnaire {questionnaireId} using: {_dataService.GetType()}.", this);
            var result = await _dataService.GetItemAsync<bool>(url);

            //Add to cache
            _cache.SetItem<bool>(cacheKey, result);

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerResponse"></param>
        /// <returns></returns>
        public async Task SetAnswers(PlayerResponse playerResponse)
        {
            if (playerResponse == null)
                return;

            _logService.Info(String.Format("Settings {0} answers using: {1}", playerResponse.Answers.Count, _dataService.GetType().ToString()), this);
            await _dataService.SetItemAsync<PlayerResponse>(Defines.API_RESPONSE_ENDPOINT, playerResponse);
        }


    }
}
