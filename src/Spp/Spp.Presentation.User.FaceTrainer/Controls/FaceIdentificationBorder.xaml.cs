/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FaceAPITrainer.Controls
{
    public sealed partial class FaceIdentificationBorder : UserControl
    {
        public static readonly DependencyProperty BalloonBackgroundProperty =
            DependencyProperty.Register(
            "BalloonBackground",
            typeof(SolidColorBrush),
            typeof(FaceIdentificationBorder),
            new PropertyMetadata(null)
            );

        public SolidColorBrush BalloonBackground
        {
            get { return (SolidColorBrush)GetValue(BalloonBackgroundProperty); }
            set { SetValue(BalloonBackgroundProperty, (SolidColorBrush)value); }
        }

        public static readonly DependencyProperty BalloonForegroundProperty =
            DependencyProperty.Register(
            "BalloonForeground",
            typeof(SolidColorBrush),
            typeof(FaceIdentificationBorder),
            new PropertyMetadata(null)
            );

        public SolidColorBrush BalloonForeground
        {
            get { return (SolidColorBrush)GetValue(BalloonForegroundProperty); }
            set { SetValue(BalloonForegroundProperty, (SolidColorBrush)value); }
        }

        public string CaptionText { get; set; }

        public FaceIdentificationBorder()
        {
            this.InitializeComponent();
        }

        public void ShowFaceRectangle(double width, double height)
        {
            this.faceRectangle.Width = width;
            this.faceRectangle.Height = height;

            this.faceRectangle.Visibility = Visibility.Visible;
        }

        public void ShowIdentificationData(double age, string gender, uint confidence, string name = null)
        {
            int roundedAge = (int)Math.Round(age);

            if (!string.IsNullOrEmpty(name))
            {
                this.CaptionText = string.Format("{0}, {1} ({2}%)", name, roundedAge, confidence);
                this.genderIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.CaptionText = roundedAge.ToString();
                if (string.Compare(gender, "male", true) == 0)
                {
                    this.genderIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/male.png"));
                }
                else if (string.Compare(gender, "female", true) == 0)
                {
                    this.genderIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/female.png"));
                }
            }

            this.DataContext = this;
            this.captionCanvas.Visibility = Visibility.Visible;
        }

        private void OnCaptionSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.captionCanvas.Margin = new Thickness(this.faceRectangle.Margin.Left - (this.captionCanvas.ActualWidth - this.faceRectangle.ActualWidth) / 2,
                                                      -this.captionCanvas.ActualHeight, 0, 0);

        }
    }
}