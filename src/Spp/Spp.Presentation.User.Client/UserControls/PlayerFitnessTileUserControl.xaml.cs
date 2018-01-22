/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Spp.Presentation.User.Client.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class PlayerFitnessTileUserControl : UserControl
    {
        SplitView rootPage = Shell.Current;

        public List<PlayerFitnessViewModel> PlayerList
        {
            get { return (List<PlayerFitnessViewModel>)GetValue(PlayerListProperty); }
            set { SetValue(PlayerListProperty, value); }
        }

        public static readonly DependencyProperty PlayerListProperty =
            DependencyProperty.Register("PlayerList", typeof(List<PlayerFitnessViewModel>), typeof(PlayerFitnessTileUserControl), null);

        public PlayerFitnessTileUserControl()
        {
            this.InitializeComponent();
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(Player360View), (sender as Button).DataContext as PlayerFitnessViewModel);
        }


        private void panel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement panel = sender as FrameworkElement;
            if (panel.Name == "panel1")
            {
                panel1.Opacity = 1;
                Image1Rotation.Rotation = 0;
                panel2.Opacity = 0.5;
                Image2Rotation.Rotation = -90;
                panel3.Opacity = 0.5;
                Image3Rotation.Rotation = -90;
            }
            else if (panel.Name == "panel2")
            {
                panel1.Opacity = 0.5;
                Image1Rotation.Rotation = -90;
                panel2.Opacity = 1;
                Image2Rotation.Rotation = 0;
                panel3.Opacity = 0.5;
                Image3Rotation.Rotation = -90;
            }
            else
            {
                panel1.Opacity = 0.5;
                Image1Rotation.Rotation = -90;
                panel2.Opacity = 0.5;
                Image2Rotation.Rotation = -90;
                panel3.Opacity = 1;
                Image3Rotation.Rotation = 0;
            }
        }
    }
}
