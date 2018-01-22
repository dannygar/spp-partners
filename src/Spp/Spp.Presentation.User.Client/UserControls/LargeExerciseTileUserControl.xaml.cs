/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Spp.Presentation.User.Client.Helpers;
using Spp.Presentation.User.Client.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class LargeExerciseTileUserControl : UserControl
    {
        private string TempNotes;
        public event EventHandler<int> ExerciseUpdated;
        public event EventHandler<LargeExerciseTileUserControl> ExerciseRemoved;

        public LargeExerciseTileUserControl()
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
                OnExerciseRemoved(this);
            }
            else if (RightButtonText.Text == "Save Notes")
            {
                VisualStateManager.GoToState(this, "Idle", true);
            }
        }

        private void RemoveDrillTapped(object sender, TappedRoutedEventArgs e)
        {
            RightButtonText.Text = "Remove Exercise";
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
            DependencyProperty.Register("ExerciseTitle", typeof(string), typeof(LargeExerciseTileUserControl), null);

        public int ExerciseId
        {
            get { return (int)GetValue(ExerciseIdProperty); }
            set { SetValue(ExerciseIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExerciseIdProperty =
            DependencyProperty.Register("ExerciseId", typeof(int), typeof(LargeExerciseTileUserControl), null);

        public string ExerciseDescription
        {
            get { return (string)GetValue(ExerciseDescriptionProperty); }
            set { SetValue(ExerciseDescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExerciseDescriptionProperty =
            DependencyProperty.Register("ExerciseDescription", typeof(string), typeof(LargeExerciseTileUserControl), null);

        public string ExerciseImage
        {
            get { return (string)GetValue(ExerciseImageProperty); }
            set { SetValue(ExerciseImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExerciseImageProperty =
            DependencyProperty.Register("ExerciseImage", typeof(string), typeof(LargeExerciseTileUserControl), null);

        public string TrainingLoad
        {
            get { return (string)GetValue(TrainingLoadProperty); }
            set
            {
                SetValue(TrainingLoadProperty, value);
                OnExerciseUpdated((!string.IsNullOrEmpty(value)) ? int.Parse(value) : 0);
            }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrainingLoadProperty =
            DependencyProperty.Register("TrainingLoad", typeof(string), typeof(LargeExerciseTileUserControl), null);

        public string NumberOfSets
        {
            get { return (string)GetValue(NumberOfSetsProperty); }
            set { SetValue(NumberOfSetsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberOfSetsProperty =
            DependencyProperty.Register("NumberOfSets", typeof(string), typeof(LargeExerciseTileUserControl), null);


        public ObservableCollection<AthleteExerciseSetViewModel> Sets
        {
            get { return (ObservableCollection<AthleteExerciseSetViewModel>)GetValue(SetsProperty); }
            set { SetValue(SetsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SetsProperty =
            DependencyProperty.Register("SetsProperty", typeof(ObservableCollection<AthleteExerciseSetViewModel>), typeof(LargeExerciseTileUserControl), new PropertyMetadata(null));

        public int Duration
        {
            get { return (int)GetValue(RecoveryTimeProperty); }
            set { SetValue(RecoveryTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecoveryTimeProperty =
            DependencyProperty.Register("Duration", typeof(string), typeof(LargeExerciseTileUserControl), null);

        public string Notes
        {
            get { return (string)GetValue(NotesProperty); }
            set { SetValue(NotesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotesProperty =
            DependencyProperty.Register("Notes", typeof(string), typeof(LargeExerciseTileUserControl), null);

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            Storyboard sb = this.Resources["ImageVisible"] as Storyboard;
            sb.Begin();
        }

        private void RightButtonText_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void DeleteSetTapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Image button)
            {
                var task = button.DataContext as AthleteExerciseSetViewModel;

                ((ObservableCollection<AthleteExerciseSetViewModel>)ExerciseSets.ItemsSource).Remove(task);
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

        private void EditTileTapped(object sender, TappedRoutedEventArgs e)
        {
            EditTileImage.Visibility = Visibility.Collapsed;
            SaveTileImage.Visibility = Visibility.Visible;
            ExerciseDescriptionBlock.Visibility = Visibility.Collapsed;
            ExerciseDescriptionBox.Visibility = Visibility.Visible;
            ExerciseTitleBlock.Visibility = Visibility.Collapsed;
            ExerciseTitleBox.Visibility = Visibility.Visible;
            NumOfSetsBlock.Visibility = Visibility.Collapsed;
            NumOfSetsBox.Visibility = Visibility.Visible;
            TrainingLoadBlock.Visibility = Visibility.Collapsed;
            TrainingLoadBox.Visibility = Visibility.Visible;
            RecoveryTimeBlock.Visibility = Visibility.Collapsed;
            RecoveryTimeBox.Visibility = Visibility.Visible;
        }

        private void SaveTileTapped(object sender, TappedRoutedEventArgs e)
        {
            EditTileImage.Visibility = Visibility.Visible;
            SaveTileImage.Visibility = Visibility.Collapsed;

            ExerciseDescriptionBlock.Visibility = Visibility.Visible;
            ExerciseDescriptionBox.Visibility = Visibility.Collapsed;
            ExerciseDescriptionBlock.Text = ExerciseDescriptionBox.Text;

            ExerciseTitleBlock.Visibility = Visibility.Visible;
            ExerciseTitleBox.Visibility = Visibility.Collapsed;
            ExerciseTitleBlock.Text = ExerciseTitleBox.Text;

            NumOfSetsBlock.Visibility = Visibility.Visible;
            NumOfSetsBox.Visibility = Visibility.Collapsed;
            NumOfSetsBlock.Text = NumOfSetsBox.Text;

            TrainingLoadBlock.Visibility = Visibility.Visible;
            TrainingLoadBox.Visibility = Visibility.Collapsed;
            TrainingLoadBlock.Text = TrainingLoadBox.Text;

            RecoveryTimeBlock.Visibility = Visibility.Visible;
            RecoveryTimeBox.Visibility = Visibility.Collapsed;
            RecoveryTimeBlock.Text = RecoveryTimeBox.Text;
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
                if (textBox.Name == "Reps")
                {
                    try
                    {
                        Sets[0].Reps =
                            !(string.IsNullOrEmpty(textBox.Text) && int.TryParse(textBox.Text, out int iResult))
                                ? int.Parse(textBox.Text)
                                : 0;
                    }
                    catch
                    {
                        this.Sets[0].Reps = 0;
                    }
                    finally
                    {
                        this.TrainingLoad =
                            AthleteExerciseSetViewModel.CalculateTrainingLoad(this.Sets[0].Reps, this.Sets[0].Weight)
                                .ToString();
                    }
                }
                if (textBox.Name == "Weight")
                {
                    try
                    {
                        Sets[0].Weight =
                            !(string.IsNullOrEmpty(textBox.Text) && float.TryParse(textBox.Text, out float dResult))
                                ? float.Parse(textBox.Text)
                                : 0;
                    }
                    catch
                    {
                        this.Sets[0].Weight = 0.0F;
                    }
                    finally
                    {
                        this.TrainingLoad =
                            AthleteExerciseSetViewModel.CalculateTrainingLoad(this.Sets[0].Reps, this.Sets[0].Weight)
                                .ToString();
                    }
                }
            }


            foreach (StackPanel stackPanel in stackPanels)
            {
                stackPanel.Visibility = Visibility.Visible;
            }

            foreach (Image image in images)
            {
                image.Visibility = Visibility.Collapsed;
            }


            OnExerciseUpdated(!string.IsNullOrEmpty(this.TrainingLoad) ? int.Parse(this.TrainingLoad) : 0);
        }

        private void OnExerciseUpdated(int e)
        {
            ExerciseUpdated?.Invoke(this, e);
        }

        private void OnExerciseRemoved(LargeExerciseTileUserControl e)
        {
            ExerciseRemoved?.Invoke(this, e);
        }
    }
}
