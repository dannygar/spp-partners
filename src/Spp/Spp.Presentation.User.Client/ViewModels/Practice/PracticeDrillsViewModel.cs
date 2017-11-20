/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftSportsScience.ViewModels
{
    public class PracticeDrillsViewModel : NotificationBase
    {
        private AthletePracticeModel _practiceModel;
        private IList<PracticeDrill> _drills;
        private List<AthleteDrillViewModel> _drillsModel = new List<AthleteDrillViewModel>();

        public PracticeDrillsViewModel()
        {
            _practiceModel = SimpleIoc.Default.GetInstance<AthletePracticeModel>();
        }

        public List<AthleteDrillViewModel> Drills
        {
            get { return _drillsModel; }
            set { _drillsModel = value; }
        }


        private int _cumulativeTrainingLoad = 0;

        public int CumulativeTrainingLoad
        {
            get { return _cumulativeTrainingLoad; }
            set { this.SetProperty(ref _cumulativeTrainingLoad, value); }
        }



        public override async Task Load()
        {
            _logService.Info("Attempting to load drills", this);
            _drills = await _practiceModel.GetAllDrills();

            foreach (var drill in _drills)
            {
                var np = new AthleteDrillViewModel(drill);
                np.RecommendedTrainingLoad = np.TrainingLoad;
                _drillsModel.Add(np);
            }
        }
    }
}
