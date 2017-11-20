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
    public sealed class ModalDialogEntries
    {
        public string Entry1 { get; set; }
        public string Entry2 { get; set; }
        public string Entry3 { get; set; }
    }

    public sealed partial class TopicSubTopicUserControl : UserControl
    {
        SplitView rootPage = Shell.Current;

        public TopicSubTopicUserControl()
        {
            this.InitializeComponent();
        }


        public ModalDialogEntries UserEntries { get; set; } = new ModalDialogEntries();

        public int stages
        {
            get { return (int)GetValue(stagesProperty); }
            set { SetValue(stagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for stages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty stagesProperty =
            DependencyProperty.Register("stages", typeof(int), typeof(TopicSubTopicUserControl), new PropertyMetadata(null));



        public string Destination
        {
            get { return (string)GetValue(DestinationProperty); }
            set { SetValue(DestinationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Destination.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DestinationProperty =
            DependencyProperty.Register("Destination", typeof(string), typeof(TopicSubTopicUserControl), new PropertyMetadata(null));



        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(TopicSubTopicUserControl), new PropertyMetadata(null));
        
        public string PlaceHolderText
        {
            get { return (string)GetValue(PlaceHolderTextProperty); }
            set { SetValue(PlaceHolderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderTextProperty =
            DependencyProperty.Register("PlaceHolderText", typeof(string), typeof(TopicSubTopicUserControl), new PropertyMetadata(null));

        public string HeaderText2
        {
            get { return (string)GetValue(HeaderText2Property); }
            set { SetValue(HeaderText2Property, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderText2Property =
            DependencyProperty.Register("HeaderText2", typeof(string), typeof(TopicSubTopicUserControl), new PropertyMetadata(null));


        public string HeaderText3
        {
            get { return (string)GetValue(HeaderText3Property); }
            set { SetValue(HeaderText3Property, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderText3Property =
            DependencyProperty.Register("HeaderText3", typeof(string), typeof(TopicSubTopicUserControl), new PropertyMetadata(null));

        public string PlaceHolderText2
        {
            get { return (string)GetValue(PlaceHolderText2Property); }
            set { SetValue(PlaceHolderText2Property, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderText2Property =
            DependencyProperty.Register("PlaceHolderText2", typeof(string), typeof(TopicSubTopicUserControl), new PropertyMetadata(null));


        public string PlaceHolderText3
        {
            get { return (string)GetValue(PlaceHolderText3Property); }
            set { SetValue(PlaceHolderText3Property, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderText3Property =
            DependencyProperty.Register("PlaceHolderText3", typeof(string), typeof(TopicSubTopicUserControl), new PropertyMetadata(null));

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            switch (stages)
            {
                case 3:
                    UserEntries.Entry1 = TextEntry.Text;
                    stages -= 1;
                    Header.Text = HeaderText3;
                    TextEntry.PlaceholderText = PlaceHolderText2;
                    TextEntry.Text = "";
                    break;
                case 2:
                    UserEntries.Entry2 = TextEntry.Text;
                    stages -= 1;
                    Header.Text = HeaderText2 ?? "";
                    TextEntry.PlaceholderText = PlaceHolderText3 ?? "";
                    TextEntry.Text = "";
                    break;
                default:
                    UserEntries.Entry3 = TextEntry.Text;
                    CloseModal();
                    Type pageType = Type.GetType(Destination);
                    (rootPage.Content as Frame).Navigate(pageType, UserEntries);
                    break;
            }
        }

        public void OpenModal()
        {
            VisualStateManager.GoToState(this, "Open", true);
        }

        public void CloseModal()
        {
            VisualStateManager.GoToState(this, "Closed", true);
        }
    }
}
