/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Services;

namespace Spp.Presentation.User.Client.Models
{
    public class AthleteQuestionModel : BaseModel
    {
        public int QuestionnaireId { get; set; }
        private IHttpClientService _dataService;
        private ILogService _logService;
        private ICacheService _cacheService;

        public AthleteQuestionModel(IHttpClientService dataService, ILogService logService, ICacheService cacheService)
        {
            _dataService = dataService;
            _logService = logService;
            _cacheService = cacheService;
        }

        public async Task<IList<AthleteQuestion>> GetQuestions(int sessionId)
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_ATHLETEQUESTIONS))
            {
                var cache = _cacheService.GetItem<Dictionary<int, List<AthleteQuestion>>>(Defines.CACHE_KEY_ATHLETEQUESTIONS);
                if (cache.ContainsKey(sessionId))
                    return cache[sessionId];
            }

            _logService.Info(String.Format("Getting questions for session: {0}, using: {1}", sessionId, _dataService.GetType().ToString()), this);
            var questionnaire = await _dataService.GetItemAsync<AthleteQuestionnaire>(string.Format(Defines.API_QUESTIONS_ENDPOINT, sessionId));
            if (questionnaire == null)
                return null;

            var questions = questionnaire.Questions;
            this.QuestionnaireId = questionnaire.Id;

            this.CacheQuestions(sessionId, questions);

            return questions;
        }


        public async Task SetQuestions(int sessionId, List<AthleteQuestion> questions)
        {
            if (questions == null)
                return;

            _logService.Info(String.Format("Settings {0} questions, using: {1}", questions.Count, _dataService.GetType().ToString()), this);
            await _dataService.SetItemAsync<List<AthleteQuestion>>(null, questions);
        }


        public async Task SaveQuestionnaire(AthleteQuestionnaire questionnaire)
        {
            if (questionnaire == null)
                return;

            _logService.Info($"Saving practice session questionnaire, using: {_dataService.GetType()}", this);
            await _dataService.SetItemAsync<AthleteQuestionnaire>(Defines.API_QUESTIONS_ENDPOINT, questionnaire);
        }

        public async Task CacheAllQuestions(List<int> sessionIds)
        {
            var historyCache = new Dictionary<int, List<AthleteQuestion>>();

            foreach (var sessionId in sessionIds)
            {
                var sessionCache = new Dictionary<int, List<AthleteQuestion>>();
                historyCache.Add(sessionId, (List<AthleteQuestion>)await this.GetQuestions(sessionId));
            }

            _cacheService.SetItem<Dictionary<int, List<AthleteQuestion>>>(Defines.CACHE_KEY_ATHLETEQUESTIONS, historyCache);
        }

        public void CacheQuestions(int sessionId, List<AthleteQuestion> questions)
        {
            var cache = _cacheService.GetItem<Dictionary<int, List<AthleteQuestion>>>(Defines.CACHE_KEY_ATHLETEQUESTIONS);

            if (cache == null)
                cache = new Dictionary<int, List<AthleteQuestion>>();

            if (!cache.ContainsKey(sessionId))
                cache.Add(sessionId, questions);
            else
                cache[sessionId] = questions;

            _cacheService.SetItem<Dictionary<int, List<AthleteQuestion>>>(Defines.CACHE_KEY_ATHLETEQUESTIONS, cache);
        }
    }
}
