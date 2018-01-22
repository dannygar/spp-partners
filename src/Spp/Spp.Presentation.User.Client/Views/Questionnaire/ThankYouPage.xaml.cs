/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Spp.Presentation.User.Client
{
    public sealed partial class ThankYouPage : Page
    {
        public ThankYouPage()
        {
            this.InitializeComponent();
            this.Loaded += ThankYouPage_Loaded;
        }

        private async void ThankYouPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.BlinkStoryboard.Begin();
            await Task.Delay(3000);

            var _session = SimpleIoc.Default.GetInstance<AppSessionModel>();


            //hack to prevent user from being memorized by the session
            _session.CurrentUser = new Data.User();
            _session.CurrentSession = new Data.Session();
            _session.TeamId = -1;

            Frame f = _session.ContentView.Content as Frame;
            var firstFrame = f.BackStack[0];
            f.BackStack.Clear();
            f.BackStack.Add(firstFrame);
            f.GoBack();
        }
    }
}
