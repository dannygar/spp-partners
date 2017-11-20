/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Models;
using MicrosoftSportsScience.ViewModels;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MicrosoftSportsScience.UserControls
{
    public sealed partial class ExerciseTileUserControl : UserControl
    {
        private bool DetailsWindowOpen = false;

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        public event ValueChangedEventHandler OnCompleted;

        public ExerciseTileUserControl()
        {
            this.InitializeComponent();
        }

        public string ExerciseIndex
        {
            get { return (string)GetValue(ExerciseIndexProperty); }
            set { SetValue(ExerciseIndexProperty, value); }
        }

        public static readonly DependencyProperty ExerciseIndexProperty =
            DependencyProperty.Register("ExerciseIndex", typeof(string), typeof(ExerciseTileUserControl), null);

        public int ExerciseId
        {
            get { return (int)GetValue(ExerciseIdProperty); }
            set { SetValue(ExerciseIdProperty, value); }
        }

        public static readonly DependencyProperty ExerciseIdProperty =
            DependencyProperty.Register("ExerciseId", typeof(int), typeof(ExerciseTileUserControl), null);

        public string ExerciseCategory
        {
            get { return (string)GetValue(ExerciseCategoryProperty); }
            set { SetValue(ExerciseCategoryProperty, value); }
        }
        
        public static readonly DependencyProperty ExerciseCategoryProperty =
            DependencyProperty.Register("ExerciseCategory", typeof(string), typeof(ExerciseTileUserControl), null );

        public string ExerciseName
        {
            get { return (string)GetValue(ExerciseNameProperty); }
            set { SetValue(ExerciseNameProperty, value); }
        }

        public static readonly DependencyProperty ExerciseNameProperty =
            DependencyProperty.Register("ExerciseName", typeof(string), typeof(ExerciseTileUserControl), null);

        public string ExerciseImage
        {
            get { return (string)GetValue(ExerciseImageProperty); }
            set { SetValue(ExerciseImageProperty, value); }
        }

        public static readonly DependencyProperty ExerciseImageProperty =
            DependencyProperty.Register("ExerciseImage", typeof(string), typeof(ExerciseTileUserControl), null);

        public string ExerciseDescription
        {
            get { return (string)GetValue(ExerciseDescriptionProperty); }
            set { SetValue(ExerciseDescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ExerciseDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExerciseDescriptionProperty =
            DependencyProperty.Register("ExerciseDescription", typeof(string), typeof(ExerciseTileUserControl), null);

        public ObservableCollection<AthleteExerciseSetViewModel> ExerciseSetList
        {
            get { return (ObservableCollection<AthleteExerciseSetViewModel>)GetValue(ExerciseSetListProperty); }
            set { SetValue(ExerciseSetListProperty, value); }
        }

        public static readonly DependencyProperty ExerciseSetListProperty =
            DependencyProperty.Register("ExerciseSetList", typeof(ObservableCollection<AthleteExerciseSetViewModel>), typeof(ExerciseTileUserControl), null);

        public bool Completed
        {
            get { return (bool)GetValue(CompletedProperty); }
            set { SetValue(CompletedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Completed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompletedProperty =
            DependencyProperty.Register("Completed", typeof(bool), typeof(AthleteWorkoutViewModel), null);

        public bool TileActive
        {
            get { return (bool)GetValue(TileActiveProperty); }
            set { SetValue(TileActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Completed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TileActiveProperty =
            DependencyProperty.Register("TileActive", typeof(bool), typeof(AthleteWorkoutViewModel), null);

        private void Details_Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ToggleDetail(!DetailsWindowOpen);
        }

        public void ToggleDetail(bool toggle)
        {
            if (toggle)
            {
                Storyboard sb = this.Resources["DetailsOpen"] as Storyboard;
                sb.Begin();
            }
            else
            {
                Storyboard sb = this.Resources["DetailsClosed"] as Storyboard;
                sb.Begin();
            }

            DetailsWindowOpen = !DetailsWindowOpen;
        }
        
        public void OpenTile()
        {
            Storyboard sb = this.Resources["ActivateTile"] as Storyboard;
            sb.Begin();
        }

        private void Modified_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //await SimpleIoc.Default.GetInstance<AthleteWorkoutModel>().CompleteExercise(this.ExerciseId, true);

            Storyboard sb = this.Resources["DoneModified"] as Storyboard;
            sb.Begin();
            if (OnCompleted != null)
                OnCompleted(this, null);
        }

        private void AsPerscribed_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //await SimpleIoc.Default.GetInstance<AthleteWorkoutModel>().CompleteExercise(this.ExerciseId, false);

            Storyboard sb = this.Resources["Done"] as Storyboard;
            sb.Begin();
            if (OnCompleted != null)
                OnCompleted(this, null);
        }

        private void Complete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Storyboard sb = this.Resources["Complete"] as Storyboard;
            sb.Begin();
            grid1.Tapped -= Complete_Tapped;
        }

        public void RemoveDoneHandler()
        {
            grid3.Tapped -= AsPerscribed_Tapped;
            grid5.Tapped -= Modified_Tapped;
        }
    }
}
