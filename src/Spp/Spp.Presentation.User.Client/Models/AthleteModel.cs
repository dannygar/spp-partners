/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Data.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MicrosoftSportsScience.Models
{
    public class AthleteModel : BaseModel
    {
        private ITypedDataService _dataService;
        private ILogService _logService;
        private ICacheService _cacheService;

        public AthleteModel(ITypedDataService dataService, ILogService logService, ICacheService cacheService)
        {
            _dataService = dataService;
            _logService = logService;
            _cacheService = cacheService;
        }

        public async Task<Athlete> GetAthelete()
        {
            return await Task.Run(() => new Athlete());
        }

        public async Task<List<Athlete>> GetAtheletes()
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_ATHLETES))
            {
                var cache = _cacheService.GetItem<Dictionary<int, Athlete>>(Defines.CACHE_KEY_ATHLETES);
                if (cache != null)
                    return cache.Values.ToList();
            }

            var url = ConstructUrl(Defines.API_BASE_URL, Defines.API_PLAYERS_ENDPOINT);

            _logService.Info(String.Format("Getting athletes using: {0}", _dataService.GetType().ToString()), this);
            var athletes = await _dataService.GetItemAsync<List<Athlete>>(url);
            this.CacheAthletes(athletes);

            return athletes;
        }

        public async Task SetAthletes(string name, List<Athlete> athletes)
        {
            if (name == null || athletes == null)
                return;

            _logService.Info(String.Format("Setting {0} athletes for team: {1}, using: {2}", athletes.Count, name, _dataService.GetType().ToString()), this);
            await _dataService.SetItemAsync<List<Athlete>>(name, athletes);
        }

        public async Task CacheAllAthletes()
        {
            var athleteCache = new Dictionary<int, Athlete>();
            var athletes = await this.GetAtheletes();

            foreach (var athlete in athletes)
                athleteCache.Add(athlete.PlayerId, athlete);

            _cacheService.SetItem<Dictionary<int, Athlete>>(Defines.CACHE_KEY_ATHLETES, athleteCache);
        }

        public void CacheAthletes(List<Athlete> athletes)
        {
            var cache = _cacheService.GetItem<Dictionary<int, Athlete>>(Defines.CACHE_KEY_ATHLETES);

            if (cache == null)
                cache = new Dictionary<int, Athlete>();

            cache.Clear();
            foreach (var athlete in athletes)
                cache.Add(athlete.Id, athlete);

            _cacheService.SetItem<Dictionary<int, Athlete>>(Defines.CACHE_KEY_ATHLETES, cache);
        }
    }
}