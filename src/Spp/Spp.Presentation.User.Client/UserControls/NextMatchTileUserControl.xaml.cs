/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class NextMatchTileUserControl : UserControl
    {
        SplitView rootPage = Shell.Current;

        public NextMatchTileUserControl()
        {
            this.InitializeComponent();
        }

        public bool IncludeHistory
        {
            get { return (bool)GetValue(IncludeHistoryProperty); }
            set { SetValue(IncludeHistoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IncludeHistory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IncludeHistoryProperty =
            DependencyProperty.Register("IncludeHistory", typeof(bool), typeof(NextMatchTileUserControl), new PropertyMetadata(true));

        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(string), typeof(NextMatchTileUserControl), new PropertyMetadata(null));

        public string Location
        {
            get { return (string)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Location.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(string), typeof(NextMatchTileUserControl), new PropertyMetadata(null));

        public string HomeTeamImage
        {
            get { return (string)GetValue(HomeTeamImageProperty); }
            set { SetValue(HomeTeamImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HomeTeamImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HomeTeamImageProperty =
            DependencyProperty.Register("HomeTeamImage", typeof(string), typeof(NextMatchTileUserControl), new PropertyMetadata(null));

        public string AwayTeamImage
        {
            get { return (string)GetValue(AwayTeamImageProperty); }
            set { SetValue(AwayTeamImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AwayTeamImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AwayTeamImageProperty =
            DependencyProperty.Register("AwayTeamImage", typeof(string), typeof(NextMatchTileUserControl), new PropertyMetadata(null));

        private void NextMatchTile_Loaded(object sender, RoutedEventArgs e)
        {
            if (IncludeHistory == false)
            {
                HistoryGrid.Visibility = Visibility.Collapsed;
                OpponentStrength.Visibility = Visibility.Collapsed;
            }
        }
    }
}
