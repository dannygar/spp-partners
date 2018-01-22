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
    public sealed partial class SelectPracticeTileUserControl : UserControl
    {
        private bool selected = false;

        public SelectPracticeTileUserControl()
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
            DependencyProperty.Register("CategoryColor", typeof(string), typeof(SelectPracticeTileUserControl), new PropertyMetadata(null));

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Category.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(SelectPracticeTileUserControl), new PropertyMetadata(null));

        public string ExerciseName
        {
            get { return (string)GetValue(ExerciseNameProperty); }
            set { SetValue(ExerciseNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WorkouName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExerciseNameProperty =
            DependencyProperty.Register("ExerciseName", typeof(string), typeof(SelectPracticeTileUserControl), new PropertyMetadata(null));

        public string TrainingLoad
        {
            get { return (string)GetValue(TrainingLoadProperty); }
            set { SetValue(TrainingLoadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrainingLoadProperty =
            DependencyProperty.Register("TrainingLoad", typeof(string), typeof(SelectPracticeTileUserControl), new PropertyMetadata(null));

        public string BackgroundImage
        {
            get { return (string)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.Register("BackgroundImage", typeof(string), typeof(SelectPracticeTileUserControl), new PropertyMetadata(null));



        private void SelectExerciseTapped(object sender, TappedRoutedEventArgs e)
        {
            if (!selected)
                VisualStateManager.GoToState(this, "On", true);
            else
                VisualStateManager.GoToState(this, "Off", true);

            selected = !selected;
        }
    }
}
