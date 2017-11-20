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
    public sealed partial class PracticeSessionManager : Page
    {
        SplitView rootPage = Shell.Current;
        public PracticesViewModel PracticesViewModel { get; set; }

        public PracticeSessionManager()
        {
            this.InitializeComponent();
            rootPage.CompactPaneLength = 50;
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Modal.CloseModal();

            progress.Visibility = Visibility.Visible;

            PracticesViewModel = new PracticesViewModel();
            await PracticesViewModel.Load();

            progress.Visibility = Visibility.Collapsed;

            //Update the binding for the asynchronous loading of items
            //this.Bindings.Update();

           // await Task.Delay(500);

            var practice = new AthletePractice() { Topic = "New Practice" };
            var practiceView = new AthletePracticeViewModel(practice);
            var listofPractices = new List<AthletePracticeViewModel>() { practiceView };

            listofPractices.AddRange(PracticesViewModel.Practices);
            PracticesList.ItemsSource = listofPractices;

            base.OnNavigatedTo(e);
        }

        private void AddNewWorkoutPlan(object sender, TappedRoutedEventArgs e)
        {
            Modal.OpenModal();
        }
    }
}
