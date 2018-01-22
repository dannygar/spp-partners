/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class LoadMeterUserControl : UserControl
    {
        public LoadMeterUserControl()
        {
            this.InitializeComponent();
        }



        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); DetermineColor(); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(LoadMeterUserControl), new PropertyMetadata(0));



        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); CalculateDimensions(); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(LoadMeterUserControl), new PropertyMetadata(0));



        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); CalculateDimensions(); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(LoadMeterUserControl), new PropertyMetadata(0));



        public int Target
        {
            get { return (int)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); CalculateDimensions(); DetermineColor(); }
        }

        // Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(int), typeof(LoadMeterUserControl), new PropertyMetadata(0));

        private void CalculateDimensions()
        {
            if (Target < Maximum && Target > Minimum)
            {
                var ratio = (double)Target / (double)Maximum;
                firstColumn.Width = new GridLength(ratio, GridUnitType.Star);
                thirdColumn.Width = new GridLength(1 - ratio, GridUnitType.Star);
            }

        }

        private void DetermineColor()
        {
            if (Value < Target && Value > 0.8 * Target)
            {
                progress.Foreground = new SolidColorBrush(Colors.YellowGreen);
            }
            else if (Value > Target)
            {
                progress.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                progress.Foreground = new SolidColorBrush(Colors.Green);
            }

        }

    }
}
