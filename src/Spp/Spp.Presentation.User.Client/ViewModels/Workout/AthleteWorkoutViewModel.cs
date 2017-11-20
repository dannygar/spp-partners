/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftSportsScience.ViewModels
{
    public class AthleteWorkoutViewModel : NotificationBase<AthleteWorkout>
    {
        //private List<AthleteExerciseViewModel> _exerciseModels;
        //private ObservableCollection<AthleteExerciseViewModel>  _exercises;

        public List<AthleteExerciseViewModel> Exercises { get; set; }

        public AthleteWorkoutViewModel(AthleteWorkout workout) : base(workout)
        {
            Workout = workout;
            This.Session = workout?.Session;
            Exercises = new List<AthleteExerciseViewModel>();
            foreach (var exercise in workout?.Exercises)
            {
                Exercises.Add(new AthleteExerciseViewModel(exercise));
            }
            //_exercises = (_exerciseModels != null)? new ObservableCollection<AthleteExerciseViewModel>(_exerciseModels) : null;
            //PropertyChanged += AthleteWorkoutViewModel_PropertyChanged;
        }

        private void AthleteWorkoutViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }

        public Type PreviousPage { get; set; }

        public AthleteWorkout Workout { get; set; }

        public string Name => This?.Name;
        public string Timestamp => (This.Session != null)? This.Session.Scheduled.ToString("MMMM", CultureInfo.InvariantCulture) + " " + This.Session.Scheduled.Day + ", " + This.Session.Scheduled.Year
            : "";
        public override Task Load() => null;

        public string Category => This?.Category;

        public string FirstExercise => (Exercises != null && Exercises.Count > 0) ? Exercises.First().Name : "None";

        private int _estimatedTrainingLoad;
        public int EstimatedTrainingLoad
        {
            get { return _estimatedTrainingLoad; }
            set { SetProperty(ref _estimatedTrainingLoad, value); }
        }

    }
}
