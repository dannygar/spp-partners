/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Models;
using Spp.Presentation.User.Client.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Spp.Presentation.User.Client
{
    public sealed partial class AthleteSummary : Page
    {
        public AthleteQuestionHistoryViewModel AthleteQuestionHistory { get; set; }
        public MessagesViewModel AthleteMessages { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
            UserThanksTextBox.Text += sessionModel.CurrentUser.FirstName;

            if (sessionModel.CurrentUser.IsCoach)
                return;

            AthleteQuestionHistory = new AthleteQuestionHistoryViewModel();
            AthleteMessages = new MessagesViewModel();

            await AthleteQuestionHistory.Load();
            await AthleteMessages.Load();

            WellnessControl.WellnessScore = AthleteQuestionHistory.Wellness;
            this.Bindings.Update();
        }

        public AthleteSummary()
        {
            this.InitializeComponent();
        }

        private void Button_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
            (sessionModel.ContentView.Content as Frame).Navigate(typeof(Workouts));
        }
    }
}
