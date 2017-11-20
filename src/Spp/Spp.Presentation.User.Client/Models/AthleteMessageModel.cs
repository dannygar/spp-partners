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
    class AthleteMessageModel : BaseModel
    {
        private IHttpClientService _dataService;
        private ILogService _logService;
        private ICacheService _cacheService;

        public AthleteMessageModel(IHttpClientService dataService, ILogService logService, ICacheService cacheService)
        {
            _dataService = dataService;
            _logService = logService;
            _cacheService = cacheService;
        }

        public async Task<List<Message>> GetTodaysAthleteMessages(User athlete, int sessionId)
        {
            if (athlete == null)
                return null;

            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_ATHLETEMESSAGES))
            {
                var cache = _cacheService.GetItem<Dictionary<int, Dictionary<int, List<Message>>>>(Defines.CACHE_KEY_ATHLETEMESSAGES);
                if (cache.ContainsKey(sessionId) && cache[sessionId].ContainsKey(athlete.Id))
                    return cache[sessionId][athlete.Id];
            }

            _logService.Info(String.Format("Getting messages for athlete: {0}, using: {1}", athlete.Id, _dataService.GetType().ToString()), this);

            var messages = await _dataService.GetItemAsync<List<Message>>(string.Format(Defines.API_MESSAGE_ENDPOINT, athlete.Id));

            // Cache messages
            this.CacheMessages(athlete, sessionId, messages);

            return messages;
        }

        public async Task CacheAllMessages(List<User> athletes, List<int> sessionIds)
        {
            var messageCache = new Dictionary<int, Dictionary<int, List<Message>>>();
            
            foreach (var sessionId in sessionIds)
            {
                var sessionCache = new Dictionary<int, List<Message>>();
                messageCache.Add(sessionId, sessionCache);

                foreach (var athlete in athletes)
                    sessionCache.Add(athlete.Id, await this.GetTodaysAthleteMessages(athlete, sessionId));
            }

            _cacheService.SetItem<Dictionary<int, Dictionary<int, List<Message>>>>(Defines.CACHE_KEY_ATHLETEMESSAGES, messageCache);
        }

        public void CacheMessages(User athlete, int sessionId, List<Message> messages)
        {
            var cache = _cacheService.GetItem<Dictionary<int, Dictionary<int, List<Message>>>>(Defines.CACHE_KEY_ATHLETEMESSAGES);

            if (cache == null)
                cache = new Dictionary<int, Dictionary<int, List<Message>>>();

            if (!cache.ContainsKey(sessionId))
                cache.Add(sessionId, new Dictionary<int, List<Message>>());

            if (!cache[sessionId].ContainsKey(athlete.Id))
                cache[sessionId].Add(athlete.Id, messages);
            else
                cache[sessionId][athlete.Id] = messages;

            _cacheService.SetItem<Dictionary<int, Dictionary<int, List<Message>>>>(Defines.CACHE_KEY_ATHLETEMESSAGES, cache);
        }
    }
}