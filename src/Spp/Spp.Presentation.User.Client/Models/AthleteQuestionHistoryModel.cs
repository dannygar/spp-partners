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
    class AthleteQuestionHistoryModel : BaseModel
    {
        private IHttpClientService _dataService;
        private ILogService _logService;
        private ICacheService _cacheService;

        public AthleteQuestionHistoryModel(IHttpClientService dataService, ILogService logService, ICacheService cacheService)
        {
            _dataService = dataService;
            _logService = logService;
            _cacheService = cacheService;
        }

        public async Task<List<AthleteQuestionHistory>> GetHistory(User athlete, int sessionId)
        {
            if (athlete == null)
                return null;

            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_ATHLETEHISTORY))
            {
                var cache = _cacheService.GetItem<Dictionary<int, Dictionary<int, List<AthleteQuestionHistory>>>>(Defines.CACHE_KEY_ATHLETEHISTORY);
                if (cache.ContainsKey(sessionId) && cache[sessionId].ContainsKey(athlete.Id))
                    return cache[sessionId][athlete.Id];
            }

            // Get questionnaire
            //_logService.Info(String.Format("Getting questions for session: {0}, using: {1}", sessionId, _dataService.GetType().ToString()), this);
            //var questionnaires = await _dataService.GetItemAsync<List<AthleteQuestionnaire>>(String.Format(Defines.API_QUESTIONS_ENDPOINT, sessionId));

            //if (questionnaires == null)
            //    return null;

            //var questionnaire = questionnaires[0]; // Get the first one for now, in the future we need to select by type here probably

            //if (questionnaire == null)
            //    return null;

            // Get the history
            _logService.Info($"Getting history for athlete: {athlete.Id} and the session {sessionId}, using: {_dataService.GetType()}", this);
            var historyResponses = await _dataService.GetItemAsync<List<AthleteQuestionHistory>>(
                string.Format(Defines.API_QUESTIONS_RESPONSE_ENDPOINT, sessionId, athlete.Id));

            //var questions = new Dictionary<int, AthleteQuestionHistory>();
            //foreach (var item in combinedResult.Responses)
            //{
            //    if (!questions.ContainsKey(item.QuestionId))
            //        questions.Add(item.QuestionId, new AthleteQuestionHistory()
            //        {
            //            //Athlete = athlete,
            //            //Question = questionnaire.Questions.FirstOrDefault(x => x.Id.Equals(item.QuestionId))
            //        });

            //    // Temp code to fix data issues
            //    //if (questions[item.QuestionId].Question == null)
            //    //    questions[item.QuestionId].Question = questionnaire.Questions[0];

            //    questions[item.QuestionId].Responses.Add(item);
            //}

            //var histories = questions.Keys.Select(x => questions[x]).ToList();
            this.CacheHistories(athlete, sessionId, historyResponses);

            return historyResponses;
        }

        public async Task CacheAllHistories(List<User> athletes, List<int> sessionIds)
        {
            var historyCache = new Dictionary<int, Dictionary<int, List<AthleteQuestionHistory>>>();

            foreach (var sessionId in sessionIds)
            {
                var sessionCache = new Dictionary<int, List<AthleteQuestionHistory>>();
                historyCache.Add(sessionId, sessionCache);

                foreach (var athlete in athletes)
                    sessionCache.Add(athlete.Id, await this.GetHistory(athlete, sessionId));
            }

            _cacheService.SetItem<Dictionary<int, Dictionary<int, List<AthleteQuestionHistory>>>>(Defines.CACHE_KEY_ATHLETEHISTORY, historyCache);
        }

        public void CacheHistories(User athlete, int sessionId, List<AthleteQuestionHistory> histories)
        {
            var cache = _cacheService.GetItem<Dictionary<int, Dictionary<int, List<AthleteQuestionHistory>>>>(Defines.CACHE_KEY_ATHLETEHISTORY);

            if (cache == null)
                cache = new Dictionary<int, Dictionary<int, List<AthleteQuestionHistory>>>();

            if (!cache.ContainsKey(sessionId))
                cache.Add(sessionId, new Dictionary<int, List<AthleteQuestionHistory>>());

            if (!cache[sessionId].ContainsKey(athlete.Id))
                cache[sessionId].Add(athlete.Id, histories);
            else
                cache[sessionId][athlete.Id] = histories;

            _cacheService.SetItem<Dictionary<int, Dictionary<int, List<AthleteQuestionHistory>>>>(Defines.CACHE_KEY_ATHLETEHISTORY, cache);
        }
    }
}
