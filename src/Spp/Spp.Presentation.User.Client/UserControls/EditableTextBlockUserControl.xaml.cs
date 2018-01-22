/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class EditableTextBlockUserControl : UserControl
    {
        public EditableTextBlockUserControl()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

        }

        public string EditableContent
        {
            get { return (string)GetValue(EditableContentProperty); }
            set { SetValue(EditableContentProperty, value); }
        }

        public static readonly DependencyProperty EditableContentProperty =
            DependencyProperty.Register("EditableContent", typeof(string), typeof(EditableTextBlockUserControl), new PropertyMetadata(null));


        public Style TextBlockStyle
        {
            get { return (Style)GetValue(TextBlockStyleProperty); }
            set { SetValue(TextBlockStyleProperty, value); }
        }

        public static readonly DependencyProperty TextBlockStyleProperty =
            DependencyProperty.Register("TextBlockStyle", typeof(Style), typeof(EditableTextBlockUserControl), new PropertyMetadata(null));



        public Style TextBoxStyle
        {
            get { return (Style)GetValue(TextBoxStyleProperty); }
            set { SetValue(TextBoxStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextBoxStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxStyleProperty =
            DependencyProperty.Register("TextBoxStyle", typeof(Style), typeof(EditableTextBlockUserControl), new PropertyMetadata(null));



        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEditing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(EditableTextBlockUserControl), new PropertyMetadata(null, new PropertyChangedCallback(OnLabelChanged)));

        private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EditableTextBlockUserControl etbuc = d as EditableTextBlockUserControl; //null checks omitted
            bool editing = Convert.ToBoolean(e.NewValue);
            etbuc.DisplayTxt.Visibility = (editing == true) ? Visibility.Collapsed : Visibility.Visible;
            etbuc.EditTxt.Visibility = (editing == false) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
