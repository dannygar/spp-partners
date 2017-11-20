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
    public sealed partial class SelectGroupUserControl : UserControl
    {
        public SelectGroupUserControl()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }

        public string TileBGImage
        {
            get { return (string)GetValue(TileBGImageProperty); }
            set { SetValue(TileBGImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TileBGImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TileBGImageProperty =
            DependencyProperty.Register("TileBGImage", typeof(string), typeof(SelectGroupUserControl), new PropertyMetadata(null));



        public string TopperImage
        {
            get { return (string)GetValue(TopperImageProperty); }
            set { SetValue(TopperImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopperImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopperImageProperty =
            DependencyProperty.Register("TopperImage", typeof(string), typeof(SelectGroupUserControl), new PropertyMetadata(null));

        public string PlayerNumber
        {
            get { return (string)GetValue(PlayerNumberProperty); }
            set { SetValue(PlayerNumberProperty, value); }
        }

        public static readonly DependencyProperty PlayerNumberProperty =
            DependencyProperty.Register("PlayerNumber", typeof(string), typeof(SelectGroupUserControl), new PropertyMetadata(null));

        public string PlayerName
        {
            get { return (string)GetValue(PlayerNameProperty); }
            set { SetValue(PlayerNameProperty, value); }
        }

        public static readonly DependencyProperty PlayerNameProperty =
            DependencyProperty.Register("PlayerName", typeof(string), typeof(SelectGroupUserControl), new PropertyMetadata(null));

        public string PlayerPosition
        {
            get { return (string)GetValue(PlayerPositionProperty); }
            set { SetValue(PlayerPositionProperty, value); }
        }

        public static readonly DependencyProperty PlayerPositionProperty =
            DependencyProperty.Register("PlayerPosition", typeof(string), typeof(SelectGroupUserControl), new PropertyMetadata(null));
    }
}
