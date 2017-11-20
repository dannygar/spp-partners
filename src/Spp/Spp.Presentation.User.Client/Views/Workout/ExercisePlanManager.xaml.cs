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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using MicrosoftSportsScience.Annotations;

namespace MicrosoftSportsScience
{
    public sealed partial class ExercisePlanManager : Page, INotifyPropertyChanged
    {
        SplitView rootPage = Shell.Current;
        public AthleteWorkoutViewModel SelectedExercisesViewModel { get; set; }
        public WorkoutExercisesViewModel ExercisesViewModel { get; set; }
        private List<SelectExerciseTileUserControl> _exerciseTiles = new List<SelectExerciseTileUserControl>();

        public ExercisePlanManager()
        {
            this.InitializeComponent();
        }


        private int _cumulativeTrainingLoad = 0;

        public int CumulativeTrainingLoad
        {
            get { return _cumulativeTrainingLoad; }
            set
            {
                _cumulativeTrainingLoad = value;
                this.OnPropertyChanged();
            }
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ExercisesViewModel = new WorkoutExercisesViewModel();
            await ExercisesViewModel.Load();

            ExerciseList.ItemsSource = ExercisesViewModel.Exercises;

            if (ExerciseList.Items == null) return;

            //Get topic and subtopic for the new practice session
            var userEntries = (ModalDialogEntries)e.Parameter;

            SelectedExercisesViewModel = new AthleteWorkoutViewModel(new AthleteWorkout()
            {
                Name = userEntries?.Entry1,
                Topic = userEntries?.Entry2,
                SubTopic = userEntries?.Entry3,
                Exercises = new List<AthleteExercise>(),
            })
            {
                EstimatedTrainingLoad = ExercisesViewModel.CumulativeTrainingLoad,
                Exercises = new List<AthleteExerciseViewModel>(),
                PreviousPage = typeof(ExercisePlanManager),
            };



            //Update the binding for the asynchronous loading of items
            this.Bindings.Update();

            await Task.Delay(500);

            for (var i = 0; i < ExerciseList.Items?.Count; i++)
            {
                var container = ExerciseList.ContainerFromIndex(i);
                var tile = HelperMethods.FindVisualChild<SelectExerciseTileUserControl>(container);
                tile.TileSelected += Tile_TileSelected;
                tile.TileUnSelected += Tile_TileUnSelected;
                _exerciseTiles.Add(tile);
            }

            base.OnNavigatedTo(e);
        }


        private void Tile_TileSelected(object sender, SelectExerciseTileUserControl e)
        {
            this.CumulativeTrainingLoad += int.Parse(e.TrainingLoad);

            //Find the selected drill from the list of all drills
            foreach (var exercise in ExercisesViewModel.Exercises)
            {
                if (exercise.Id == ((AthleteExerciseViewModel)e.DataContext).Id)
                    SelectedExercisesViewModel.Exercises.Add(exercise);
            }
        }


        private void Tile_TileUnSelected(object sender, SelectExerciseTileUserControl e)
        {
            this.CumulativeTrainingLoad -= int.Parse(e.TrainingLoad);
            if (this.CumulativeTrainingLoad < 0) this.CumulativeTrainingLoad = 0;

            //Find the selected drill from the list of all drills and then remove it from the collection of the selected drills
            foreach (var exercise in ExercisesViewModel.Exercises)
            {
                if (exercise.Id == ((AthleteExerciseViewModel)e.DataContext).Id)
                    SelectedExercisesViewModel.Exercises.Remove(exercise);
            }
        }


        private void BackButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(WorkoutManager));
        }

        private void NextButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(EditWorkoutDetails), SelectedExercisesViewModel);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
