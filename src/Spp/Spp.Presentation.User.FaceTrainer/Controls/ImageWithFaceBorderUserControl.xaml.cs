/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Microsoft.ProjectOxford.Face.Contract;
using System;
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
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FaceAPITrainer.Controls
{
    public sealed partial class ImageWithFaceBorderUserControl : UserControl
    {
        public ImageWithFaceBorderUserControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty DetectFacesOnLoadProperty =
            DependencyProperty.Register(
            "DetectFacesOnLoad",
            typeof(bool),
            typeof(ImageWithFaceBorderUserControl),
            new PropertyMetadata(false)
            );

        public static readonly DependencyProperty ShowMultipleFacesProperty =
            DependencyProperty.Register(
            "ShowMultipleFaces",
            typeof(bool),
            typeof(ImageWithFaceBorderUserControl), 
            new PropertyMetadata(false)
            );

        public static readonly DependencyProperty PerformRecognitionProperty =
            DependencyProperty.Register(
            "PerformRecognition",
            typeof(bool),
            typeof(ImageWithFaceBorderUserControl),
            new PropertyMetadata(false)
            );

        public static readonly DependencyProperty DetectFaceAttributesProperty =
            DependencyProperty.Register(
            "DetectFaceAttributes",
            typeof(bool),
            typeof(ImageWithFaceBorderUserControl),
            new PropertyMetadata(false)
            );

        public static readonly DependencyProperty ShowRecognitionResultsProperty =
            DependencyProperty.Register(
            "ShowRecognitionResults",
            typeof(bool),
            typeof(ImageWithFaceBorderUserControl),
            new PropertyMetadata(false)
            );

        public static readonly DependencyProperty ShowDialogOnApiErrorsProperty =
            DependencyProperty.Register(
            "ShowDialogOnApiErrors",
            typeof(bool),
            typeof(ImageWithFaceBorderUserControl),
            new PropertyMetadata(false)
            );

        public static readonly DependencyProperty BalloonBackgroundProperty =
            DependencyProperty.Register(
            "BalloonBackground",
            typeof(SolidColorBrush),
            typeof(ImageWithFaceBorderUserControl),
            new PropertyMetadata(null)
            );

        public static readonly DependencyProperty BalloonForegroundProperty =
            DependencyProperty.Register(
            "BalloonForeground",
            typeof(SolidColorBrush),
            typeof(ImageWithFaceBorderUserControl),
            new PropertyMetadata(null)
            );

        public SolidColorBrush BalloonBackground
        {
            get { return (SolidColorBrush)GetValue(BalloonBackgroundProperty); }
            set { SetValue(BalloonBackgroundProperty, (SolidColorBrush)value); }
        }

        public SolidColorBrush BalloonForeground
        {
            get { return (SolidColorBrush)GetValue(BalloonForegroundProperty); }
            set { SetValue(BalloonForegroundProperty, (SolidColorBrush)value); }
        }

        public bool ShowMultipleFaces
        {
            get { return (bool)GetValue(ShowMultipleFacesProperty); }
            set { SetValue(ShowMultipleFacesProperty, (bool)value); }
        }

        public bool DetectFacesOnLoad
        {
            get { return (bool)GetValue(DetectFacesOnLoadProperty); }
            set { SetValue(DetectFacesOnLoadProperty, (bool)value); }
        }

        public bool DetectFaceAttributes
        {
            get { return (bool)GetValue(DetectFaceAttributesProperty); }
            set { SetValue(DetectFaceAttributesProperty, (bool)value); }
        }

        public bool PerformRecognition
        {
            get { return (bool)GetValue(PerformRecognitionProperty); }
            set { SetValue(PerformRecognitionProperty, (bool)value); }
        }

        public bool ShowRecognitionResults
        {
            get { return (bool)GetValue(ShowRecognitionResultsProperty); }
            set { SetValue(ShowRecognitionResultsProperty, (bool)value); }
        }

        public bool ShowDialogOnApiErrors
        {
            get { return (bool)GetValue(ShowDialogOnApiErrorsProperty); }
            set { SetValue(ShowDialogOnApiErrorsProperty, (bool)value); }
        }

        private async void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            ImageWithFace dataContext = this.DataContext as ImageWithFace;

            foreach (var child in this.hostGrid.Children.Where(c => !(c is Image)).ToArray())
            {
                this.hostGrid.Children.Remove(child);
            }

            if (dataContext != null)
            {
                try
                {
                    if (dataContext.ImageUrl != null)
                    {
                        this.bitmapImage.UriSource = new Uri(dataContext.ImageUrl);
                    }
                    else if (dataContext.LocalImageFile != null)
                    {
                        this.bitmapImage.SetSource(await dataContext.LocalImageFile.OpenReadAsync());
                    }

                }
                catch (Exception ex)
                {
                    // If we fail to load the image we will just not display it
                    this.bitmapImage.UriSource = null;
                    if (this.ShowDialogOnApiErrors)
                    {
                        await Util.GenericApiCallExceptionHandler(ex, "Error loading captured image.");
                    }
                }
            }
            else
            {
                this.bitmapImage.UriSource = null;
            }
        }

        private async Task DetectAndShowFaceBorders()
        {
            this.progressIndicator.IsActive = true;

            foreach (var child in this.hostGrid.Children.Where(c => !(c is Image)).ToArray())
            {
                this.hostGrid.Children.Remove(child);
            }

            ImageWithFace imageWithFace = this.DataContext as ImageWithFace;
            if (imageWithFace != null)
            {
                if (imageWithFace.DetectedFaces == null)
                {
                    await imageWithFace.DetectFacesAsync(detectFaceAttributes: this.DetectFaceAttributes, imageWidth: this.bitmapImage.PixelWidth, imageHeight: this.bitmapImage.PixelHeight);
                }

                double renderedImageXTransform = this.imageControl.RenderSize.Width / this.bitmapImage.PixelWidth;
                double renderedImageYTransform = this.imageControl.RenderSize.Height / this.bitmapImage.PixelHeight;

                foreach (Face face in imageWithFace.DetectedFaces)
                {
                    FaceIdentificationBorder faceUI = new FaceIdentificationBorder()
                    {
                        Tag = face.FaceId,
                    };

                    faceUI.Margin = new Thickness((face.FaceRectangle.Left * renderedImageXTransform) + ((this.ActualWidth - this.imageControl.RenderSize.Width) / 2),
                                                  (face.FaceRectangle.Top * renderedImageYTransform) + ((this.ActualHeight - this.imageControl.RenderSize.Height) / 2), 0, 0);

                    faceUI.BalloonBackground = this.BalloonBackground;
                    faceUI.BalloonForeground = this.BalloonForeground;
                    faceUI.ShowFaceRectangle(face.FaceRectangle.Width * renderedImageXTransform, face.FaceRectangle.Height * renderedImageYTransform);

                    this.hostGrid.Children.Add(faceUI);

                    if (!this.ShowMultipleFaces)
                    {
                        break;
                    }
                }

                if (this.PerformRecognition)
                {
                    if (imageWithFace.IdentifiedPersons == null)
                    {
                        await imageWithFace.IdentifyFacesAsync();
                    }

                    if (this.ShowRecognitionResults)
                    {
                        foreach (Face face in imageWithFace.DetectedFaces)
                        {
                            // Get the border for the associated face id
                            FaceIdentificationBorder faceUI = (FaceIdentificationBorder)this.hostGrid.Children.FirstOrDefault(e => e is FaceIdentificationBorder && (Guid)(e as FaceIdentificationBorder).Tag == face.FaceId);

                            if (faceUI != null)
                            {
                                IdentifiedPerson faceIdIdentification = imageWithFace.IdentifiedPersons.FirstOrDefault(p => p.FaceId == face.FaceId);

                                string name = this.DetectFaceAttributes && faceIdIdentification != null ? faceIdIdentification.Person.Name : null;
                                string gender = this.DetectFaceAttributes ? face.FaceAttributes.Gender : null;
                                double age = this.DetectFaceAttributes ? face.FaceAttributes.Age : 0;
                                double confidence = this.DetectFaceAttributes && faceIdIdentification != null ? faceIdIdentification.Confidence : 0;

                                faceUI.ShowIdentificationData(age, gender, (uint)Math.Round(confidence * 100), name);
                            }
                        }
                    }
                }
            }

            this.progressIndicator.IsActive = false;
        }

        private async Task PreviewImageFaces()
        {
            if (!this.DetectFacesOnLoad || this.progressIndicator.IsActive)
            {
                return;
            }

            await this.DetectAndShowFaceBorders();
        }

        private async void OnBitmapImageOpened(object sender, RoutedEventArgs e)
        {
            await PreviewImageFaces();
        }

        private async void OnImageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            await this.PreviewImageFaces();
        }

    }
}
