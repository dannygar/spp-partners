/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spp.Presentation.User.Client.Services;

namespace Spp.Presentation.User.Client.Models
{
    using Data;

    class CacheModel : BaseModel
    {
        private ICacheService _cacheService;
        private AthleteMessageModel _messageModel;
        private AthleteQuestionHistoryModel _questionhistoryModel;
        private AthleteQuestionModel _questionModel;
        private AthleteSessionModel _sessionmodel;

        public CacheModel(ICacheService cacheService, AthleteMessageModel messageModel, AthleteQuestionHistoryModel questionHistoryModel, AthleteQuestionModel questionModel, AthleteSessionModel sessionModel)
        {
            _cacheService = cacheService;
            _messageModel = messageModel;
            _questionhistoryModel = questionHistoryModel;
            _questionModel = questionModel;
            _sessionmodel = sessionModel;
        }

        public async Task CacheData()
        {
            // Cache sessions and athletes
            await _sessionmodel.CacheAllSessions();
            //await _teamModel.CacheAllAthletes();

            var sessions = await _sessionmodel.GetSessions();
            var athletes = new List<User>();
            foreach (var session in sessions)
            {
                athletes.AddRange(session.Users);
            }

            // Cache messages
            await _messageModel.CacheAllMessages(athletes, sessions.Select(x => x.Id).ToList());

            // Cache questions
            await _questionModel.CacheAllQuestions(sessions.Select(x => x.Id).ToList());

            // Cache histories
            await _questionhistoryModel.CacheAllHistories(athletes, sessions.Select(x => x.Id).ToList());
        }
    }
}
