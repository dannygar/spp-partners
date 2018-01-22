/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Services;

namespace Spp.Presentation.User.Client.Views
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class AccessDenied : Page
    {
        private readonly SplitView rootPage = Shell.Current;

        public AccessDenied()
        {
            this.InitializeComponent();
            this.rootPage.CompactPaneLength = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.textBlock.Text = "Access Denied. Please, contact the Administrator.";
        }

        private void SignInPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double padding = this.textBlock.ActualWidth % 200;
            this.textBlock.Padding = new Thickness(padding / 2, 0, 0, 0);
        }

        private void BackButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var authService = SimpleIoc.Default.GetInstance<IApiAuthService>();
            authService.SignOut();

            (this.rootPage.Content as Frame).Navigate(typeof(SignIn));
        }


        private void OnSettingsClicked(object sender, TappedRoutedEventArgs e)
        {
            (this.rootPage.Content as Frame).Navigate(typeof(SettingsPage), this);
        }
    }
}
