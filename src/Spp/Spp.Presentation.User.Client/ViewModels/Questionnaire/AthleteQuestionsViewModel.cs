/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.ComponentModel;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Services;
using MicrosoftSportsScience.Models;
using MicrosoftSportsScience.Data;
using System.Collections.Generic;
using System.Linq;

namespace MicrosoftSportsScience.ViewModels
{
    public class AthleteQuestionsViewModel : NotificationBase
    {
        private AthleteQuestionModel _questionsModel;
        private AppSessionModel _session;
        private List<AthleteQuestion> _modelQuestions;
        private int _selectedIndex;

        public AthleteQuestionsViewModel()
        {
            _questionsModel = SimpleIoc.Default.GetInstance<AthleteQuestionModel>();
            _session = SimpleIoc.Default.GetInstance<AppSessionModel>();
        }

        private ObservableCollection<AthleteQuestionViewModel> _questions = new ObservableCollection<AthleteQuestionViewModel>();
        public ObservableCollection<AthleteQuestionViewModel> Questions
        {
            get { return _questions; }
            set { SetProperty(ref _questions, value); }
        }
        
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (SetProperty(ref _selectedIndex, value))
                { RaisePropertyChanged(nameof(SelectedPerson)); }
            }
        }

        public AthleteQuestionViewModel SelectedPerson
        {
            get { return (_selectedIndex >= 0) ? _questions[_selectedIndex] : null; }
        }

        public override async Task Load()
        {
            _selectedIndex = -1;

            _logService.Info("Attempting to load questions for athlete", this);
            _modelQuestions = (List<AthleteQuestion>) await _questionsModel.GetQuestions(_session.CurrentSession.Id);

            if (_modelQuestions == null)
                return;
            foreach (var question in _modelQuestions)
            {
                var np = new AthleteQuestionViewModel(question);
                await np.Load();

                np.PropertyChanged += Question_OnNotifyPropertyChanged;
                _questions.Add(np);
            }
        }

        public async void Add()
        {
            var question = new AthleteQuestion() { Id = _modelQuestions.Count };
            var questionModel = new AthleteQuestionViewModel(question);
            await questionModel.Load();

            questionModel.PropertyChanged += Question_OnNotifyPropertyChanged;
            Questions.Add(questionModel);
            SelectedIndex = Questions.IndexOf(questionModel);

            _modelQuestions.Add(question);
        }

        public async void Delete()
        {
            if (SelectedIndex != -1)
            {
                var question = Questions[SelectedIndex];

                Questions.RemoveAt(SelectedIndex);
                _modelQuestions.Remove(_modelQuestions.FirstOrDefault(x => x.Id.Equals(question.Id)));

                _logService.Info("Attempting to remove question from questionnaire, question id: " + question.QuestionId, this);
                await _questionsModel.SetQuestions(_session.CurrentSession.Id, _modelQuestions);
            }
        }

        private async void Question_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            _logService.Info("Attempting to add new question to questionnaire", this);
            await _questionsModel.SetQuestions(_session.CurrentSession.Id, _modelQuestions);
        }
    }
}