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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Helpers;
using MicrosoftSportsScience.Models;
using MicrosoftSportsScience.UserControls;
using MicrosoftSportsScience.Data;

namespace MicrosoftSportsScience
{
    public sealed partial class SessionPlanner : Page
    {
        public AthletePracticeViewModel PracticeViewModel;
        SplitView rootPage = Shell.Current;
        private List<DrillTileUserControl> drillTiles = new List<DrillTileUserControl>();
        public RecommendedLoadViewModel LoadViewModel { get; set; }


        public SessionPlanner()
        {
            this.InitializeComponent();
            rootPage.CompactPaneLength = 50;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            progress.Visibility = Visibility.Visible;

            PracticeViewModel = (AthletePracticeViewModel)e.Parameter;
            //load details of practice
            await PracticeViewModel.Load();

            LoadViewModel = new RecommendedLoadViewModel();
            await LoadViewModel.Load();

            progress.Visibility = Visibility.Collapsed;
            
            DrillsList.ItemsSource = PracticeViewModel?.Drills;

            this.Bindings.Update();

            await Task.Delay(500);


            for (var i = 0; i < DrillsList.Items?.Count; i++)
            {
                var container = DrillsList.ContainerFromIndex(i);
                var drillTile = HelperMethods.FindVisualChild<DrillTileUserControl>(container);
                if (drillTile != null)
                {
                    drillTile.DrillUpdated += DrillTile_DrillUpdated;
                    drillTile.DrillRemoved += DrillTile_DrillRemoved;
                    drillTiles.Add(drillTile);
                }
            }


            //Calculate Cumulative Training Load
            if (PracticeViewModel != null)
                PracticeViewModel.EstimatedTrainingLoad = CalculateTotalLoad();
        }

        private void DrillTile_DrillRemoved(object sender, DrillTileUserControl e)
        {
            drillTiles.Remove(e);
            PracticeViewModel.EstimatedTrainingLoad = CalculateTotalLoad();

            //Also, remove from the parent model view
            var tobeRemoved = PracticeViewModel.Drills.Where(drillViewModel => drillViewModel.Name == e.DrillTitle 
                && drillViewModel.Description == e.DrillDescription).ToList();
            foreach (var viewModel in tobeRemoved)
            {
                PracticeViewModel.RemoveDrill(viewModel);
            }
        }

        private void DrillTile_DrillUpdated(object sender, int e)
        {
            PracticeViewModel.EstimatedTrainingLoad = CalculateTotalLoad();
        }

        private int CalculateTotalLoad()
        {
            return drillTiles.Sum(drillTile => (drillTile.DataContext as AthleteDrillViewModel).TrainingLoad);
        }


        private void BackButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(PracticeViewModel != null && PracticeViewModel.PreviousPage != null)
                (rootPage.Content as Frame).Navigate(PracticeViewModel.PreviousPage, new ModalDialogEntries()
                {
                    Entry1 = PracticeViewModel.Name,
                    Entry2 = PracticeViewModel.Topic,
                    Entry3 = PracticeViewModel.SubTopic,
                });
            else
                (rootPage.Content as Frame).Navigate(typeof(PracticeSessionManager));
        }

        private async void NextButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var session = SimpleIoc.Default.GetInstance<AppSessionModel>();
            var _practiceModel = SimpleIoc.Default.GetInstance<AthletePracticeModel>();

            //Save the new practice session
            var newPractice = new AthletePractice()
            {
                Name = PracticeViewModel.Name,
                Topic = PracticeViewModel.Topic,
                SubTopic = PracticeViewModel.SubTopic,
                SessionId = (int)session?.CurrentSession?.Id,
                EstimatedTrainingLoad = PracticeViewModel.EstimatedTrainingLoad,
                RecommendedTrainingLoad = PracticeViewModel.CalculatedRecommendedTrainingLoad,
                TeamId = (int)session?.TeamId,
                IsModified = false,
                PracticeDrills = new List<PracticeDrill>(),
            };

            var drillSequence = 1;
            foreach (var drillMode in PracticeViewModel.Drills)
            {
                newPractice.PracticeDrills.Add(new PracticeDrill()
                {
                    DrillId = drillMode.DrillId,
                    IsModified = false,
                    Duration = drillMode.Duration,
                    Size = drillMode.Size,
                    NumberOfPlayers = drillMode.NumberOfPlayers,
                    Sequence = drillSequence++,
                    CalculatedTrainingLoad = drillMode.TrainingLoad,
                });
            }
            
            //Save a new practice to the DB
            await _practiceModel.SaveAthletePracticeForSession(newPractice);

            //Go Back to dashboard... if available in the backstack we go back to it and clear the navigation stack
            var f = rootPage.Content as Frame;

            if (f.BackStack.Where(b => b.SourcePageType == typeof(TrainingDashboard)) != null)
            {
                while (f.BackStack[f.BackStackDepth - 1].SourcePageType != typeof(TrainingDashboard))
                {
                    f.BackStack.RemoveAt(f.BackStackDepth - 1);
                }
                f.GoBack();
            }
            
            else //not in the backstack so we arrived here through other means. Let's just navigate to it.
            {
                f.Navigate(typeof(TrainingDashboard));
            }

        }


    }
}
