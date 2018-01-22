/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{

    public sealed partial class DrillTileUserControl : UserControl
    {
        private string TempNotes;

        public event EventHandler<int> DrillUpdated;
        public event EventHandler<DrillTileUserControl> DrillRemoved;

        public DrillTileUserControl()
        {
            this.InitializeComponent();
            DrillImage = " ";
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
                OnDrillRemoved(this);
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

        public string DrillTitle
        {
            get { return (string)GetValue(DrillTitleProperty); }
            set { SetValue(DrillTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DrillTitleProperty =
            DependencyProperty.Register("DrillTitle", typeof(string), typeof(DrillTileUserControl), null);

        public int DrillId
        {
            get { return (int)GetValue(DrillIdProperty); }
            set { SetValue(DrillIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DrillIdProperty =
            DependencyProperty.Register("DrillId", typeof(int), typeof(DrillTileUserControl), null);

        public string DrillDescription
        {
            get { return (string)GetValue(DrillDescriptionProperty); }
            set { SetValue(DrillDescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DrillDescriptionProperty =
            DependencyProperty.Register("DrillDescription", typeof(string), typeof(DrillTileUserControl), null);

        public string DrillImage
        {
            get { return (string)GetValue(DrillImageProperty); }
            set { SetValue(DrillImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DrillImageProperty =
            DependencyProperty.Register("DrillImage", typeof(string), typeof(DrillTileUserControl), null);

        public string TrainingLoad
        {
            get { return (string)GetValue(TrainingLoadProperty); }
            set
            {
                SetValue(TrainingLoadProperty, value);
                OnDrillUpdated(int.Parse(value));
            }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrainingLoadProperty =
            DependencyProperty.Register("TrainingLoad", typeof(string), typeof(DrillTileUserControl), null);

        public string NumberOfPlayers
        {
            get { return (string)GetValue(NumberOfPlayersProperty); }
            set { SetValue(NumberOfPlayersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberOfPlayersProperty =
            DependencyProperty.Register("NumberOfPlayers", typeof(string), typeof(DrillTileUserControl), null);

        public string Size
        {
            get { return (string)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(string), typeof(DrillTileUserControl), null);

        public string Duration
        {
            get { return (string)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(string), typeof(DrillTileUserControl), null);

        public string Notes
        {
            get { return (string)GetValue(NotesProperty); }
            set { SetValue(NotesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrillDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotesProperty =
            DependencyProperty.Register("Notes", typeof(string), typeof(DrillTileUserControl), null);

        private void image_ImageOpened(object sender, RoutedEventArgs e)
        {
            Storyboard sb = this.Resources["ImageVisible"] as Storyboard;
            sb.Begin();
        }

        private async void RightButtonText_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await SimpleIoc.Default.GetInstance<AthletePracticeModel>().SetCoachNote(this.DrillId, this.Notes);
        }

        private void OnDrillUpdated(int e)
        {
            DrillUpdated?.Invoke(this, e);
        }

        private void OnDrillRemoved(DrillTileUserControl e)
        {
            DrillRemoved?.Invoke(this, e);
        }
    }
}
