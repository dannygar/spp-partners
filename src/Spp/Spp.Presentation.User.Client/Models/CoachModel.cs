/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MicrosoftSportsScience.Models
{
    public class CoachModel : BaseModel
    {
        private IHttpClientService _dataService;
        private ILogService _logService;
        private ICacheService _cacheService;

        public CoachModel(IHttpClientService dataService, ILogService logService, ICacheService cacheService)
        {
            _dataService = dataService;
            _logService = logService;
            _cacheService = cacheService;
        }

        public async Task<List<Coach>> GetCoaches()
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_COACHES))
            {
                var cache = _cacheService.GetItem<Dictionary<int, Coach>>(Defines.CACHE_KEY_COACHES);
                if (cache != null)
                    return cache.Values.ToList();
            }

            _logService.Info(String.Format("Getting coaches using: {0}", _dataService.GetType().ToString()), this);
            var coaches =  await _dataService.GetItemAsync<List<Coach>>(Defines.API_COACHES_ENDPOINT);
            this.CacheCoaches(coaches);

            return coaches;
        }

        public async Task CacheAllCoaches()
        {
            var coachCache = new Dictionary<int, Coach>();
            var coaches = await this.GetCoaches();

            foreach (var coach in coaches)
                coachCache.Add(coach.CoachId, coach);

            _cacheService.SetItem<Dictionary<int, Coach>>(Defines.CACHE_KEY_COACHES, coachCache);
        }

        public void CacheCoaches(List<Coach> coaches)
        {
            var cache = _cacheService.GetItem<Dictionary<int, Coach>>(Defines.CACHE_KEY_COACHES);

            if (cache == null)
                cache = new Dictionary<int, Coach>();

            cache.Clear();
            foreach (var coach in coaches)
                cache.Add(coach.Id, coach);

            _cacheService.SetItem<Dictionary<int, Coach>>(Defines.CACHE_KEY_COACHES, cache);
        }
    }
}