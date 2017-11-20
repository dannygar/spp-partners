/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Models;
using MicrosoftSportsScience.ViewModels;
using MicrosoftSportsScience.ViewModels.Dashboard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MicrosoftSportsScience
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TrainingDashboard : Page
    {
        SplitView rootPage = Shell.Current;

        public TrainingDashboardViewModel ViewModel { get; set; }

        public TrainingDashboard()
        {
            this.InitializeComponent();
            this.rootPage.CompactPaneLength = 50;
            this.ViewModel = new TrainingDashboardViewModel();
            this.Loaded += TrainingDashboard_Loaded;
        }

        private async void TrainingDashboard_Loaded(object sender, RoutedEventArgs e)
        {
               await this.ViewModel.Load();
        }

        private void WorkoutsTapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(WorkoutManager));
        }

        private void WorkoutPlansTapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(WorkoutPlanManager));
        }

        private void PracticeSessionsTapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(PracticeSessionManager));
        }

        private void ExercisesTapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(ExercisePlanManager));
        }

        private void DrillsTapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(PracticeSessionPlannerAddDrills));
        }
    }
}
