/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Models;
using MicrosoftSportsScience.ViewModels;
using MicrosoftSportsScience.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MicrosoftSportsScience.Helpers;
using System.Threading.Tasks;

namespace MicrosoftSportsScience
{
    public sealed partial class Workouts : Page
    {
        public AthleteWorkoutViewModel WorkoutViewModel { get; set; }
        private List<ExerciseTileUserControl> exerciseTiles = new List<ExerciseTileUserControl>();
        private int activeExerciseIndex = 0;

        public Workouts()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var workoutModel = SimpleIoc.Default.GetInstance<AthleteWorkoutModel>();
            var sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();

            if (sessionModel.CurrentUser.IsCoach)
                return;

            var workout = await workoutModel.GetAthleteWorkout(sessionModel.CurrentUser, sessionModel.CurrentSession);
            if (workout != null) WorkoutViewModel = new AthleteWorkoutViewModel(workout);
            
            this.Bindings.Update();
            await Task.Delay(500);

            for (int j = 0; j < ExerciseTiles.Items.Count; j++)
            {
                var container = ExerciseTiles.ContainerFromIndex(j);
                if (container != null)
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(container); i++)
                    {
                        var child = VisualTreeHelper.GetChild(container, i);
                        if (child is Control)
                        {
                            exerciseTiles.Add(child as ExerciseTileUserControl);
                        }
                    }
                }
            }

            if (exerciseTiles != null && exerciseTiles[activeExerciseIndex] != null)
                exerciseTiles[activeExerciseIndex].OpenTile();
        }

        private async void ExerciseTile_OnCompleted(object sender, EventArgs e)
        {
            exerciseTiles[activeExerciseIndex].RemoveDoneHandler();
            await Task.Delay(1000);
            if(exerciseTiles.Count > activeExerciseIndex + 1)
                exerciseTiles[activeExerciseIndex + 1].OpenTile();

            activeExerciseIndex = activeExerciseIndex + 1;
           
        }


        private void Button_Done(object sender, TappedRoutedEventArgs e)
        {
            var sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
            (sessionModel.ContentView.Content as Frame).Navigate(typeof(AnswerPostQuestions));
        }
    }
}
