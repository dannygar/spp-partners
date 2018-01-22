/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class EmotionMeterControl : UserControl
    {
        public EmotionMeterControl()
        {
            this.InitializeComponent();
            //this.DataContext = this;
        }

        public static readonly DependencyProperty EmotionNameProperty =
            DependencyProperty.Register(
            "EmotionName",
            typeof(string),
            typeof(EmotionMeterControl),
            new PropertyMetadata("")
            );

        public static readonly DependencyProperty EmotionValueProperty =
            DependencyProperty.Register(
            "EmotionValue",
            typeof(float),
            typeof(EmotionMeterControl),
            new PropertyMetadata(0)
            );

        public static readonly DependencyProperty MeterForegroundProperty =
            DependencyProperty.Register(
            "MeterForeground",
            typeof(SolidColorBrush),
            typeof(EmotionMeterControl),
            new PropertyMetadata(new SolidColorBrush(Colors.White))
            );

        public SolidColorBrush MeterForeground
        {
            get { return (SolidColorBrush)GetValue(MeterForegroundProperty); }
            set { SetValue(MeterForegroundProperty, (SolidColorBrush)value); }
        }

        public string EmotionName
        {
            get { return (string)GetValue(EmotionNameProperty); }
            set { SetValue(EmotionNameProperty, (string)value); }
        }
        public float EmotionValue
        {
            get { return (float)GetValue(EmotionValueProperty); }
            set { SetValue(EmotionValueProperty, (float)value); }
        }
    }
}
