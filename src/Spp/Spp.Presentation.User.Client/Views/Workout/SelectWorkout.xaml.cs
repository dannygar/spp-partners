/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Spp.Presentation.User.Client
{

    public sealed partial class SelectWorkout : Page
    {
        SplitView rootPage = Shell.Current;

        public SelectWorkout()
        {
            this.InitializeComponent();
            rootPage.CompactPaneLength = 50;
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(WorkoutPlanManagerEditWorkout));
        }
    }
}
