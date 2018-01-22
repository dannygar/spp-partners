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
    public class WorkoutsViewModel : NotificationBase
    {
        private readonly AthleteWorkoutModel _apiWorkout;
        private List<AthleteWorkout> _workouts;

        public WorkoutsViewModel()
        {
            _apiWorkout = SimpleIoc.Default.GetInstance<AthleteWorkoutModel>();
        }

        public List<AthleteWorkoutViewModel> WorkoutsVM { get; } = new List<AthleteWorkoutViewModel>();

        public override async Task Load()
        {
            _logService.Info("Attempting to load practices", this);
            var session = SimpleIoc.Default.GetInstance<AppSessionModel>();
            _workouts = (List<AthleteWorkout>)await _apiWorkout.GetSessionWorkouts(session.CurrentSession.Id);

            foreach (var workout in _workouts)
            {
                //calculate training load
                foreach (var exercise in workout.Exercises)
                {
                    exercise.Sets.TrainingLoad = AthleteExerciseSetViewModel.CalculateTrainingLoad(exercise.Sets.Reps,
                        exercise.Sets.Weight);
                }
                var np = new AthleteWorkoutViewModel(workout);
                WorkoutsVM.Add(np);
            }
        }
    }
}
