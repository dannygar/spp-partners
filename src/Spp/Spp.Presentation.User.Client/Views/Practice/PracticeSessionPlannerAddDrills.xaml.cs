/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Annotations;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Helpers;
using MicrosoftSportsScience.UserControls;
using MicrosoftSportsScience.ViewModels;
using MicrosoftSportsScience.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MicrosoftSportsScience
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PracticeSessionPlannerAddDrills : Page, INotifyPropertyChanged
    {
        readonly SplitView rootPage = Shell.Current;
        public PracticeDrillsViewModel DrillsViewModel { get; set; }
        public AthletePracticeViewModel PracticeViewModel { get; set; }
        private List<AddDrillsUserControl> drillTiles = new List<AddDrillsUserControl>();
        public RecommendedLoadViewModel LoadViewModel { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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


        public PracticeSessionPlannerAddDrills()
        {
            this.InitializeComponent();
            
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            progress.Visibility = Visibility.Visible;

            LoadViewModel = new RecommendedLoadViewModel();
            await LoadViewModel.Load();

            DrillsViewModel = new PracticeDrillsViewModel();
            await DrillsViewModel.Load();

            progress.Visibility = Visibility.Collapsed;


            DrillsList.ItemsSource = DrillsViewModel.Drills;
         

            if (DrillsList.Items == null) return;

            //Get topic and subtopic for the new practice session
            var userEntries = (ModalDialogEntries)e.Parameter;

            PracticeViewModel = new AthletePracticeViewModel(new AthletePractice()
            {
                EstimatedTrainingLoad = DrillsViewModel.CumulativeTrainingLoad,
                Name = userEntries?.Entry1,
                Topic = userEntries?.Entry2,
                SubTopic = userEntries?.Entry3,
            })
            {
                Drills = new List<AthleteDrillViewModel>(),
                PreviousPage = typeof(PracticeSessionPlannerAddDrills),
                IsNewPractice = true,
            };

     
            //Update the binding for the asynchronous loading of items
            this.Bindings.Update();

            await Task.Delay(500);

            for (var i = 0; i < DrillsList.Items?.Count; i++)
            {
                var container = DrillsList.ContainerFromIndex(i);
                var drillTile = HelperMethods.FindVisualChild<AddDrillsUserControl>(container);
                drillTile.DrillSelected += DrillTile_DrillSelected;
                drillTile.DrillUnSelected += DrillTile_DrillUnSelected;
                drillTiles.Add(drillTile);
            }

            base.OnNavigatedTo(e);
        }

        private void DrillTile_DrillSelected(object sender, AddDrillsUserControl e)
        {
            this.CumulativeTrainingLoad += int.Parse(e.TrainingLoad);
            PracticeViewModel.AddDrill((AthleteDrillViewModel)e.DataContext);
        }

        private void DrillTile_DrillUnSelected(object sender, AddDrillsUserControl e)
        {
            this.CumulativeTrainingLoad -= int.Parse(e.TrainingLoad);
            if (this.CumulativeTrainingLoad < 0) this.CumulativeTrainingLoad = 0;
            PracticeViewModel.RemoveDrill((AthleteDrillViewModel)e.DataContext);

        }


        private void BackButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(PracticeSessionManager));
        }

        private void NextButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
             (rootPage.Content as Frame).Navigate(typeof(SessionPlanner), PracticeViewModel);
        }

    }
}
