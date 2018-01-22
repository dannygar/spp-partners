/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Spp.Presentation.User.Client
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
