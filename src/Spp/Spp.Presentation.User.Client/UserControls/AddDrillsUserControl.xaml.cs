/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Models;
using MicrosoftSportsScience.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MicrosoftSportsScience.UserControls
{
    public sealed partial class AddDrillsUserControl : UserControl
    {
        public event EventHandler<AddDrillsUserControl> DrillSelected;
        public event EventHandler<AddDrillsUserControl> DrillUnSelected;

        private bool _selected = false;

        public AddDrillsUserControl()
        {
            this.InitializeComponent();
        }


        public string CategoryColor
        {
            get { return (string)GetValue(CategoryColorProperty); }
            set { SetValue(CategoryColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CategoryColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryColorProperty =
            DependencyProperty.Register("CategoryColor", typeof(string), typeof(AddDrillsUserControl), new PropertyMetadata(null));

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public string SubCategory
        {
            get { return (string)GetValue(SubCategoryProperty); }
            set { SetValue(SubCategoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Category.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(AddDrillsUserControl), new PropertyMetadata(null));

        public static readonly DependencyProperty SubCategoryProperty =
            DependencyProperty.Register("SubCategory", typeof(string), typeof(AddDrillsUserControl), new PropertyMetadata(null));

        public string ExerciseName
        {
            get { return (string)GetValue(ExerciseNameProperty); }
            set { SetValue(ExerciseNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WorkouName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExerciseNameProperty =
            DependencyProperty.Register("ExerciseName", typeof(string), typeof(AddDrillsUserControl), new PropertyMetadata(null));

        public string TrainingLoad
        {
            get { return (string)GetValue(TrainingLoadProperty); }
            set { SetValue(TrainingLoadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrainingLoadProperty =
            DependencyProperty.Register("TrainingLoad", typeof(string), typeof(AddDrillsUserControl), new PropertyMetadata(null));

        private void SelectExerciseTapped(object sender, TappedRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, !_selected ? "On" : "Off", true);

            _selected = !_selected;

            if(_selected) OnDrillSelected(this);
            else OnDrillUnSelected(this);
        }

        private void OnDrillSelected(AddDrillsUserControl e)
        {
            DrillSelected?.Invoke(this, e);
        }

        private void OnDrillUnSelected(AddDrillsUserControl e)
        {
            DrillUnSelected?.Invoke(this, e);
        }
    }
}
