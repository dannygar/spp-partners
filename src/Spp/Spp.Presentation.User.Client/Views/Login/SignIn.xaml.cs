/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Helpers;
using Spp.Presentation.User.Client.Models;
using Spp.Presentation.User.Client.Services;
using Spp.Presentation.User.Client.UserControls;
using Spp.Presentation.User.Client.ViewModels;
using Spp.Presentation.User.Client.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Spp.Presentation.User.Client
{
    public sealed partial class SignIn : Page
    {
        private static readonly DependencyProperty SigningInProperty = DependencyProperty.Register("SigningIn", typeof(bool), typeof(SignIn), new PropertyMetadata(false));
        private static readonly DependencyProperty SignedInProperty = DependencyProperty.Register("SignedIn", typeof(bool), typeof(SignIn), new PropertyMetadata(false));

        private AppSessionModel appSessionModel;
        private ILogService logService;


        readonly SplitView rootPage = Shell.Current;
        public LoginViewModel UsersModel { get; set; }

        public bool SignedIn
        {
            get { return (bool)GetValue(SignedInProperty); }
            set { SetValue(SignedInProperty, value); }
        }

        public bool SigningIn
        {
            get { return (bool)this.GetValue(SigningInProperty); }
            set { this.SetValue(SigningInProperty, value); }
        }
        public SignIn()
        {
            this.InitializeComponent();
            TileWidth = 200;
            rootPage.CompactPaneLength = 0;
            logService = SimpleIoc.Default.GetInstance<ILogService>();

            appSessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();

        }

        public double TileWidth
        {
            get { return (double)GetValue(TileWidthProperty); }
            set { SetValue(TileWidthProperty, value); }
        }

        public static readonly DependencyProperty TileWidthProperty =
            DependencyProperty.Register("TileWidth", typeof(double), typeof(SignIn), null);



        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.SignedIn = false;
            this.SigningIn = true;
            this.SignInText.Text = "Signing in...";
            this.BlinkStoryboard.Begin();
            this.SignInText.Visibility = Visibility.Visible;
            this.TeamMottoStoryboard.Begin();
            this.TeamMottoText.Visibility = Visibility.Visible;

            if (appSessionModel.TeamId > 0)
            {
                //The authenticated User has returned to the main dashboard
                await Authenticated(e.Parameter);
            }
            else
            {
                // Authenticate user
                var authService = SimpleIoc.Default.GetInstance<IApiAuthService>();

                //Authenticate with AADv2
                appSessionModel.TeamId = await authService.AuthenticateUserAsync();

                if (appSessionModel.TeamId == 0) //Failed to authenticate
                {
                    logService.Error("Unable to sign in the user", this);
                    (rootPage.Content as Frame).Navigate(typeof(AccessDenied));
                }
                else
                {
                    await Authenticated(e.Parameter);
                }
            }
        }




        private async Task Authenticated(object navigatedFromPage)
        {
            if (appSessionModel.TeamId > 0)
            {
                this.SignInText.Text = "Loading...";

                if (!(navigatedFromPage is GreetingPage))
                {
                    //Already has been signed, no need to show "Sign in" sign
                    this.SignedIn = true;
                    this.BlinkStoryboard.Stop();
                    this.SignInText.Visibility = Visibility.Collapsed;
                }

                // Load all players for now, in the future this should load the players from the session for the day
                UsersModel = new LoginViewModel();
                await UsersModel.Load();

                //show players first and coaches last
                this.PlayerList.ItemsSource = UsersModel.Users.OrderBy(u => u.IsCoach);

                //get compleness information on each user...asynchronously
                foreach (var u in UsersModel.Users)
                {
                    await u.LoadCompleteness();
                }

                if (CSSettingsHelper.Instance.EnableFaceRecognition)
                {
                    //Retrieve Cognitive Services API keys
                    await appSessionModel.GetCognitiveServiceKeys(appSessionModel.TeamId);
                }

                //If this came from the GreetingPage, jump directly into the user's corresponding dashboard page
                if (navigatedFromPage is GreetingPage)
                {
                    var userTile =
                        (UsersModel.Users.Any(u => u.User.FullName == appSessionModel.CurrentUser.FullName)) ?
                            UsersModel.Users.First(
                            u => u.User.FullName == appSessionModel.CurrentUser.FullName) : null;
                    if (userTile != null)
                    {
                        var session = SimpleIoc.Default.GetInstance<AppSessionModel>();
                        session.CurrentUser = userTile.User;
                        session.ContentView.Pane.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        session.AppShell.SetUserPhoto(userTile.User.PathToPhoto);

                        // Navigate to appropriate page
                        if (session.CurrentUser.IsAthlete)
                            (rootPage.Content as Frame).Navigate(typeof(AnswerQuestions));
                        else
                            (rootPage.Content as Frame).Navigate(typeof(TrainingDashboard));
                    }
                }

                this.SigningIn = false;
                this.SignedIn = true;
                this.BlinkStoryboard.Stop();
                this.SignInText.Visibility = Visibility.Collapsed;
                this.TeamMottoStoryboard.Stop();
                //this.TeamMottoText.Visibility = Visibility.Collapsed;
            }
            else
            {
                logService.Error("Unable to sign in the user", this);
                (rootPage.Content as Frame).Navigate(typeof(AccessDenied));
            }
        }




        private void SignInStoryboard_Completed(object sender, object e)
        {
            (rootPage.Content as Frame).Navigate(typeof(AnswerQuestions));
        }

        private void PlayerTileUserControl_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var tile = (PlayerTileUserControl)sender;
            var session = SimpleIoc.Default.GetInstance<AppSessionModel>();

            session.CurrentUser = tile.User;
            session.ContentView.Pane.Visibility = Windows.UI.Xaml.Visibility.Visible;
            session.AppShell.SetUserPhoto(tile.User.PathToPhoto);

            // Navigate to appropriate page
            if (session.CurrentUser.IsAthlete)
                (rootPage.Content as Frame).Navigate(typeof(AnswerQuestions));
            else
                (rootPage.Content as Frame).Navigate(typeof(TrainingDashboard));
        }


        private void SignInPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double padding = PlayerList.ActualWidth % TileWidth;
            PlayerList.Padding = new Thickness(padding / 2, 0, 0, 0);
        }

        private void Calendar_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(Calendar));
        }

        private void OnGoToSettingsClick(object sender, RoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(SettingsPage), this);
        }
    }
}
