/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftSportsScience.ViewModels
{
    public class UserViewModel : NotificationBase<User>
    {

        private AthleteAnswerModel _answerModel;

        public UserViewModel(User user) : base(user)
        {
            _answerModel = SimpleIoc.Default.GetInstance<AthleteAnswerModel>();
        }

        public String FirstName
        {
            get { return This.FirstName; }
        }

        public String LastName
        {
            get { return This.LastName; }
        }

        public User User
        {
            get { return This; }
        }

        public string PhotoUrl
        {
            get { return This.PathToPhoto; }
        }

        public bool IsCoach
        {
            get { return ((RoleTypes)This.RoleId == RoleTypes.Coach); }
        }

        public bool IsPlayer
        {
            get { return ((RoleTypes)This.RoleId == RoleTypes.Player); }
        }

        private bool _completedQuestionnaire;
        public bool CompletedQuestionnaire
        {
            get { return _completedQuestionnaire; }
            set { SetProperty(ref _completedQuestionnaire, value); }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }


        public override Task Load()
        {
            return null;
        }

        public async Task LoadCompleteness()
        {
            //filling completeness data
            if (IsPlayer)
            {
                IsLoading = true;
                CompletedQuestionnaire = await _answerModel.GetPlayerResponded(This.Id);
                IsLoading = false;
             }

            return;
        }
    }
}
