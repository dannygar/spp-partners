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
    public class AthletePracticeModel : BaseModel
    {
        private IHttpClientService _dataService;
        private ILogService _logService;
        private ICacheService _cacheService;

        public AthletePracticeModel(IHttpClientService dataService, ILogService logService, ICacheService cacheService)
        {
            _dataService = dataService;
            _logService = logService;
            _cacheService = cacheService;
        }

        public async Task<List<AthletePractice>> GetAthletePractices(List<int> sessionIds)
        {
            var practices = new List<AthletePractice>();

            foreach (var id in sessionIds)
                practices.Add(await GetAthletePracticeForSession(id));

            return practices;
        }

        public async Task<AthletePractice> GetAthletePracticeForSession(int sessionId)
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_ATHLETEPRACTICES))
            {
                var cache = _cacheService.GetItem<Dictionary<int, AthletePractice>>(Defines.CACHE_KEY_ATHLETEPRACTICES);
                if (cache.ContainsKey(sessionId))
                    return cache[sessionId];
            }

            var url = string.Format(Defines.API_PRACTICE_ENDPOINT, sessionId);

            _logService.Info(String.Format("Getting practice for session: {0}, using: {1}", sessionId, _dataService.GetType().ToString()), this);
            var practice =  await _dataService.GetItemAsync<AthletePractice>(url);
            this.CachePractice(sessionId, practice);

            return practice;
        }

        public async Task<AthletePractice> GetAthletePracticeById(int id)
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_ATHLETEPRACTICES_BYID))
            {
                var cache = _cacheService.GetItem<Dictionary<int, AthletePractice>>(Defines.CACHE_KEY_ATHLETEPRACTICES_BYID);
                if (cache.ContainsKey(id))
                    return cache[id];
            }

            var url = string.Format(Defines.API_PRACTICE_BYID_ENDPOINT, id);

            _logService.Info(String.Format("Getting practice for session: {0}, using: {1}", id, _dataService.GetType().ToString()), this);
            var practice = await _dataService.GetItemAsync<AthletePractice>(url);
            this.CachePracticeById(id, practice);

            return practice;
        }

        public async Task<IList<AthletePractice>> GetAthletePractices()
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_ATHLETEPRACTICES))
            {
                var cache = _cacheService.GetItem<List<AthletePractice>>(Defines.CACHE_KEY_ATHLETEALLPRACTICES);
                if (cache != null)
                    return cache;
            }


            _logService.Info(String.Format("Getting all practices, using: {0}", _dataService.GetType().ToString()), this);
            return await _dataService.GetItemAsync<List<AthletePractice>>(Defines.API_PRACTICES_ENDPOINT);
        }

        public async Task SetCoachNote(int drillId, string note)
        {
            var url = string.Format(Defines.API_COACH_NOTES_ENDPOINT, drillId, note);

            _logService.Info(String.Format("Setting coach note on drill: {0}, using: {1}", drillId, _dataService.GetType().ToString()), this);
            await _dataService.SetItemAsync<object>(url, null);
        }



        public async Task<IList<PracticeDrill>> GetPracticeDrills(int practiceId)
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_PRACTICEDRILLS))
            {
                var cache = _cacheService.GetItem<List<PracticeDrill>>(Defines.CACHE_KEY_PRACTICEDRILLS);
                if (cache != null)
                    return cache;
            }

            _logService.Info($"Getting all practice's drills, using: {_dataService.GetType()}", this);

            return await _dataService.GetItemAsync<List<PracticeDrill>>(string.Format(Defines.API_PRACTICEDRILLS_ENDPOINT, practiceId));
        }


        public async Task<IList<PracticeDrill>> GetAllDrills()
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_PRACTICEALLDRILLS))
            {
                var cache = _cacheService.GetItem<List<PracticeDrill>>(Defines.CACHE_KEY_PRACTICEALLDRILLS);
                if (cache != null)
                    return cache;
            }

            _logService.Info($"Getting all practice drills, using: {_dataService.GetType()}", this);

            return await _dataService.GetItemAsync<List<PracticeDrill>>(Defines.API_ALLPRACTICEDRILLS_ENDPOINT);
        }


        /// <summary>
        /// Saves a new Practice Session to DB
        /// </summary>
        /// <param name="practice"></param>
        /// <returns></returns>
        public async Task SaveAthletePracticeForSession(AthletePractice practice)
        {
            _logService.Info($"Saving a new practice session using: {_dataService.GetType()}", this);
            await _dataService.SetItemAsync<object>(Defines.API_PRACTICES_ENDPOINT, practice);
        }

        /// <summary>
        /// Updates a new Practice Session in the DB
        /// </summary>
        /// <param name="practice"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAthletePractice(AthletePractice practice)
        {
            _logService.Info($"Saving a new practice session using: {_dataService.GetType()}", this);
            return await _dataService.SetItemAsync<object>(Defines.API_PRACTICES_UPDATE_ENDPOINT, practice);
        }


        public Task CacheAllPractices(List<int> sessionIds)
        {
            return null;
        }

        public void CachePractice(int sessionId, AthletePractice practice)
        {
            var cache = _cacheService.GetItem<Dictionary<int, AthletePractice>>(Defines.CACHE_KEY_ATHLETEPRACTICES);

            if (cache == null)
                cache = new Dictionary<int, AthletePractice>();

            if (!cache.ContainsKey(sessionId))
                cache.Add(sessionId, practice);
            else
                cache[sessionId] = practice;

            _cacheService.SetItem<Dictionary<int, AthletePractice>>(Defines.CACHE_KEY_ATHLETEPRACTICES, cache);
        }

        public void CachePracticeById(int id, AthletePractice practice)
        {
            var cache = _cacheService.GetItem<Dictionary<int, AthletePractice>>(Defines.CACHE_KEY_ATHLETEPRACTICES_BYID);

            if (cache == null)
                cache = new Dictionary<int, AthletePractice>();

            if (!cache.ContainsKey(id))
                cache.Add(id, practice);
            else
                cache[id] = practice;

            _cacheService.SetItem<Dictionary<int, AthletePractice>>(Defines.CACHE_KEY_ATHLETEPRACTICES, cache);
        }
    }
}
