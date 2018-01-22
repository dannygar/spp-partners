/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Services;

namespace Spp.Presentation.User.Client.Models
{
    class AthleteSessionModel : BaseModel
    {
        private IHttpClientService _dataService;
        private ILogService _logService;
        private ICacheService _cacheService;

        public AthleteSessionModel(IHttpClientService dataService, ILogService logService, ICacheService cacheService)
        {
            _dataService = dataService;
            _logService = logService;
            _cacheService = cacheService;
        }

        public async Task<Session> GetSession()
        {
            int hardcoded = 4;

            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_ATHLETESESSIONS))
            {
                var cache = _cacheService.GetItem<Dictionary<int, Session>>(Defines.CACHE_KEY_ATHLETESESSIONS);
                if (cache != null && cache.ContainsKey(hardcoded))
                    return cache[hardcoded];
            }

            var sessions = await this.GetSessions();
            if (sessions != null)
            {
                var session = sessions.FirstOrDefault(x => x.Id.Equals(hardcoded));

                if (session == null)
                    session = sessions[0];

                this.CacheSession(session);

                return session;
            }
            return new Session();
        }

        public async Task<List<Session>> GetSessions(DateTime begin, DateTime end)
        {
            var sessions = await this.GetSessions();

            return sessions?.Where(x => x.Scheduled >= begin && x.Scheduled <= end).ToList();
        }

        public async Task<List<Session>> GetSessions()
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_ATHLETESESSIONS))
            {
                var cache = _cacheService.GetItem<Dictionary<int, Session>>(Defines.CACHE_KEY_ATHLETESESSIONS);
                if (cache != null)
                    return cache.Values.ToList();
            }

            _logService.Info(String.Format("Getting sessions using: {0}", _dataService.GetType().ToString()), this);
            var sessions = await _dataService.GetItemAsync<List<Session>>(Defines.API_SESSION_ENDPOINT);

            if (sessions == null || sessions.Count <= 0)
                return default(List<Session>);

            return sessions;
        }

        public async Task<int> SaveSession(Session session)
        {
            _logService.Info($"Saving a new session using: {_dataService.GetType()}", this);
            return await _dataService.SetItemAndReturnIdAsync<Session>(Defines.API_SESSION_ENDPOINT, session);
        }

        public async Task<bool> SaveSessionUsers(Session session)
        {
            _logService.Info($"Saving a new session using: {_dataService.GetType()}", this);
            return await _dataService.SetItemAsync<Session>(string.Format(Defines.API_SESSION_USERS_SAVE_ENDPOINT, session.Id), session);
        }

        public async Task CacheAllSessions()
        {
            var sessionCache = new Dictionary<int, Session>();
            var sessions = await this.GetSessions();

            foreach (var session in sessions)
                sessionCache.Add(session.Id, session);

            _cacheService.SetItem<Dictionary<int, Session>>(Defines.CACHE_KEY_ATHLETESESSIONS, sessionCache);
        }

        public void CacheSession(Session session)
        {
            var cache = _cacheService.GetItem<Dictionary<int, Session>>(Defines.CACHE_KEY_ATHLETESESSIONS);

            if (cache == null)
                cache = new Dictionary<int, Session>();

            if (!cache.ContainsKey(session.Id))
                cache.Add(session.Id, session);

            _cacheService.SetItem<Dictionary<int, Session>>(Defines.CACHE_KEY_ATHLETESESSIONS, cache);
        }
    }
}
