/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Services;
using MicrosoftSportsScience.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftSportsScience.ViewModels
{
    public class AthleteQuestionHistoryViewModel : NotificationBase
    {
        private AthleteQuestionHistoryModel _historyModel;
        private AppSessionModel _sessionModel;

        public AthleteQuestionHistoryViewModel()
        {
            _historyModel = SimpleIoc.Default.GetInstance<AthleteQuestionHistoryModel>();
            _sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
        }

        private ObservableCollection<AthleteQuestionHistoryItemViewModel> _history = new ObservableCollection<AthleteQuestionHistoryItemViewModel>();
        public ObservableCollection<AthleteQuestionHistoryItemViewModel> HistoryList
        {
            get { return _history; }
            set { SetProperty(ref _history, value); }
        }

        private string _wellness = "";

        public string  Wellness
        {
            get { return _wellness; }
            set { SetProperty(ref _wellness, value); }
        }

        public override async Task Load()
        {
            _logService.Info("Attempting to load history data for athlete", this);
            var historyItems = await _historyModel.GetHistory(_sessionModel.CurrentUser, 
                _sessionModel.CurrentSession.Id);

            if (historyItems == null)
                return;

            
            var lastResponse = historyItems.GroupBy(x => x.Responses[0].AnswerDateTime).OrderByDescending(x => x.Key).First();
            var score = lastResponse.Sum(response => response.Responses.Sum(entry => entry.Answer.Value));
            Wellness = (score * 2.5).ToString(CultureInfo.InvariantCulture);

            //this is causing the charts to display with just one day and repeated metrics, because the api is weird
            //foreach (var item in historyItems)
            //    _history.Add(new AthleteQuestionHistoryItemViewModel(item));

            //hack to solve repetition problem but this should be fixed at the api level
            var groups = historyItems.GroupBy(x => x.Question.Id);
            foreach (var group in groups)
            {
                List<AthleteQuestionHistoryEntry> entries = new List<AthleteQuestionHistoryEntry>();
                foreach (var question in group)
                {
                    var recordedAnswer = question.Responses.First();
                    entries.Add(recordedAnswer);
                }

                var h = new AthleteQuestionHistory
                {
                    Question = group.First().Question,
                    Responses = entries,
                };

                var vm = new AthleteQuestionHistoryItemViewModel(h);
                _history.Add(vm);
            }
        }      
    }
}
