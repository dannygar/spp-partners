/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using MicrosoftSportsScience.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Models;
using System.Collections.ObjectModel;
using MicrosoftSportsScience.Data;

namespace MicrosoftSportsScience
{
    public sealed partial class WorkoutPlanManagerEditWorkout : Page
    {
        public ObservableCollection<AthleteExercise> ExerciseSets = new ObservableCollection<AthleteExercise>();
        public ObservableCollection<AthleteExercise> ExerciseSets1 = new ObservableCollection<AthleteExercise>();
        SplitView rootPage = Shell.Current;
        
        public WorkoutPlanManagerEditWorkout()
        {
            this.InitializeComponent();

            

            for (int i = 0; i < 4; i++)
            {AthleteExercise tempModel = new AthleteExercise();
                tempModel.Name = "Exercise Name " + i.ToString();
                tempModel.TrainingLoad = (i * 100).ToString();
                ExerciseSets.Add(tempModel);
                ExerciseSets1.Add(tempModel);
            }


            rootPage.CompactPaneLength = 50;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void Button_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(WorkoutPlanManager));
        }
    }
}
