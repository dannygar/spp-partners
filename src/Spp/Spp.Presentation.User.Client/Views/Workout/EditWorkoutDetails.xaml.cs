/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Helpers;
using Spp.Presentation.User.Client.Models;
using Spp.Presentation.User.Client.UserControls;
using Spp.Presentation.User.Client.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Spp.Presentation.User.Client
{
    public sealed partial class EditWorkoutDetails : Page
    {
        public ObservableCollection<AthleteExerciseSetViewModel> ExerciseSets = new ObservableCollection<AthleteExerciseSetViewModel>();
        SplitView rootPage = Shell.Current;
        public AthleteWorkoutViewModel WorkoutViewModel;
        private List<LargeExerciseTileUserControl> _exerciseTiles = new List<LargeExerciseTileUserControl>();
        private readonly AthleteWorkoutModel _apiWorkout;

        public EditWorkoutDetails()
        {
            this.InitializeComponent();
            rootPage.CompactPaneLength = 50;
            _apiWorkout = SimpleIoc.Default.GetInstance<AthleteWorkoutModel>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            WorkoutViewModel = (AthleteWorkoutViewModel)e.Parameter;

            ExercisesList.ItemsSource = WorkoutViewModel?.Exercises;

            this.Bindings.Update();

            await Task.Delay(500);


            for (var i = 0; i < ExercisesList.Items?.Count; i++)
            {
                var container = ExercisesList.ContainerFromIndex(i);
                var exerTile = HelperMethods.FindVisualChild<LargeExerciseTileUserControl>(container);
                if (exerTile != null)
                {
                    exerTile.ExerciseUpdated += ExerTile_ExerciseUpdated;
                    exerTile.ExerciseRemoved += ExerTile_ExerciseRemoved;
                    _exerciseTiles.Add(exerTile);
                }
            }


            if (WorkoutViewModel != null)
            {
                //Calculate Cumulative Training Load
                WorkoutViewModel.EstimatedTrainingLoad = CalculateTotalLoad();

                //Get the exercises set
                foreach (var exercise in WorkoutViewModel.Workout.Exercises)
                {
                    //FOR DEMO PURPOSES ONLY! 
                    //TODO: Change per customer's requirements
                    for (int i = 1; i <= (exercise.Sets.Sets + 1); i++)
                    {
                        ExerciseSets.Add(new AthleteExerciseSetViewModel(exercise.Sets, i));
                    }
                }
            }
        }

        private void ExerTile_ExerciseRemoved(object sender, LargeExerciseTileUserControl e)
        {
            _exerciseTiles.Remove(e);
            CalculateTotalLoad();

            //Also, remove from the parent model view
            var tobeRemoved = WorkoutViewModel.Exercises.Where(model => model.Name == e.ExerciseTitle
                && model.Description == e.ExerciseDescription && model.TrainingLoad == e.TrainingLoad).ToList();
            foreach (var viewModel in tobeRemoved)
            {
                WorkoutViewModel.Exercises.Remove(viewModel);
            }
        }

        private void ExerTile_ExerciseUpdated(object sender, int e)
        {
            WorkoutViewModel.EstimatedTrainingLoad = CalculateTotalLoad();
        }


        private int CalculateTotalLoad()
        {
            return _exerciseTiles.Sum(tile => (AthleteExerciseSetViewModel.CalculateTrainingLoad(tile.Sets[0].Reps, tile.Sets[0].Weight)));
        }

        private void BackButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (WorkoutViewModel.PreviousPage != null)
                (rootPage.Content as Frame).Navigate(WorkoutViewModel.PreviousPage, new ModalDialogEntries()
                {
                    Entry1 = WorkoutViewModel.Name,
                    Entry2 = WorkoutViewModel.Workout.Topic,
                    Entry3 = WorkoutViewModel.Workout.SubTopic,
                });
            else
                (rootPage.Content as Frame).Navigate(typeof(WorkoutManager));
        }

        private async void SaveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Save the updated workout to the Database
            if (WorkoutViewModel.PreviousPage != null)
            {
                var session = SimpleIoc.Default.GetInstance<AppSessionModel>();
                var newWorkout = new AthleteWorkout()
                {
                    SessionId = session.CurrentSession.Id,
                    Name = WorkoutViewModel.Name,
                    Topic = WorkoutViewModel.Workout.Topic,
                    SubTopic = WorkoutViewModel.Workout.SubTopic,
                    Category = WorkoutViewModel.Category,
                    Exercises = new List<AthleteExercise>(),
                };
                foreach (var exercise in WorkoutViewModel.Exercises)
                {
                    newWorkout.Exercises.Add(new AthleteExercise()
                    {
                        Id = exercise.Id,
                        Name = exercise.Name,
                        Category = WorkoutViewModel.Category,
                        Description = exercise.Description,
                        ImageUrl = exercise.ImageUrl,
                        IsDone = false,
                        IsModified = false,
                        Duration = exercise.Duration,
                        Note = new Note() { Created = DateTime.UtcNow, Text = exercise.Notes },
                        TrainingLoad = exercise.TrainingLoad,
                        Sets = new AthleteExerciseSet()
                        {
                            Order = exercise.Order,
                            Sets = exercise.NumberOfSets,
                            Reps = exercise.Reps,
                            Weight = exercise.Weight,
                            RecoveryTimeInMin = exercise.RecoveryTimeInMin,
                            TrainingLoad = int.Parse(exercise.TrainingLoad),
                        },
                    });
                }
                //Create a new workout
                await _apiWorkout.CreateAthleteWorkout(newWorkout);
            }
            else
                //Update current workout
                await _apiWorkout.UpdateAthleteWorkout(WorkoutViewModel.Workout);

            //Invalidate (refresh) cache
            _apiWorkout.InvalidateCache(Defines.CACHE_KEY_SESSIONWORKOUTS);

            //Navigate back to the list of workouts
            (rootPage.Content as Frame).Navigate(typeof(WorkoutManager));
        }
    }
}
