/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MicrosoftSportsScience.UserControls
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
