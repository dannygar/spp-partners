/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class PlayersAssignedUserControl : UserControl
    {
        public PlayersAssignedUserControl()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PlayersAssignedUserControl), new PropertyMetadata(null));



        public string InitialLetter
        {
            get { return (string)GetValue(InitialLetterProperty); }
            set { SetValue(InitialLetterProperty, value); }
        }

        public static readonly DependencyProperty InitialLetterProperty =
            DependencyProperty.Register("InitialLetter", typeof(string), typeof(PlayersAssignedUserControl), new PropertyMetadata(null));


    }
}
