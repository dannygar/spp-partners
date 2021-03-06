/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Models;

namespace Spp.Presentation.User.Client.ViewModels.Dashboard
{
    public class TrainingDashboardViewModel : NotificationBase
    {
        private TeamModel _teamModel;
        private AppSessionModel _sessionModel;

        private List<PlayerFitnessViewModel> _playerList;

        public TrainingDashboardViewModel()
        {
            _teamModel = SimpleIoc.Default.GetInstance<TeamModel>();
            _sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
        }

        public List<PlayerFitnessViewModel> PlayerList
        {
            get
            {
                return _playerList;
            }
        }

        public override async Task Load()
        {
            _playerList = new List<PlayerFitnessViewModel>();

            var session = _sessionModel.CurrentSession;

            string[] colorArray = new string[3]
            {
                "#49E20E",  // available
                "#FF0000",  // injured
                "#FFFF00"   // resting
            };

            if (session != null && session.Users != null)
            {
                Random r = new Random();
                foreach (var u in session.Users.Where(p => (RoleTypes)p.RoleId == Data.RoleTypes.Player))
                {
                    _playerList.Add(new PlayerFitnessViewModel
                    {
                        Color = u.PlayerInfo?.IsInjured == true ? colorArray[1] : u.PlayerInfo?.IsResting == true ? colorArray[2] : colorArray[0],
                        PlayerImage = u.PathToPhoto,
                        PlayerName = u.FullName,
                        PlayerTrendNumber = string.Format("{0}%", r.Next(0, 8)),
                        PlayerAge = (u.DateofBirth != null) ? (DateTime.UtcNow.Year - ((DateTime)u.DateofBirth).Year).ToString() : "24",
                        PlayerHeight = ConvertCMtoFtInch(u.Height),
                        PlayerJerseyNum = u.PlayerInfo?.JerseyNumber.ToString(),
                        PlayerPosition = u.PlayerInfo?.Position.Name
                    });
                }
            }
            RaisePropertyChanged("PlayerList");
        }


        private string ConvertCMtoFtInch(decimal? length)
        {
            if (length == null) return "N/A";
            var metric_length = (double)length;

            var feet = (int)Math.Round(metric_length / 2.54 * 8, 0) / 96;
            var inch = (int)Math.Round((metric_length / 2.54 - feet * 12) * 8, 0) / 8;

            return $"{feet}' {inch}\"";
        }
    }
}
