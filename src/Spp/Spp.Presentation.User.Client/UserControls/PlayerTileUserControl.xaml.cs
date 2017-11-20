/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace MicrosoftSportsScience.UserControls
{
    public sealed partial class PlayerTileUserControl : UserControl
    {
        public PlayerTileUserControl()
        {
            this.InitializeComponent();
        }

        public Visibility ClubNameVisibility
        {
            get { return (Visibility)this.GetValue(ClubNameVisibilityProperty); }
            set { this.SetValue(ClubNameVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ClubNameVisibilityProperty =
            DependencyProperty.Register(nameof(ClubNameVisibility), typeof(Visibility), typeof(PlayerTileUserControl), null);

        public Visibility NameAreaVisibility
        {
            get { return (Visibility)this.GetValue(NameAreaVisibilityProperty); }
            set { this.SetValue(NameAreaVisibilityProperty, value); }
        }

        
        public static readonly DependencyProperty NameAreaVisibilityProperty =
            DependencyProperty.Register(nameof(NameAreaVisibility), typeof(Visibility), typeof(PlayerTileUserControl), new PropertyMetadata(Visibility.Visible));



        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PlayerTileUserControl), new PropertyMetadata(false));

        
        public bool DisplayCompletenessInfo
        {
            get { return (bool)GetValue(DisplayCompletenessInfoProperty); }
            set { SetValue(DisplayCompletenessInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayCompletenessInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayCompletenessInfoProperty =
            DependencyProperty.Register("DisplayCompletenessInfo", typeof(bool), typeof(PlayerTileUserControl), new PropertyMetadata(false));



        /// <summary>
        /// Gets or sets the resting icon visibility. The control will display the resting icon if the player is resting.
        /// </summary>
        public Visibility RestingIconVisibility
        {
            get { return (Visibility)this.GetValue(RestingIconVisibilityProperty); }
            set { this.SetValue(RestingIconVisibilityProperty, value); }
        }

        public static readonly DependencyProperty RestingIconVisibilityProperty =
            DependencyProperty.Register(nameof(RestingIconVisibility), typeof(Visibility), typeof(PlayerTileUserControl), null);

        public Visibility SkillIconVisibility
        {
            get { return (Visibility)this.GetValue(SkillIconVisibilityProperty); }
            set { this.SetValue(SkillIconVisibilityProperty, value); }
        }

        public static readonly DependencyProperty SkillIconVisibilityProperty =
            DependencyProperty.Register(nameof(SkillIconVisibility), typeof(Visibility), typeof(PlayerTileUserControl), new PropertyMetadata(Visibility.Visible));


        public Brush TeamColor
        {
            get { return (Brush)this.GetValue(TeamColorProperty); }
            set { this.SetValue(TeamColorProperty, value); }
        }

        public static readonly DependencyProperty TeamColorProperty =
            DependencyProperty.Register(nameof(TeamColor), typeof(Brush), typeof(PlayerTileUserControl), null);

        public double TileWidth
        {
            get { return (double)GetValue(TileWidthProperty); }
            set { SetValue(TileWidthProperty, value); }
        }
        
        public static readonly DependencyProperty TileWidthProperty =
            DependencyProperty.Register("TileWidth", typeof(double), typeof(PlayerTileUserControl), null);

        
        public User User
        {
            get { return (User)GetValue(PlayerProperty); }
            set { SetValue(PlayerProperty, value); }
        }

        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register("User", typeof(User), typeof(PlayerTileUserControl), null);

        public string PlayerProfileImage
        {
            get { return (string)GetValue(PlayerProfileImageProperty); }
            set { SetValue(PlayerProfileImageProperty, value); }
        }
        
        public static readonly DependencyProperty PlayerProfileImageProperty =
            DependencyProperty.Register("PlayerProfileImage", typeof(string), typeof(PlayerTileUserControl), null);

        public string PlayerFirstName
        {
            get { return (string)GetValue(PlayerFirstNameProperty); }
            set { SetValue(PlayerFirstNameProperty, value); }
        }

        public static readonly DependencyProperty PlayerFirstNameProperty =
            DependencyProperty.Register("PlayerFirstName", typeof(string), typeof(PlayerTileUserControl), null);

        public string PlayerLastName
        {
            get { return (string)GetValue(PlayerLastNameProperty); }
            set { SetValue(PlayerLastNameProperty, value); }
        }

        public static readonly DependencyProperty PlayerLastNameProperty =
            DependencyProperty.Register("PlayerLastName", typeof(string), typeof(PlayerTileUserControl), null);
        
        public bool Completed
        {
            get { return (bool)GetValue(CompletedProperty); }
            set { SetValue(CompletedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Completed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompletedProperty =
            DependencyProperty.Register("Completed", typeof(bool), typeof(PlayerTileUserControl), new PropertyMetadata(false));



        public event RoutedEventHandler Click;

        private void OnPlayerTileClick(object sender, RoutedEventArgs e)
        {
            if (this.Click != null)
                this.Click(this, e);
        }
    }
}
