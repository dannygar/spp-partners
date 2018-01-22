/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
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
