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
    public sealed partial class EditExerciseListItem : UserControl
    {
        public EditExerciseListItem()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }

        public string ExerciseName
        {
            get { return (string)GetValue(ExerciseNameProperty); }
            set { SetValue(ExerciseNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ExerciseName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExerciseNameProperty =
            DependencyProperty.Register("ExerciseName", typeof(string), typeof(EditExerciseListItem), new PropertyMetadata(string.Empty));



        public string ExerciseReps
        {
            get { return (string)GetValue(ExerciseRepsProperty); }
            set { SetValue(ExerciseRepsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ExerciseReps.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExerciseRepsProperty =
            DependencyProperty.Register("ExerciseReps", typeof(string), typeof(EditExerciseListItem), new PropertyMetadata(string.Empty));




    }
}
