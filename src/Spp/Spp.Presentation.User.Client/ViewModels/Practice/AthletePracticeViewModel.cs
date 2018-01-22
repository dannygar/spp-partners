/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Models;

namespace Spp.Presentation.User.Client.ViewModels
{
    using Data;

    public class AthletePracticeViewModel : NotificationBase<AthletePractice>
    {
        private AthletePracticeModel _practiceModel;
        private TeamModel _teamModel;
        private AppSessionModel _sessionModel;

        public AthletePracticeViewModel(AthletePractice practice) : base(practice)
        {
            _practiceModel = SimpleIoc.Default.GetInstance<AthletePracticeModel>();
            _teamModel = SimpleIoc.Default.GetInstance<TeamModel>();
            _sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
        }

        public bool IsNewPractice { get; set; }

        private List<User> _playerList = new List<User>();
        public List<User> PlayerList
        {
            get
            {
                return _playerList;
            }
        }

        private ObservableCollection<User> _assignedUsers = new ObservableCollection<User>();
        public ObservableCollection<User> AssignedUsers
        {
            get
            {
                return _assignedUsers;
            }
            set
            {
                SetProperty(ref _assignedUsers, value);
            }
        }

        public Type PreviousPage { get; set; }

        private string _name;
        public string Name { get { return This.Name; } set { _name = value; } }

        private string _topic;
        public string Topic { get { return This.Topic; } set { _topic = value; } }

        private string _subtopic;
        public string SubTopic { get { return This.SubTopic; } set { _subtopic = value; } }

        private int _estimatedTrainingLoad;
        public int EstimatedTrainingLoad
        {
            get { return _estimatedTrainingLoad; }
            set { SetProperty(ref _estimatedTrainingLoad, value); }
        }

        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        private DateTime _endDate;

        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }


        private string _questionnaireName;
        public string QuestionnaireName
        {
            get { return _questionnaireName; }
            set { SetProperty(ref _questionnaireName, value); }
        }

        private List<AthleteDrillViewModel> _drills = new List<AthleteDrillViewModel>();

        public List<AthleteDrillViewModel> Drills
        {
            get { return _drills; }
            set { SetProperty(ref _drills, value); }
        }


        private ObservableCollection<AthleteDrillViewModel> _assignedDrills = new ObservableCollection<AthleteDrillViewModel>();
        public ObservableCollection<AthleteDrillViewModel> AssignedDrills
        {
            get { return _assignedDrills; }
            set { SetProperty(ref _assignedDrills, value); }
        }


        public void AddDrill(AthleteDrillViewModel d)
        {
            Drills.Add(d);
            AssignedDrills.Add(d);
        }

        public void RemoveDrill(AthleteDrillViewModel d)
        {
            Drills.Remove(d);
            AssignedDrills.Remove(d);
        }

        public int CalculatedRecommendedTrainingLoad
        {
            get { return Drills.Sum(drill => drill.RecommendedTrainingLoad); }
        }

        public override async Task Load()
        {
            if (!IsNewPractice)
            {
                var details = await _practiceModel.GetAthletePracticeById(This.Id);
                if (details != null)
                {
                    //store practice drills
                    This.PracticeDrills = details.PracticeDrills;
                    Drills = This.PracticeDrills?.Select(x => new AthleteDrillViewModel(x)).ToList();

                    AssignedDrills.Clear();
                    foreach (var d in Drills)
                    {
                        AssignedDrills.Add(d);
                    }

                    EstimatedTrainingLoad = This.EstimatedTrainingLoad != null ? (int)This.EstimatedTrainingLoad : 0;
                }
            }
        }

        public async Task<bool> UpdatePracticeWithSessionId(int sessionId)
        {
            var _practiceModel = SimpleIoc.Default.GetInstance<AthletePracticeModel>();
            This.SessionId = sessionId;
            return await _practiceModel.UpdateAthletePractice(This);
        }

        public async Task LoadPlayerList()
        {
            var t = await _teamModel.GetTeam(_sessionModel.TeamId);
            _playerList = t.Users.Where(u => (RoleTypes)u.RoleId == RoleTypes.Player).ToList();
            RaisePropertyChanged("PlayerList");
        }
    }
}
