/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Models;
using Spp.Presentation.User.Client.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Spp.Presentation.User.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Calendar : Page
    {
        public List<CalendarDayViewModel> Days;
        SplitView rootPage = Shell.Current;

        public Calendar()
        {
            this.InitializeComponent();
            rootPage.CompactPaneLength = 50;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var appSessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
            var sessionModel = SimpleIoc.Default.GetInstance<AthleteSessionModel>();
            var workoutModel = SimpleIoc.Default.GetInstance<AthleteWorkoutModel>();
            var practiceModel = SimpleIoc.Default.GetInstance<AthletePracticeModel>();

            // Get sessions for the shown date-range
            var sessions = await sessionModel.GetSessions(DateTime.Now.AddDays(-7), DateTime.Now.AddDays(7));

            if (sessions == null || sessions.Count <= 0)
                return;

            // Get workouts for the sessions
            var workouts = await workoutModel.GetAthleteWorkouts(appSessionModel.CurrentUser as Athlete, sessions);

            // Get practices for the sessions
            var practices = await practiceModel.GetAthletePractices(sessions.Select(x => x.Id).ToList());

            Days = new List<CalendarDayViewModel>();

            // Generate days
            for (int i = -7; i < 7; i++)
            {
                DateTime day = DateTime.Now.AddDays(i);
                var session = sessions.FirstOrDefault(x => x.Scheduled.Month == day.Month && x.Scheduled.Day == day.Day);

                if (session == null)
                    continue;

                Days.Add(new CalendarDayViewModel(session, practices.Where(x => x.SessionId == session.Id).ToList(), workouts.Where(x => x.SessionId == session.Id).ToList(), day));
            }
        }
    }
}
