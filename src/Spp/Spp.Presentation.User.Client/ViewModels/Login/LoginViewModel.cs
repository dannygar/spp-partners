/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Models;
using Windows.UI.Popups;

namespace Spp.Presentation.User.Client.ViewModels
{
    using Data;

    public class LoginViewModel : NotificationBase
    {
        private AppSessionModel _sessionModel;
        private List<User> _users;
        private List<UserViewModel> _userModels = new List<UserViewModel>();
        private ConfigurationSettingsViewModel _appConfigurationSettings = new ConfigurationSettingsViewModel();

        public LoginViewModel()
        {
            _sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
        }

        public List<UserViewModel> Users
        {
            get { return _userModels; }
        }


        public override async Task Load()
        {
            _users = new List<User>();

            await _appConfigurationSettings.Load();
            _logService.Info("Attempting to load players, coaches and the current session", this);

            if (_sessionModel.CurrentSession != null && _sessionModel.IsAuthenticated && _sessionModel.CurrentSession.Users != null)
            {
                _users.AddRange(_sessionModel.CurrentSession.Users);
            }
            else
            {
                var session = await _sessionModel.GetSession(_appConfigurationSettings.AppConfigurationSettings.SessionDate.ToString("d"));
                if (session != null)
                {
                    _sessionModel.CurrentSession = session;
                    _users.AddRange(_sessionModel.CurrentSession.Users);
                }
                else
                {
                    await new MessageDialog(
                        $"There are no teams assigned to this session date: {_appConfigurationSettings.AppConfigurationSettings.SessionDate.ToString("d")}",
                        "No match").ShowAsync();
                }
            }


            foreach (var user in _users)
            {
                var np = new UserViewModel(user);
                _userModels.Add(np);
            }
        }
    }
}
