/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Models;
using Spp.Presentation.User.Client.UserControls;
using Spp.Presentation.User.Client.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Spp.Presentation.User.Client
{
    public sealed partial class AnswerPostQuestions : Page
    {
        public AthleteQuestionsViewModel PreWorkoutQuestions { get; set; }
        private Dictionary<int, int> _responses;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Hardcode session for now
            //var session = SimpleIoc.Default.GetInstance<AppSessionModel>();
            //session.CurrentSession.Id = 4;

            _responses = new Dictionary<int, int>();
            PreWorkoutQuestions = new AthleteQuestionsViewModel();

            await PreWorkoutQuestions.Load();
        }

        public AnswerPostQuestions()
        {
            this.InitializeComponent();
            SubmitButton.IsEnabled = false;
        }

        private void QuestionUserControl_ResponseSelected(object sender, RoutedEventArgs e)
        {
            var questionControl = (QuestionUserControl)sender;

            if (!_responses.ContainsKey(questionControl.QuestionId))
                _responses.Add(questionControl.QuestionId, int.Parse(((RadioButton)e.OriginalSource).Content as String));
            else
                _responses[questionControl.QuestionId] = int.Parse(((RadioButton)e.OriginalSource).Content as String);

            if (_responses.Count == PreWorkoutQuestions.Questions.Count)
            {
                SubmitButton.IsEnabled = true;
            }
        }

        private async void SubmitButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
            var answersModel = SimpleIoc.Default.GetInstance<AthleteAnswerModel>();
            var questionModel = SimpleIoc.Default.GetInstance<AthleteQuestionModel>();
            await answersModel.SetAnswers(
                new PlayerResponse()
                {
                    SessionId = sessionModel.CurrentSession.Id,
                    PlayerId = sessionModel.CurrentUser.Id,
                    QuestionnaireId = questionModel.QuestionnaireId,
                    Answers = _responses.Keys.Select(x => new AthleteQuestionHistoryEntry()
                    {
                        QuestionId = x,
                        AnswerDateTime = DateTime.UtcNow,
                        Answer = new KeyValuePair<string, int>(x.ToString(), _responses[x]),
                    }).ToList(),
                });

            (sessionModel.ContentView.Content as Frame).Navigate(typeof(ThankYouPage));
        }
    }
}
