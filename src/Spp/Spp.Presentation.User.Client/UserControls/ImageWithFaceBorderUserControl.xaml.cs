/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face.Contract;
using Spp.Presentation.User.Client.Helpers;
using Spp.Presentation.User.Shared.CognitiveServiceHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public class EmotionFeedback
    {
        public string Text { get; set; }
        public SolidColorBrush AccentColor { get; set; }
        public string ImageFileName { get; set; }
    }

    public interface IEmotionFeedbackDataProvider
    {
        EmotionFeedback GetEmotionFeedback(Emotion emotion);
    }

    public sealed partial class ImageWithFaceBorderUserControl : UserControl
    {
        public event EventHandler<List<FaceCharacteristics>> FaceRecognitionCompleted;
        private List<FaceCharacteristics> recognizedFaces = new List<FaceCharacteristics>();


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

        public static readonly DependencyProperty ShowEmotionRecognitionProperty =
            DependencyProperty.Register(
            "ShowEmotionRecognition",
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

        public bool ShowEmotionRecognition
        {
            get { return (bool)GetValue(ShowEmotionRecognitionProperty); }
            set { SetValue(ShowEmotionRecognitionProperty, (bool)value); }
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
            ImageAnalyzer dataContext = this.DataContext as ImageAnalyzer;

            foreach (var child in this.hostGrid.Children.Where(c => !(c is Image)).ToArray())
            {
                this.hostGrid.Children.Remove(child);
            }

            // remove the current source
            this.bitmapImage.UriSource = null;

            if (dataContext != null)
            {
                try
                {
                    if (dataContext.ImageUrl != null)
                    {
                        this.bitmapImage.UriSource = new Uri(dataContext.ImageUrl);
                    }
                    else if (dataContext.GetImageStreamCallback != null)
                    {
                        await this.bitmapImage.SetSourceAsync((await dataContext.GetImageStreamCallback()).AsRandomAccessStream());
                    }
                }
                catch (Exception ex)
                {
                    // If we fail to load the image we will just not display it
                    this.bitmapImage.UriSource = null;
                    if (this.ShowDialogOnApiErrors)
                    {
                        await FaceRecognitionHelper.GenericApiCallExceptionHandler(ex, "Error loading captured image.");
                    }
                }
            }
        }

        private async Task DetectAndShowFaceBorders()
        {
            this.progressIndicator.IsActive = true;

            foreach (var child in this.hostGrid.Children.Where(c => !(c is Image)).ToArray())
            {
                this.hostGrid.Children.Remove(child);
            }

            ImageAnalyzer imageWithFace = this.DataContext as ImageAnalyzer;
            if (imageWithFace != null)
            {
                //Detect faces
                if (imageWithFace.DetectedFaces == null)
                {
                    await imageWithFace.DetectFacesAsync(detectFaceAttributes: this.DetectFaceAttributes);
                }

                //Detect emotions
                if (imageWithFace.DetectedEmotion == null)
                {
                    await imageWithFace.DetectEmotionAsync();
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
                        var people = new List<IdentifiedPerson>();
                        foreach (Face face in imageWithFace.DetectedFaces)
                        {
                            // Get the border for the associated face id
                            FaceIdentificationBorder faceUI = (FaceIdentificationBorder)this.hostGrid.Children.
                                FirstOrDefault(e => e is FaceIdentificationBorder
                                && (Guid)(e as FaceIdentificationBorder).Tag == face.FaceId);

                            if (faceUI != null)
                            {
                                var faceIdIdentification = imageWithFace.IdentifiedPersons.FirstOrDefault(p => p.FaceId == face.FaceId);

                                string name = this.DetectFaceAttributes && faceIdIdentification != null ? faceIdIdentification.Person.Name : null;
                                string gender = this.DetectFaceAttributes ? face.FaceAttributes.Gender : null;
                                double age = this.DetectFaceAttributes ? face.FaceAttributes.Age : 0;
                                double confidence = this.DetectFaceAttributes && faceIdIdentification != null ? faceIdIdentification.Confidence : 0;

                                //Add to the collection of recognized people
                                people.Add(faceIdIdentification);

                                if (!Defines.FACE_RECOGNITION_DEBUG && name != null)
                                    faceUI.ShowIdentificationMessage($"Welcome back, {name}!");
                                else
                                {
                                    faceUI.ShowIdentificationData(age, gender,
                                            (confidence > 0) ? (uint)Math.Round(confidence * 100) : 0, name);
                                }
                            }
                        }

                        //Delay # of seconds for the user to see the quick result
                        await Task.Delay(Defines.FACE_RECOGNITION_MESSAGE_DELAY);

                        //Add emotions
                        var emotions = await this.DetectAndShowEmotion();

                        //Add all recognized people and their emotions into the collection
                        for (var i = 0; i < people.Count; i++)
                        {
                            recognizedFaces.Add(new FaceCharacteristics()
                            {
                                Person = people[i],
                                Emotion = (emotions.Count > i) ? emotions[i] : null,
                            });
                        }

                        //Delay # of seconds for the user to see the quick result
                        await Task.Delay(Defines.FACE_RECOGNITION_MESSAGE_DELAY);

                        //Delegate the event to the top handler
                        this.OnFaceRecognitionCompleted(recognizedFaces);

                    }
                }
            }

            this.progressIndicator.IsActive = false;
        }

        private async Task<List<Emotion>> DetectAndShowEmotion()
        {
            List<Emotion> emotions = new List<Emotion>();
            this.progressIndicator.IsActive = true;
            double leftShift = 0;

            foreach (var child in this.hostGrid.Children.Where(c => !(c is Image)).ToArray())
            {
                leftShift = ((FrameworkElement)child).ActualWidth;
                //this.hostGrid.Children.Remove(child);
            }

            ImageAnalyzer imageWithFace = this.DataContext as ImageAnalyzer;
            if (imageWithFace != null)
            {
                if (imageWithFace.DetectedEmotion == null)
                {
                    await imageWithFace.DetectEmotionAsync();
                }

                double renderedImageXTransform = this.imageControl.RenderSize.Width / this.bitmapImage.PixelWidth;
                double renderedImageYTransform = this.imageControl.RenderSize.Height / this.bitmapImage.PixelHeight;

                foreach (Emotion emotion in imageWithFace.DetectedEmotion)
                {
                    if (Defines.FACE_RECOGNITION_DEBUG)
                    {
                        FaceIdentificationBorder faceUI = new FaceIdentificationBorder();

                        //emotion.FaceRectangle.Left -= (int)((leftShift * renderedImageXTransform) - ((this.ActualWidth - this.imageControl.RenderSize.Width) / 2)) - 20;
                        faceUI.Margin = new Thickness(((emotion.FaceRectangle.Left) * renderedImageXTransform) +
                            ((this.ActualWidth - this.imageControl.RenderSize.Width) / 2),
                            (emotion.FaceRectangle.Top * renderedImageYTransform) + ((this.ActualHeight - this.imageControl.RenderSize.Height) / 2),
                                0, 0);

                        faceUI.BalloonBackground = this.BalloonBackground;
                        faceUI.BalloonForeground = this.BalloonForeground;


                        faceUI.ShowEmotionData(emotion);
                        this.hostGrid.Children.Add(faceUI);
                    }


                    emotions.Add(emotion);

                    if (!this.ShowMultipleFaces)
                    {
                        break;
                    }
                }
            }

            this.progressIndicator.IsActive = false;
            return emotions;
        }

        private async Task PreviewImageFaces()
        {
            if (!this.DetectFacesOnLoad || this.progressIndicator.IsActive)
            {
                return;
            }

            ImageAnalyzer img = this.DataContext as ImageAnalyzer;
            img?.UpdateDecodedImageSize(this.bitmapImage.PixelHeight, this.bitmapImage.PixelWidth);

            //if (this.ShowEmotionRecognition)
            //{
            //}
            if (this.DetectFaceAttributes)
            {
                await this.DetectAndShowFaceBorders();
            }
        }

        private async void OnBitmapImageOpened(object sender, RoutedEventArgs e)
        {
            await PreviewImageFaces();
        }

        private async void OnImageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            await this.PreviewImageFaces();
        }


        private void OnFaceRecognitionCompleted(List<FaceCharacteristics> faces)
        {
            FaceRecognitionCompleted?.Invoke(this, faces);
        }
    }
}
