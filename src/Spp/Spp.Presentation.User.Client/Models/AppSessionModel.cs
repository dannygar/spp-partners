/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spp.Presentation.User.Client.Helpers;
using Spp.Presentation.User.Client.Services;
using Spp.Presentation.User.Shared.CognitiveServiceHelpers;
using Windows.UI.Xaml.Controls;

namespace Spp.Presentation.User.Client.Models
{
    using Data;

    class AppSessionModel : BaseModel
    {
        public event EventHandler<int> Authenticated;

        public bool IsAuthenticated { get; set; }

        public User CurrentUser { get; set; }
        public Session CurrentSession { get; set; }
        public SplitView ContentView { get; set; }
        public Shell AppShell { get; set; }

        public int TeamId { get; set; }

        private IHttpClientService _dataService;
        private ILogService _logService;
        private ICacheService _cacheService;

        public AppSessionModel(IHttpClientService dataService, ILogService logService, ICacheService cacheService)
        {
            _dataService = dataService;
            _logService = logService;
            _cacheService = cacheService;
            CurrentUser = new User();
        }


        public async Task<Session> GetSession(string date)
        {
            // Check cache
            //if (_cacheService.IsCached(Defines.CACHE_KEY_SESSION))
            //{
            //    var cache = _cacheService.GetItem<Dictionary<int, Session>>(Defines.CACHE_KEY_SESSION);
            //    if (cache != null)
            //        return cache[TeamId];
            //}

            _logService.Info($"Getting the team using: {_dataService.GetType()} and the session for the date: {date}", this);
            var sessions = await _dataService.GetItemAsync<List<Session>>(string.Format(Defines.API_SESSION_ENDPOINT, FormatDate(date)));
            var session = (sessions != null && sessions.Any()) ? sessions.FirstOrDefault() : null;
            this.CacheSession(session);

            return session;
        }


        public async Task<Session> GetSession(DateTime begin, DateTime end)
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_SESSION))
            {
                var cache = _cacheService.GetItem<Dictionary<int, Session>>(Defines.CACHE_KEY_SESSION);
                if (cache != null)
                    return cache[TeamId];
            }

            _logService.Info(String.Format("Getting the team using: {0}", _dataService.GetType().ToString()), this);

            var sessions = await _dataService.GetItemAsync<List<Session>>(Defines.API_SESSION_ENDPOINT, $"?from={begin}&to={end}");
            var session = (sessions != null && sessions.Any()) ? sessions.FirstOrDefault() : null;
            this.CacheSession(session);

            return session;
        }


        public async Task<int> AuthenticateADUserAsync()
        {
            this._logService.Info($"Authenticating the user", this);
            this.TeamId = await this._dataService.GetItemAsync<int>(Defines.API_AUTHENTICATION_ENDPOINT);

            if (this.TeamId > 0)
            {
                this.IsAuthenticated = true;
                this.Authenticated?.Invoke(this, this.TeamId);
            }

            return this.TeamId;
        }



        public async Task<int> AuthenticateFaciallyRecognizedPerson(IList<UserIdentity> recognizedPeople)
        {
            this._logService.Info($"Authenticating the user using face recognition", this);
            this.TeamId = await this._dataService.PostAsync<IList<UserIdentity>, int>(Defines.API_AUTH_RECOGNIZED_ENDPOINT, recognizedPeople);

            if (this.TeamId > 0)
            {
                this.IsAuthenticated = true;
                this.CurrentUser.FullName = recognizedPeople.First().FullName;
                this.Authenticated?.Invoke(this, this.TeamId);
            }

            return this.TeamId;
        }



        public void CacheSession(Session team)
        {
            var cache = _cacheService.GetItem<Dictionary<int, Session>>(Defines.CACHE_KEY_SESSION);

            if (cache == null)
                cache = new Dictionary<int, Session>();

            if (!cache.ContainsKey(TeamId))
                cache.Add(TeamId, team);

            _cacheService.SetItem<Dictionary<int, Session>>(Defines.CACHE_KEY_SESSION, cache);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public async Task<bool> GetCognitiveServiceKeys(int teamId)
        {
            if (CSSettingsHelper.Instance.IsValidSettings()) return true;

            var endpoint = $"{Defines.API_COGNITIVESERVICES_ENDPOINT}/{teamId}";

            this._logService.Info($"Retrieving the user's cognitive services API keys using team Id: {teamId}", this);
            var csKeys = await this._dataService.GetItemAsync<CognitiveServiceKeys>(endpoint);

            if (!string.IsNullOrEmpty(csKeys?.WorkspaceKey))
            {
                CSSettingsHelper.Instance.WorkspaceKey = csKeys?.WorkspaceKey;
                CSSettingsHelper.Instance.FaceApiKey = csKeys?.FaceApiKey;
                CSSettingsHelper.Instance.Location = csKeys?.Location;
                CSSettingsHelper.Instance.EmotionApiKey = csKeys?.EmotionApiKey;
                CSSettingsHelper.Instance.CameraName = csKeys?.CameraName;
                CSSettingsHelper.Instance.MinDetectableFaceCoveragePercentage = (uint)
                    csKeys?.MinDetectableFaceCoveragePercentage;

                EmotionServiceHelper.ApiKey = csKeys.EmotionApiKey;
                FaceServiceHelper.ApiKey = csKeys.FaceApiKey;
                FaceServiceHelper.ApiRoot = csKeys.Location;
                ImageAnalyzer.PeopleGroupsUserDataFilter = csKeys.WorkspaceKey;
                FaceListManager.FaceListsUserDataFilter = csKeys.WorkspaceKey;
                CoreUtil.MinDetectableFaceCoveragePercentage = csKeys.MinDetectableFaceCoveragePercentage;
            }

            return CSSettingsHelper.Instance.IsValidSettings();
        }



        private string FormatDate(string datestring)
        {
            DateTime.TryParse(datestring, out DateTime date);
            return FormatDate(date);
        }

        private string FormatDate(DateTime date)
        {
            return date.ToString("d").Replace("/", "-");
        }
    }
}
