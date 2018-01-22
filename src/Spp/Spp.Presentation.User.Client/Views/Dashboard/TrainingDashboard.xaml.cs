/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Spp.Presentation.User.Client.ViewModels.Dashboard;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Spp.Presentation.User.Client
{
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
