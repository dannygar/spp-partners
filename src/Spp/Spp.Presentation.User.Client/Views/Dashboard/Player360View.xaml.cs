/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Spp.Presentation.User.Client.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Spp.Presentation.User.Client
{
    public sealed partial class Player360View : Page
    {
        SplitView rootPage = Shell.Current;

        public PlayerFitnessViewModel ViewModel { get; set; }

        public Player360View()
        {
            this.InitializeComponent();
            rootPage.CompactPaneLength = 50;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = e.Parameter as PlayerFitnessViewModel;
        }
    }
}
