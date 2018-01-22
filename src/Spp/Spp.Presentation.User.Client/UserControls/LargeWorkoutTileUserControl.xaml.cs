/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class LargeWorkoutTileUserControl : UserControl
    {
        private string TempNotes;

        public LargeWorkoutTileUserControl()
        {
            this.InitializeComponent();
        }

        private void LeftButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            if (RightButtonText.Text == "Save Notes")
            {
                NotesField.Text = TempNotes;
            }
        }

        private void RightButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            if (RightButtonText.Text == "Remove Drill")
            {
                DrillHeight.Value = grid4.ActualHeight;
                VisualStateManager.GoToState(this, "RemoveDrillForGood", true);
            }
            else if (RightButtonText.Text == "Save Notes")
            {
                VisualStateManager.GoToState(this, "Idle", true);
            }
        }

        private void RemoveDrillTapped(object sender, TappedRoutedEventArgs e)
        {
            RightButtonText.Text = "Remove Drill";
        }

        private void AddNotesTapped(object sender, TappedRoutedEventArgs e)
        {
            TempNotes = NotesField.Text;
            RightButtonText.Text = "Save Notes";
        }

        public string ExerciseTitle
        {
            get { return (string)GetValue(ExerciseTitleProperty); }
            set { SetValue(ExerciseTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExerciseTitleProperty =
            DependencyProperty.Register("ExerciseTitle", typeof(string), typeof(LargeWorkoutTileUserControl), null);

        public int ExerciseId
        {
            get { return (int)GetValue(ExerciseIdProperty); }
            set { SetValue(ExerciseIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExerciseIdProperty =
            DependencyProperty.Register("ExerciseId", typeof(int), typeof(LargeWorkoutTileUserControl), null);

        public string ExerciseDescription
        {
            get { return (string)GetValue(ExerciseDescriptionProperty); }
            set { SetValue(ExerciseDescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExerciseDescriptionProperty =
            DependencyProperty.Register("ExerciseDescription", typeof(string), typeof(LargeWorkoutTileUserControl), null);

        //public string ExerciseImage
        //{
        //    get { return (string)GetValue(ExerciseImageProperty); }
        //    set { SetValue(ExerciseImageProperty, value); }
        //}

        //public static readonly DependencyProperty ExerciseImageProperty =
        //    DependencyProperty.Register("ExerciseImage", typeof(string), typeof(LargeWorkoutTileUserControl), null);

        //public string TrainingLoad
        //{
        //    get { return (string)GetValue(TrainingLoadProperty); }
        //    set { SetValue(TrainingLoadProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty TrainingLoadProperty =
        //    DependencyProperty.Register("TrainingLoad", typeof(string), typeof(LargeWorkoutTileUserControl), null);

        //public string NumberOfSets
        //{
        //    get { return (string)GetValue(NumberOfSetsProperty); }
        //    set { SetValue(NumberOfSetsProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty NumberOfSetsProperty =
        //    DependencyProperty.Register("NumberOfSets", typeof(string), typeof(LargeWorkoutTileUserControl), null);

        public String TrainingLoad
        {
            get { return (string)GetValue(TrainingLoadProperty); }
            set { SetValue(TrainingLoadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrainingLoad.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrainingLoadProperty =
            DependencyProperty.Register("TrainingLoad", typeof(string), typeof(LargeWorkoutTileUserControl), new PropertyMetadata(null));

        public ObservableCollection<AthleteExercise> Sets
        {
            get { return (ObservableCollection<AthleteExercise>)GetValue(SetsProperty); }
            set { SetValue(SetsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SetsProperty =
            DependencyProperty.Register("SetsProperty", typeof(ObservableCollection<AthleteExercise>), typeof(LargeWorkoutTileUserControl), new PropertyMetadata(null));

        public string Duration
        {
            get { return (string)GetValue(RecoveryTimeProperty); }
            set { SetValue(RecoveryTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecoveryTimeProperty =
            DependencyProperty.Register("Duration", typeof(string), typeof(LargeWorkoutTileUserControl), null);

        public string Notes
        {
            get { return (string)GetValue(NotesProperty); }
            set { SetValue(NotesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotesProperty =
            DependencyProperty.Register("Notes", typeof(string), typeof(LargeWorkoutTileUserControl), null);

        //private void image_ImageOpened(object sender, RoutedEventArgs e)
        //{
        //    Storyboard sb = this.Resources["ImageVisible"] as Storyboard;
        //    sb.Begin();
        //}

        private void RightButtonText_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void DeleteSetTapped(object sender, TappedRoutedEventArgs e)
        {
            var button = sender as Image;
            if (button != null)
            {
                var task = button.DataContext as AthleteExercise;

                ((ObservableCollection<AthleteExercise>)ExerciseSets.ItemsSource).Remove(task);
            }
            else
            {
                return;
            }
        }

        private void EditSetTapped(object sender, TappedRoutedEventArgs e)
        {
            Grid parentGrid = HelperMethods.FindParent<Grid>(sender as DependencyObject);
            IEnumerable<TextBlock> textBlocks = parentGrid.Children.OfType<TextBlock>();
            IEnumerable<TextBox> textBoxes = parentGrid.Children.OfType<TextBox>();
            IEnumerable<StackPanel> stackPanels = parentGrid.Children.OfType<StackPanel>();
            IEnumerable<Image> images = parentGrid.Children.OfType<Image>();

            foreach (TextBlock textBlock in textBlocks)
            {
                textBlock.Visibility = Visibility.Collapsed;
            }

            foreach (TextBox textBox in textBoxes)
            {
                textBox.Visibility = Visibility.Visible;
            }

            foreach (StackPanel stackPanel in stackPanels)
            {
                stackPanel.Visibility = Visibility.Collapsed;
            }

            foreach (Image image in images)
            {
                image.Visibility = Visibility.Visible;
            }
        }

        private void SaveSetTapped(object sender, TappedRoutedEventArgs e)
        {
            Grid parentGrid = HelperMethods.FindParent<Grid>(sender as DependencyObject);
            IEnumerable<TextBlock> textBlocks = parentGrid.Children.OfType<TextBlock>();
            IEnumerable<TextBox> textBoxes = parentGrid.Children.OfType<TextBox>();
            IEnumerable<StackPanel> stackPanels = parentGrid.Children.OfType<StackPanel>();
            IEnumerable<Image> images = parentGrid.Children.OfType<Image>();

            for (int i = 0; i < textBlocks.Count<TextBlock>(); i++)
            {
                textBlocks.ElementAt<TextBlock>(i).Visibility = Visibility.Visible;
                textBlocks.ElementAt<TextBlock>(i).Text = textBoxes.ElementAt<TextBox>(i).Text;
            }

            foreach (TextBox textBox in textBoxes)
            {
                textBox.Visibility = Visibility.Collapsed;
            }

            foreach (StackPanel stackPanel in stackPanels)
            {
                stackPanel.Visibility = Visibility.Visible;
            }

            foreach (Image image in images)
            {
                image.Visibility = Visibility.Collapsed;
            }
        }
    }
}
