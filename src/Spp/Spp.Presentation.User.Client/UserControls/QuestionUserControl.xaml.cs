/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class QuestionUserControl : UserControl
    {
        public event RoutedEventHandler ResponseSelected;

        private string resetString = "";
        private SplitView rootPage = Shell.Current;

        public QuestionUserControl()
        {
            this.InitializeComponent();

        }

        public int QuestionId
        {
            get { return (int)GetValue(QuestionIdProperty); }
            set { SetValue(QuestionIdProperty, value); }
        }

        public static readonly DependencyProperty QuestionIdProperty =
            DependencyProperty.Register("QuestionId", typeof(int), typeof(QuestionUserControl), null);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(QuestionUserControl), null);

        public string LowRangeText
        {
            get { return (string)GetValue(LowRangeTextProperty); }
            set { SetValue(LowRangeTextProperty, value); }
        }

        public static readonly DependencyProperty LowRangeTextProperty =
            DependencyProperty.Register("LowRangeText", typeof(string), typeof(QuestionUserControl), null);

        public string MidRangeText
        {
            get { return (string)GetValue(MidRangeTextProperty); }
            set { SetValue(MidRangeTextProperty, value); }
        }

        public static readonly DependencyProperty MidRangeTextProperty =
            DependencyProperty.Register("MidRangeText", typeof(string), typeof(QuestionUserControl), null);

        public string HighRangeText
        {
            get { return (string)GetValue(HighRangeTextProperty); }
            set { SetValue(HighRangeTextProperty, value); }
        }

        public static readonly DependencyProperty HighRangeTextProperty =
            DependencyProperty.Register("HighRangeText", typeof(string), typeof(QuestionUserControl), null);

        public GridLength UserControlWidth
        {
            get { return (GridLength)GetValue(UserControlWidthProperty); }
            set { SetValue(UserControlWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SmallVersion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserControlWidthProperty =
            DependencyProperty.Register("UserControlWidth", typeof(GridLength), typeof(QuestionUserControl), new PropertyMetadata(1000));

        public bool CreateMode
        {
            get { return (bool)GetValue(EditModeProperty); }
            set { SetValue(EditModeProperty, value); }
        }

        public static readonly DependencyProperty EditModeProperty =
            DependencyProperty.Register("EditMode", typeof(bool), typeof(QuestionUserControl), new PropertyMetadata(false, OnEditModeChanged));

        private static void OnEditModeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Control thisControl = dependencyObject as Control;

            if ((bool)e.NewValue == false)
                VisualStateManager.GoToState(thisControl, "FormEntry", false);
            else
                VisualStateManager.GoToState(thisControl, "QuestionCreate", false);
        }

        private void RadioButton_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (CreateMode == false)
            {
                RadioButton clickedRadioButton = sender as RadioButton;
                Storyboard sb = (Storyboard)this.Resources["GradientOverlayTransform"];
                double multiplier = Double.Parse((string)clickedRadioButton.Content);
                start.To = new Point(-0.3 + (0.1 * (multiplier + 2)), 1);
                end.To = new Point(1 + (0.1 * (multiplier + 2)), 1);
                sb.Begin();

                if ((string)clickedRadioButton.Content == "1" || (string)clickedRadioButton.Content == "10")
                    resetString = resetString + clickedRadioButton.Content;
                else
                    resetString = "";

                if (resetString == "1101")
                    (rootPage.Content as Frame).Navigate(typeof(SignIn));

                if (this.ResponseSelected != null)
                    this.ResponseSelected(this, e);
            }
        }
    }
}
