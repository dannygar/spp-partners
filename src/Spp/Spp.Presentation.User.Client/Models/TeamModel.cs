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
    public class TeamModel : BaseModel
    {
        private IHttpClientService _dataService;
        private ILogService _logService;
        private ICacheService _cacheService;

        public TeamModel(IHttpClientService dataService, ILogService logService, ICacheService cacheService)
        {
            _dataService = dataService;
            _logService = logService;
            _cacheService = cacheService;
        }

     
         public async Task<Team> GetTeam(int teamId)
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_TEAM))
            {
                var cache = _cacheService.GetItem<Dictionary<int, Team>>(Defines.CACHE_KEY_TEAM);
                if (cache != null)
                    return cache[teamId];
            }

            _logService.Info($"Getting the team using: {_dataService.GetType()} and the session for the date: {teamId}", this);
            var team = await _dataService.GetItemAsync<Team>(string.Format(Defines.API_TEAM_WITH_PLAYERS_ENDPOINT, teamId));
            this.CacheTeam(team);

            return team;
        }



        public async Task SetTeam(string teamId, Team t)
        {
            if (t == null)
                return;

            _logService.Info(String.Format("Setting team: {0}-{1}, using: {2}", t.TeamId, t.Name, _dataService.GetType().ToString()), this);
            await _dataService.SetItemAsync<Team>(teamId, t);
        }
        

        public void CacheTeam(Team t)
        {
            var cache = _cacheService.GetItem<Dictionary<int, Team>>(Defines.CACHE_KEY_TEAM);

            if (cache == null)
                cache = new Dictionary<int, Team>();

            if (cache.ContainsKey(t.id))
                cache.Remove(t.id);

            cache.Add(t.id, t);
            
            _cacheService.SetItem<Dictionary<int, Team>>(Defines.CACHE_KEY_TEAM, cache);
        }
    }
}
