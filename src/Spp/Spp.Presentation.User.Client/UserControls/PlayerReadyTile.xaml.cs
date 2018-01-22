/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class PlayerReadyTile : UserControl
    {
        SplitView rootPage = Shell.Current;

        public PlayerReadyTile()
        {
            this.InitializeComponent();
        }

        public string HomeImage
        {
            get { return (string)GetValue(HomeImageProperty); }
            set { SetValue(HomeImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HomeImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HomeImageProperty =
            DependencyProperty.Register("HomeImage", typeof(string), typeof(PlayerReadyTile), new PropertyMetadata(null));

        public string AwayImage
        {
            get { return (string)GetValue(AwayImageProperty); }
            set { SetValue(AwayImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AwayImageProperty =
            DependencyProperty.Register("AwayImage", typeof(string), typeof(PlayerReadyTile), new PropertyMetadata(null));

        public string Location
        {
            get { return (string)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Location.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(string), typeof(PlayerReadyTile), new PropertyMetadata(null));

        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(string), typeof(PlayerReadyTile), new PropertyMetadata(null));

    }
}
