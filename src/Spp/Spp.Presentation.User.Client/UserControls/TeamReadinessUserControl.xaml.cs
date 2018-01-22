/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class TeamReadinessUserControl : UserControl
    {
        SplitView rootPage = Shell.Current;

        public TeamReadinessUserControl()
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
            DependencyProperty.Register("IncludeHistory", typeof(bool), typeof(TeamReadinessUserControl), new PropertyMetadata(true));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(TeamReadinessUserControl), new PropertyMetadata(null));



        public string ReadinessPercentage
        {
            get { return (string)GetValue(ReadinessPercentageProperty); }
            set { SetValue(ReadinessPercentageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReadinessPercentage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReadinessPercentageProperty =
            DependencyProperty.Register("ReadinessPercentage", typeof(string), typeof(TeamReadinessUserControl), new PropertyMetadata(null));



        private void TeamReadiness_Loaded(object sender, RoutedEventArgs e)
        {
            if (IncludeHistory == false)
            {
                HistoryGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}
