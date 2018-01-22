/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Models;

namespace Spp.Presentation.User.Client.ViewModels
{
    public class WorkoutExercisesViewModel : NotificationBase
    {
        private AthleteWorkoutModel _apiModel;
        private IList<AthleteExercise> _exercises;
        private List<AthleteExerciseViewModel> _exerModel = new List<AthleteExerciseViewModel>();

        public WorkoutExercisesViewModel()
        {
            _apiModel = SimpleIoc.Default.GetInstance<AthleteWorkoutModel>();
        }

        public List<AthleteExerciseViewModel> Exercises
        {
            get { return _exerModel; }
            set { _exerModel = value; }
        }


        private int _cumulativeTrainingLoad = 0;

        public int CumulativeTrainingLoad
        {
            get { return _cumulativeTrainingLoad; }
            set { this.SetProperty(ref _cumulativeTrainingLoad, value); }
        }



        public override async Task Load()
        {
            _logService.Info("Attempting to load exercises", this);
            _exercises = await _apiModel.GetAllExercises();

            foreach (var exercise in _exercises)
            {
                var np = new AthleteExerciseViewModel(exercise);
                _exerModel.Add(np);
            }
        }
    }
}
