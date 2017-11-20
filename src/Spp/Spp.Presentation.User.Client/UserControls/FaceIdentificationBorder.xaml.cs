/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: http://www.microsoft.com/cognitive
// 
// Microsoft Cognitive Services Github:
// https://github.com/Microsoft/Cognitive
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Spp.Presentation.User.Shared.CognitiveServiceHelpers;
using Microsoft.ProjectOxford.Emotion.Contract;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MicrosoftSportsScience.UserControls
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

        public EmotionData[] EmotionData { get; set; }

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
            else if (!string.IsNullOrEmpty(gender))
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


        public void ShowIdentificationMessage(string message)
        {
            this.CaptionText = message;
            this.genderIcon.Visibility = Visibility.Collapsed;

            this.DataContext = this;
            this.captionCanvas.Visibility = Visibility.Visible;
        }


        public void ShowEmotionData(Emotion emotion)
        {
            this.EmotionData = EmotionServiceHelper.ScoresToEmotionData(emotion.Scores).OrderByDescending(e => e.EmotionScore).ToArray();

            this.DataContext = this;

            this.genderAgeGrid.Visibility = Visibility.Collapsed;
            this.emotionGrid.Visibility = Visibility.Visible;
            this.captionCanvas.Visibility = Visibility.Visible;
        }

        private void OnCaptionSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.captionCanvas.Margin = new Thickness(this.faceRectangle.Margin.Left - (this.captionCanvas.ActualWidth - this.faceRectangle.ActualWidth) / 2,
                                                      -this.captionCanvas.ActualHeight, 0, 0);
        }
    }
}