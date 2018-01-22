/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class PerformanceSnapshotUserControl : UserControl
    {
        SplitView rootPage = Shell.Current;

        public PerformanceSnapshotUserControl()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Opened", true);
        }



        public string TopString
        {
            get { return (string)GetValue(TopStringProperty); }
            set { SetValue(TopStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopStringProperty =
            DependencyProperty.Register("TopString", typeof(string), typeof(PerformanceSnapshotUserControl), new PropertyMetadata(null));

        public string TopLeftString
        {
            get { return (string)GetValue(TopLeftStringProperty); }
            set { SetValue(TopLeftStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopLeftString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopLeftStringProperty =
            DependencyProperty.Register("TopLeftString", typeof(string), typeof(PerformanceSnapshotUserControl), new PropertyMetadata(null));

        public string TopRightString
        {
            get { return (string)GetValue(TopRightStringProperty); }
            set { SetValue(TopRightStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopRightString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopRightStringProperty =
            DependencyProperty.Register("TopRightString", typeof(string), typeof(PerformanceSnapshotUserControl), new PropertyMetadata(null));

        public string BottomLeftString
        {
            get { return (string)GetValue(BottomLeftStringProperty); }
            set { SetValue(BottomLeftStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomLeftString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomLeftStringProperty =
            DependencyProperty.Register("BottomLeftString", typeof(string), typeof(PerformanceSnapshotUserControl), new PropertyMetadata(null));

        public string BottomRightString
        {
            get { return (string)GetValue(BottomRightStringProperty); }
            set { SetValue(BottomRightStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomRightString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomRightStringProperty =
            DependencyProperty.Register("BottomRightString", typeof(string), typeof(PerformanceSnapshotUserControl), new PropertyMetadata(null));

        public string BottomString
        {
            get { return (string)GetValue(BottomStringProperty); }
            set { SetValue(BottomStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomStringProperty =
            DependencyProperty.Register("BottomString", typeof(string), typeof(PerformanceSnapshotUserControl), new PropertyMetadata(null));

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
