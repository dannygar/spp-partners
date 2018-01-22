/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Models;
using Spp.Presentation.User.Client.ViewModels;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Spp.Presentation.User.Client
{
    using Data;

    public sealed partial class PracticeSessionPlannerAddPlayerGroups : Page
    {
        SplitView rootPage = Shell.Current;
        AthletePracticeViewModel PracticeViewModel;

        public RecommendedLoadViewModel LoadViewModel { get; set; }

        public PracticeSessionPlannerAddPlayerGroups()
        {
            this.InitializeComponent();
            rootPage.CompactPaneLength = 50;

            LoadViewModel = new RecommendedLoadViewModel();

            startDatePicker.Date = new DateTimeOffset(DateTime.Today);
            endDatePicker.Date = new DateTimeOffset(DateTime.Today.AddYears(1));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            progress.Visibility = Visibility.Visible;

            PracticeViewModel = (AthletePracticeViewModel)e.Parameter;

            await LoadViewModel.Load();
            await PracticeViewModel.Load();
            await PracticeViewModel.LoadPlayerList();

            progress.Visibility = Visibility.Collapsed;
        }



        private void BackButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(PracticeSessionManager));
        }

        private async void NextButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PracticeViewModel.StartDate = startDatePicker.Date.Value.Date;
            PracticeViewModel.EndDate = endDatePicker.Date.Value.Date;

            if (postPracticeQuestionsSwitch.IsOn)
                (rootPage.Content as Frame).Navigate(typeof(CreateQuestions), PracticeViewModel);

            else
            {
                var sessionModel = SimpleIoc.Default.GetInstance<AthleteSessionModel>();

                var session = new Session
                {
                    Location = new Location
                    {
                        Id = 1,
                        Address = "Some place",
                        Name = "Training Facility A",
                        Type = LocationType.Stadium,
                    },
                    Scheduled = PracticeViewModel.StartDate,
                    SessionType = "1",
                };


                //save the session on the DB
                var sessionId = await sessionModel.SaveSession(session);

                //Now that we have the ID, we need to update the session with the users
                session.Id = sessionId;
                session.Users = PracticeViewModel.AssignedUsers.ToList();
                await sessionModel.SaveSessionUsers(session);

                //Finally, we need to update the practice object to update the sessionId
                var result = await PracticeViewModel.UpdatePracticeWithSessionId(sessionId);

                if (result)
                {
                    MessageDialog dlg = new MessageDialog("Session Scheduled!");
                    await dlg.ShowAsync();
                }
                else
                {
                    MessageDialog dlg = new MessageDialog("Something went wrong. Please try again later.");
                    await dlg.ShowAsync();
                }

                //Navigate to the scheduling
                (rootPage.Content as Frame).Navigate(typeof(Calendar));
            }
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var p in e.AddedItems)
            {
                var player = (User)p;
                PracticeViewModel.AssignedUsers.Add(player);
            }

            foreach (var p in e.RemovedItems)
            {
                var player = (User)p;
                PracticeViewModel.AssignedUsers.Remove(player);
            }
        }
    }
}
