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
    public sealed partial class CalendarTileUserControl : UserControl
    {
        public CalendarTileUserControl()
        {
            this.InitializeComponent();
        }

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Category.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(CalendarTileUserControl), new PropertyMetadata(null));

        public string CategoryColor
        {
            get { return (string)GetValue(CategoryColorProperty); }
            set { SetValue(CategoryColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CategoryColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryColorProperty =
            DependencyProperty.Register("CategoryColor", typeof(string), typeof(CalendarTileUserControl), new PropertyMetadata(null));

        public string TileName
        {
            get { return (string)GetValue(TileNameProperty); }
            set { SetValue(TileNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TileName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TileNameProperty =
            DependencyProperty.Register("TileName", typeof(string), typeof(CalendarTileUserControl), new PropertyMetadata(null));

        private void UserControl_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            sender.Opacity = .75;
        }

        private void UserControl_DropCompleted(UIElement sender, DropCompletedEventArgs args)
        {
            sender.Opacity = 1;
        }
    }
}
