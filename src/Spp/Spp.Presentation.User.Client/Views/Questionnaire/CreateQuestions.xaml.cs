/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Models;
using MicrosoftSportsScience.ViewModels;
using MicrosoftSportsScience.UserControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using MicrosoftSportsScience.Annotations;
using MicrosoftSportsScience.Helpers;
using System.Linq;
using Windows.UI.Popups;

namespace MicrosoftSportsScience
{
    public sealed partial class CreateQuestions : Page
    {
        List<FrameworkElement> questionControls = new List<FrameworkElement>();
        SplitView rootPage = Shell.Current;
        private AthletePracticeModel _practiceModel;
        private AthleteQuestionModel _questionModel;

        internal AthletePracticeViewModel PracticeViewModel;
        internal AthleteQuestionnaire PracticeQuestions;

        public RecommendedLoadViewModel LoadViewModel { get; set; }

        public CreateQuestions()
        {
            this.InitializeComponent();
            _practiceModel = SimpleIoc.Default.GetInstance<AthletePracticeModel>();
            _questionModel = SimpleIoc.Default.GetInstance<AthleteQuestionModel>();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            PracticeViewModel = (AthletePracticeViewModel)e.Parameter;

            LoadViewModel = new RecommendedLoadViewModel();
            await LoadViewModel.Load();
        }


        private void QuestionUserControl_ResponseSelected(object sender, RoutedEventArgs e)
        {
           
        }

        private void AddTapped(object sender, TappedRoutedEventArgs e)
        {
            int index = 0;
            for (int i = 0; i < questionControls.Count; i++)
            {
                if (questionControls[i].Visibility == Visibility.Collapsed)
                {
                    index = i;
                    break;
                }
            }

            if (index != 0)
            {
                List<FrameworkElement> children = new List<FrameworkElement>();
                questionControls[index].Visibility = Visibility.Visible;
                children = HelperMethods.GetChildren(questionControls[index]);
                foreach(FrameworkElement child in children)
                {
                    if(child is QuestionUserControl)
                    {
                        var tempQuestion = child as QuestionUserControl;
                        tempQuestion.Title = "Title";
                        tempQuestion.LowRangeText = "Low Range";
                        tempQuestion.MidRangeText = "Mid Range";
                        tempQuestion.HighRangeText = "High Range";
                    }
                }
            }
        }

        private void RemoveTapped(object sender, TappedRoutedEventArgs e)
        {
            int index = 0;
            FrameworkElement source = e.OriginalSource as FrameworkElement;
            source = source.Parent as FrameworkElement;
         
            if(source.Name == "button2")
            {
                Question2.Title = Question3.Title;
                Question2.LowRangeText = Question3.LowRangeText;
                Question2.MidRangeText = Question3.MidRangeText;
                Question2.HighRangeText = Question3.HighRangeText;

                Question3.Title = Question4.Title;
                Question3.LowRangeText = Question4.LowRangeText;
                Question3.MidRangeText = Question4.MidRangeText;
                Question3.HighRangeText = Question4.HighRangeText;
            }
            if (source.Name == "button3")
            {
                Question3.Title = Question4.Title;
                Question3.LowRangeText = Question4.LowRangeText;
                Question3.MidRangeText = Question4.MidRangeText;
                Question3.HighRangeText = Question4.HighRangeText;
            }

            for (int i = 0; i < questionControls.Count; i++)
            {
                if (questionControls[i].Visibility == Visibility.Visible)
                {
                    index = i;
                }
            }

            if (index != 0)
            {
                questionControls[index].Visibility = Visibility.Collapsed;
            }
        }

        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            questionControls.Add(control1);
            questionControls.Add(control2);
            questionControls.Add(control3);
            questionControls.Add(control4);
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var questionOrder = 1;
        
            var sessionModel = SimpleIoc.Default.GetInstance<AthleteSessionModel>();

            var session = new Session
            {
                Location = new Location
                {
                    Id = 1,
                    Address = "Some place",
                    Name = "Training Facility A",
                    Type = LocationType.Stadium,
                },
                Scheduled = PracticeViewModel.StartDate,
                SessionType = "1",
            };


            //save the session on the DB
            var sessionId = await sessionModel.SaveSession(session);

            //Now that we have the ID, we need to update the session with the users
            session.Id = sessionId;
            session.Users = PracticeViewModel.AssignedUsers.ToList();
            await sessionModel.SaveSessionUsers(session);

          
            //save questions
            PracticeQuestions = new AthleteQuestionnaire()
            {
                IsEnabled = true,
                Name = PracticeViewModel.QuestionnaireName,
                SessionId = session.Id,
                SequenceOrder = 1, //TODO: Default: 1. Needs to be replaced with the order of questionnaires if required
                StartDateTime = PracticeViewModel.StartDate,
                EndDateTime = PracticeViewModel.EndDate,
                Questions = new List<AthleteQuestion>(),
            };

            //Save all questions
            foreach (var t in questionControls)
            {
                var children = HelperMethods.GetChildren(t);
                foreach (var child in children)
                {
                    var control = child as QuestionUserControl;
                    if (control != null && control.Title != "Title")
                    {
                        var questionControl = control;
                        var question = new AthleteQuestion()
                        {
                            Text = questionControl.Title,
                            MinCaptionValue = new KeyValuePair<string, int>(questionControl.LowRangeText, 0),
                            MidCaptionValue = new KeyValuePair<string, int>(questionControl.MidRangeText, 5),
                            MaxCaptionValue = new KeyValuePair<string, int>(questionControl.HighRangeText, 10),
                            SequenceOrder = questionOrder++,
                        };
                        PracticeQuestions.Questions.Add(question);
                    }
                }
            }

            //Save new practice's questionnaire
            await _questionModel.SaveQuestionnaire(PracticeQuestions);

            //Finally, we need to update the practice object to update the sessionId
            var result = await PracticeViewModel.UpdatePracticeWithSessionId(sessionId);

            if (result)
            {
                MessageDialog dlg = new MessageDialog("Session Scheduled!");
                await dlg.ShowAsync();
            }
            else
            {
                MessageDialog dlg = new MessageDialog("Something went wrong. Please try again later.");
                await dlg.ShowAsync();
            }


            //Navigate to the scheduling
            (rootPage.Content as Frame).Navigate(typeof(Calendar));
        }

        private void BackButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).GoBack();
        }
    }
}
