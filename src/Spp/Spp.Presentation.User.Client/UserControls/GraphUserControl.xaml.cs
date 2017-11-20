/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Helpers;
using System;
using System.Collections;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using System.Reflection;
using static MicrosoftSportsScience.Defines;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MicrosoftSportsScience.UserControls
{
    
    public sealed partial class GraphUserControl : UserControl
    {
        private double canvasHeight;
        private double heightIncrement;
        private double widthIncrement;
        private List<FrameworkElement> dataPoints;
        private List<FrameworkElement> dataLines;
        private List<FrameworkElement> dataRectangles;
        private List<int> dataList = new List<int>();
        private Random myRandom = new Random();
        private double halfEllipseWidth;
        private double rectangleWidth;

        public GraphUserControl()
        {
            this.InitializeComponent();
        }

        public GraphMode GraphMode
        {
            get { return (GraphMode)GetValue(GraphModeProperty); }
            set { SetValue(GraphModeProperty, value); }
        }
        
        public static readonly DependencyProperty GraphModeProperty =
            DependencyProperty.Register("GraphMode", typeof(GraphMode), typeof(GraphUserControl), new PropertyMetadata(GraphMode.Line));

        public List<string> Days
        {
            get { return (List<string>)GetValue(DaysProperty); }
            set { SetValue(DaysProperty, value); }
        }

        public static readonly DependencyProperty DaysProperty =
            DependencyProperty.Register("Days", typeof(List<string>), typeof(GraphUserControl), null);

        public List<int> Values
        {
            get { return (List<int>)GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register("Values", typeof(List<int>), typeof(GraphUserControl), null);

        private void GraphUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            widthIncrement = EllipseCanvas.ActualWidth / 7;
            heightIncrement = EllipseCanvas.ActualHeight / 10;
            canvasHeight = EllipseCanvas.ActualHeight;
            dataPoints = HelperMethods.GetChildren(EllipseCanvas);
            dataLines = HelperMethods.GetChildren(LineCanvas);
            dataRectangles = HelperMethods.GetChildren(RectangleCanvas);
            halfEllipseWidth = dataPoints[0].ActualHeight / 2;
            rectangleWidth = dataRectangles[0].Width;
            RectangleCanvasXDelta.TranslateX = (widthIncrement / 2) + (rectangleWidth / 2);

            for (int i = 0; i < dataPoints.Count; i++)
            {
                Canvas.SetTop(dataRectangles[i], canvasHeight);
                Canvas.SetLeft(dataRectangles[i], (widthIncrement * i));

                Canvas.SetTop(dataPoints[i], canvasHeight - halfEllipseWidth);
                Canvas.SetLeft(dataPoints[i], (widthIncrement * i) - halfEllipseWidth + (widthIncrement / 2));
                
                if (i < dataPoints.Count - 1)
                {
                    ((Line)dataLines[i]).X1 = (widthIncrement * i)  + (widthIncrement / 2);
                    ((Line)dataLines[i]).X2 = (widthIncrement * (i + 1))  + (widthIncrement / 2);
                    ((Line)dataLines[i]).Y1 = EllipseCanvas.ActualHeight;
                    ((Line)dataLines[i]).Y2 = EllipseCanvas.ActualHeight;
                }
            }

            if (GraphMode == GraphMode.Line)
            {
                LineCanvas.Visibility = Visibility.Visible;
                EllipseCanvas.Visibility = Visibility.Visible;
                RectangleCanvas.Visibility = Visibility.Collapsed;
            }
            else if (GraphMode == GraphMode.Bar)
            {
                LineCanvas.Visibility = Visibility.Collapsed;
                EllipseCanvas.Visibility = Visibility.Collapsed;
                RectangleCanvas.Visibility = Visibility.Visible;
            }

            AnimationSetup();
        }

        private void AnimationSetup()
        {
            ResetGraph();

            for (int i = 0; i < 7; i++)
            {
                if (i >= Values.Count)
                    break;

                // Rectangles
                var rectangleAnimationProperty = this.GetType().GetField("rectangleAnimation" + (i + 1), (BindingFlags.Instance | BindingFlags.NonPublic));
                var rectangleAnimation = rectangleAnimationProperty.GetValue(this) as EasingDoubleKeyFrame;

                if (rectangleAnimation != null)
                    rectangleAnimation.Value = canvasHeight - ((10 - Values[i]) * heightIncrement);

                // Points
                var ellipseAnimationProperty = this.GetType().GetField("ellipseAnimation" + (i + 1), (BindingFlags.Instance | BindingFlags.NonPublic));
                var ellipseAnimation = ellipseAnimationProperty.GetValue(this) as DoubleAnimation;

                if (ellipseAnimation != null)
                    ellipseAnimation.To = canvasHeight - (Values[i] * heightIncrement) - halfEllipseWidth;

                // Line
                var lineAnimationY1Property = this.GetType().GetField(String.Format("lineAnimation{0}Y1", (i + 1)), (BindingFlags.Instance | BindingFlags.NonPublic));
                var lineAnimationY2Property = this.GetType().GetField(String.Format("lineAnimation{0}Y2", (i + 1)), (BindingFlags.Instance | BindingFlags.NonPublic));

                if (lineAnimationY1Property != null)
                {
                    var lineAnimationY1 = lineAnimationY1Property.GetValue(this) as EasingDoubleKeyFrame;
                    if (lineAnimationY1 != null && i < Values.Count)
                        lineAnimationY1.Value = canvasHeight - (Values[i] * heightIncrement);
                }

                if (lineAnimationY2Property != null)
                {
                    var lineAnimationY2 = lineAnimationY2Property.GetValue(this) as EasingDoubleKeyFrame;
                    if (lineAnimationY2 != null && i + 1 < Values.Count)
                        lineAnimationY2.Value = canvasHeight - (Values[i + 1] * heightIncrement);
                }
            }

            StartAnimations();
        }

        private void ResetGraph()
        {
            for (int i = 0; i < 7; i++)
            {
                // Rectangles
                var rectangleAnimationProperty = this.GetType().GetField("rectangleAnimation" + (i + 1), (BindingFlags.Instance | BindingFlags.NonPublic));
                var rectangleAnimation = rectangleAnimationProperty.GetValue(this) as EasingDoubleKeyFrame;

                if (rectangleAnimation != null)
                    rectangleAnimation.Value = canvasHeight - ((10 - 0) * heightIncrement);

                // Points
                var ellipseAnimationProperty = this.GetType().GetField("ellipseAnimation" + (i + 1), (BindingFlags.Instance | BindingFlags.NonPublic));
                var ellipseAnimation = ellipseAnimationProperty.GetValue(this) as DoubleAnimation;

                if (ellipseAnimation != null)
                    ellipseAnimation.To = canvasHeight - (0 * heightIncrement) - halfEllipseWidth;

                // Line
                var lineAnimationY1Property = this.GetType().GetField(String.Format("lineAnimation{0}Y1", (i + 1)), (BindingFlags.Instance | BindingFlags.NonPublic));
                var lineAnimationY2Property = this.GetType().GetField(String.Format("lineAnimation{0}Y2", (i + 1)), (BindingFlags.Instance | BindingFlags.NonPublic));

                if (lineAnimationY1Property != null)
                {
                    var lineAnimationY1 = lineAnimationY1Property.GetValue(this) as EasingDoubleKeyFrame;
                    lineAnimationY1.Value = canvasHeight - (0 * heightIncrement);
                }

                if (lineAnimationY2Property != null)
                {
                    var lineAnimationY2 = lineAnimationY2Property.GetValue(this) as EasingDoubleKeyFrame;
                    lineAnimationY2.Value = canvasHeight - (0 * heightIncrement);
                }
            }
        }

        private void StartAnimations()
        {
            if (GraphMode == GraphMode.Line)
            {
                LineDataPointStoryboard.Begin();
                EllipseDataPointStoryboard.Begin();
            }
            else if (GraphMode == GraphMode.Bar)
            {
                RectangleDataPointStoryboard.Begin();
            }
        }
    }
}
