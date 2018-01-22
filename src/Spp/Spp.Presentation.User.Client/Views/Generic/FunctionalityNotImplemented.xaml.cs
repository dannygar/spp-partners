/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Spp.Presentation.User.Client.Views.Generic
{
    public sealed partial class FunctionalityNotImplemented : Page
    {
        private readonly SplitView rootPage = Shell.Current;


        public FunctionalityNotImplemented()
        {
            this.InitializeComponent();
            this.rootPage.CompactPaneLength = 0;
        }


        private void SignInPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double padding = this.textBlock.ActualWidth % 200;
            this.textBlock.Padding = new Thickness(padding / 2, 0, 0, 0);
        }

        private void BackButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            (this.rootPage.Content as Frame).Navigate(typeof(WorkoutPlanManager));
        }

    }
}
