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
    public sealed partial class WorkoutManager : Page
    {
        SplitView rootPage = Shell.Current;
        public WorkoutsViewModel WorkoutsViewModel { get; set; }

        public WorkoutManager()
        {
            this.InitializeComponent();
            rootPage.CompactPaneLength = 50;
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            WorkoutsViewModel = new WorkoutsViewModel();
            await WorkoutsViewModel.Load();

            //Update the binding for the asynchronous loading of items
            //this.Bindings.Update();

            await Task.Delay(500);

            var workouts = new List<AthleteWorkoutViewModel>() { new AthleteWorkoutViewModel(new AthleteWorkout()
            { Topic = "New Workout", Exercises = new List<AthleteExercise>()}), };

            workouts.AddRange(WorkoutsViewModel.WorkoutsVM);
            WorkoutsList.ItemsSource = workouts;

            base.OnNavigatedTo(e);
        }

        private void AddNewWorkout(object sender, TappedRoutedEventArgs e)
        {
            Modal.OpenModal();
            e.Handled = true;
        }
    }
}
