/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Data;
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
    public class MessagesViewModel : NotificationBase<AthleteQuestion>
    {
        private AthleteMessageModel _messageModel;
        private AppSessionModel _sessionModel;

        public MessagesViewModel()
        {
            _messageModel = SimpleIoc.Default.GetInstance<AthleteMessageModel>();
            _sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
        }

        private ObservableCollection<MessageViewModel> _messages = new ObservableCollection<MessageViewModel>();
        public ObservableCollection<MessageViewModel> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages, value); }
        }

        public async override Task Load()
        {
            _logService.Info("Attempting to load messages for athlete: " + _sessionModel.CurrentUser.Id, this);
            var messages = await _messageModel.GetTodaysAthleteMessages(_sessionModel.CurrentUser, _sessionModel.CurrentSession.Id);
            if(messages != null)
                foreach (var message in messages)
                    _messages.Add(new MessageViewModel(message));
        }
    }
}