/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Spp.Presentation.User.Client
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
