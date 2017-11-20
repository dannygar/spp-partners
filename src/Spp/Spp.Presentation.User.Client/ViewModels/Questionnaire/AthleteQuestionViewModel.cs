/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Services;
using MicrosoftSportsScience.Models;
using MicrosoftSportsScience.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MicrosoftSportsScience.ViewModels
{
    public class AthleteQuestionViewModel : NotificationBase<AthleteQuestion>
    {
        private AthleteAnswerModel _answerModel;
        private List<AthleteQuestionHistoryEntry> _modelAnswers;
        private AthleteQuestionHistory _modelHistory;

        public AthleteQuestionViewModel(AthleteQuestion question) : base(question)
        {
            _answerModel = SimpleIoc.Default.GetInstance<AthleteAnswerModel>();
            _modelHistory = SimpleIoc.Default.GetInstance<AthleteQuestionHistory>();
            _answers.Add(new AthleteAnswerViewModel() { AnswerText = question.MinCaptionValue });
            _answers.Add(new AthleteAnswerViewModel() { AnswerText = question.MidCaptionValue });
            _answers.Add(new AthleteAnswerViewModel() { AnswerText = question.MaxCaptionValue });
        }

        public int Id
        {
            get { return This.Id; }
            set { SetProperty(This.Id, value, () => This.Id = value); }
        }

        public String QuestionText
        {
            get { return This.Text; }
            set { SetProperty(This.Text, value, () => This.Text = value); }
        }

        public int QuestionId
        {
            get { return This.Id; }
            set { SetProperty(This.Id, value, () => This.Id = value); }
        }

        private ObservableCollection<AthleteAnswerViewModel> _answers = new ObservableCollection<AthleteAnswerViewModel>();
        public ObservableCollection<AthleteAnswerViewModel> Answers
        {
            get { return _answers; }
            set { SetProperty(ref _answers, value); }
        }

        public override async Task Load()
        {
            await Task.Run(() =>
            {
                if (This != null)
                {
                    _logService.Info("Attempting to load answers for question: " + This.Id, this);
                    _modelAnswers = (List<AthleteQuestionHistoryEntry>)_modelHistory.Responses;

                    if (_modelAnswers.Count >= 3) //minimum is 3
                    {
                        foreach (var answer in _modelAnswers)
                        {
                            _answers.Add(new AthleteAnswerViewModel(answer));
                        }
                    }
                    else
                    {
                        for (var i = 0; i < 3; i++)
                        {
                            _answers.Add(new AthleteAnswerViewModel());
                        }
                    }
                }
            });
        }
    }
}