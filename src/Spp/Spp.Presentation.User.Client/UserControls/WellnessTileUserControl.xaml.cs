/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Data;
using System;
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

namespace MicrosoftSportsScience.UserControls
{
    public sealed partial class WellnessTileUserControl : UserControl
    {
        public WellnessTileUserControl()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public string WellnessScore
        {
            get { return (string)GetValue(WellnessScoreProperty); }
            set { SetValue(WellnessScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WellnessScore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WellnessScoreProperty =
            DependencyProperty.Register("WellnessScore", typeof(string), typeof(WellnessTileUserControl), null);


    }
}
