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
    public sealed partial class EditWorkoutTileUserControl : UserControl
    {
        private bool flipped = false;
        SplitView rootPage = Shell.Current;

        public EditWorkoutTileUserControl()
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
            DependencyProperty.Register("CategoryColor", typeof(string), typeof(EditWorkoutTileUserControl), new PropertyMetadata(null));

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Category.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(EditWorkoutTileUserControl), new PropertyMetadata(null));

        public string WorkoutName
        {
            get { return (string)GetValue(WorkoutNameProperty); }
            set { SetValue(WorkoutNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WorkouName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WorkoutNameProperty =
            DependencyProperty.Register("WorkoutName", typeof(string), typeof(EditWorkoutTileUserControl), new PropertyMetadata(null));

        public string ExerciseName
        {
            get { return (string)GetValue(ExerciseNameProperty); }
            set { SetValue(ExerciseNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WorkouName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExerciseNameProperty =
            DependencyProperty.Register("ExerciseName", typeof(string), typeof(EditWorkoutTileUserControl), new PropertyMetadata(null));

        public string NumberOfWorkout
        {
            get { return (string)GetValue(NumberOfWorkoutProperty); }
            set { SetValue(NumberOfWorkoutProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberOfWorkoutProperty =
            DependencyProperty.Register("NumberOfWorkout", typeof(string), typeof(EditWorkoutTileUserControl), new PropertyMetadata(null));

        public string BackgroundImage
        {
            get { return (string)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.Register("BackgroundImage", typeof(string), typeof(EditWorkoutTileUserControl), new PropertyMetadata(null));

        private void grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!flipped)
                VisualStateManager.GoToState(this, "Flip", true);
            else
                VisualStateManager.GoToState(this, "FlipBack", true);

            flipped = !flipped;
        }

        private void EditTapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(EditWorkoutDetails), ((Grid)sender).DataContext);
            e.Handled = true;
        }

        private void ScheduleTapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(ChoosePlayerGroups));
            e.Handled = true;
        }
    }
}
