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
    class AthleteWorkoutModel : BaseModel
    {
        private IHttpClientService _dataService;
        private ILogService _logService;
        private ICacheService _cacheService;

        public AthleteWorkoutModel(IHttpClientService dataService, ILogService logService, ICacheService cacheService)
        {
            _dataService = dataService;
            _logService = logService;
            _cacheService = cacheService;
        }



        public async Task<IList<AthleteWorkout>> GetSessionWorkouts(int sessionId)
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_SESSIONWORKOUTS))
            {
                var cache = _cacheService.GetItem<IList<AthleteWorkout>>(Defines.CACHE_KEY_SESSIONWORKOUTS);
                if (cache != null)
                    return cache;
            }

            _logService.Info($"Getting workouts for the current session: { sessionId }, using: { _dataService.GetType().ToString() }", this);
            return await _dataService.GetItemAsync<IList<AthleteWorkout>>(string.Format(Defines.API_WORKOUTS_ENDPOINT, sessionId));
        }


        public async Task<List<AthleteWorkout>> GetAthleteWorkouts(User athlete, List<Session> sessions)
        {
            var workouts = new List<AthleteWorkout>();

            foreach (var session in sessions)
                workouts.Add(await GetAthleteWorkout(athlete, session));

            return workouts;
        }



        public async Task<IList<AthleteExercise>> GetAllExercises()
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_EXERCISES))
            {
                var cache = _cacheService.GetItem<IList<AthleteExercise>>(Defines.CACHE_KEY_EXERCISES);
                if (cache != null)
                    return cache;
            }

            _logService.Info($"Getting all exercises using: { _dataService.GetType() }", this);
            return await _dataService.GetItemAsync<IList<AthleteExercise>>(Defines.API_WORKOUT_GET_EXERCISES_ENDPOINT);
        }


        public async Task<AthleteWorkout> GetAthleteWorkout(User athlete, Session session)
        {
            if (athlete == null)
                return null;

            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_ATHLETEWORKOUTS))
            {
                var cache = _cacheService.GetItem<Dictionary<int, Dictionary<int, AthleteWorkout>>>(Defines.CACHE_KEY_ATHLETEWORKOUTS);
                if (cache.ContainsKey(session.Id) && cache[session.Id].ContainsKey(athlete.Id))
                    return cache[session.Id][athlete.Id];
            }

            _logService.Info($"Getting workouts for athlete: { athlete.Id }, using: { _dataService.GetType().ToString() }", this);
            var workouts = await _dataService.GetItemAsync<List<AthleteWorkout>>(string.Format(Defines.API_WORKOUTS_ENDPOINT, session.Id));

            return workouts?.First();
        }

        public async Task CompleteExercise(int exerciseId, bool modified)
        {
            _logService.Info($"Updating workout completion for exercise: { exerciseId }, using: { _dataService.GetType().ToString() }", this);
            await _dataService.PutItemAsync<object>(string.Format(Defines.API_WORKOUTS_COMPLETE_ENDPOINT, exerciseId, "true", modified), null);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="workout"></param>
        /// <returns></returns>
        public async Task CreateAthleteWorkout(AthleteWorkout workout)
        {
            _logService.Info($"Creating a new workout using: {_dataService.GetType()}", this);
            await _dataService.SetItemAsync<object>(Defines.API_WORKOUTS_ENDPOINT, workout);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="workout"></param>
        /// <returns></returns>
        public async Task UpdateAthleteWorkout(AthleteWorkout workout)
        {
            _logService.Info($"Updating a new workout using: {_dataService.GetType()}", this);
            await _dataService.SetItemAsync<object>(Defines.API_WORKOUT_SAVE_ENDPOINT, workout);
        }


        public void InvalidateCache(string cacheKey)
        {
            _cacheService.ClearCache(cacheKey);
        }


        public async Task CacheAllWorkouts(List<User> athletes, List<Session> sessions)
        {
            var workoutCache = new Dictionary<int, Dictionary<int, AthleteWorkout>>();

            foreach (var session in sessions)
            {
                var sessionCache = new Dictionary<int, AthleteWorkout>();
                workoutCache.Add(session.Id, sessionCache);

                foreach (var athlete in athletes)
                    sessionCache.Add(athlete.Id, await this.GetAthleteWorkout(athlete, session));
            }

            _cacheService.SetItem<Dictionary<int, Dictionary<int, AthleteWorkout>>>(Defines.CACHE_KEY_ATHLETEWORKOUTS, workoutCache);
        }

        public void CacheWorkout(User athlete, int sessionId, AthleteWorkout workout)
        {
            var cache = _cacheService.GetItem<Dictionary<int, Dictionary<int, AthleteWorkout>>>(Defines.CACHE_KEY_ATHLETEMESSAGES);

            if (cache == null)
                cache = new Dictionary<int, Dictionary<int, AthleteWorkout>>();

            if (!cache.ContainsKey(sessionId))
                cache.Add(sessionId, new Dictionary<int, AthleteWorkout>());

            if (!cache[sessionId].ContainsKey(athlete.Id))
                cache[sessionId].Add(athlete.Id, workout);
            else
                cache[sessionId][athlete.Id] = workout;

            _cacheService.SetItem<Dictionary<int, Dictionary<int, AthleteWorkout>>>(Defines.CACHE_KEY_ATHLETEMESSAGES, cache);
        }
    }
}
