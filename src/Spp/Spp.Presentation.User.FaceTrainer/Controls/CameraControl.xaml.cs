/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.FaceAnalysis;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FaceAPITrainer.Controls
{
    public sealed partial class CameraControl : UserControl
    {
        public event EventHandler<ImageWithFace> ImageCaptured;
        public event EventHandler CameraRestarted;

        public static readonly DependencyProperty ShowDialogOnApiErrorsProperty =
            DependencyProperty.Register(
            "ShowDialogOnApiErrors",
            typeof(bool),
            typeof(CameraControl),
            new PropertyMetadata(true)
            );

        public bool ShowDialogOnApiErrors
        {
            get { return (bool)GetValue(ShowDialogOnApiErrorsProperty); }
            set { SetValue(ShowDialogOnApiErrorsProperty, (bool)value); }
        }

        public double CameraAspectRatio { get; set; }

        public int NumFacesOnLastFrame { get; set; }

        public CameraStreamState CameraStreamState { get { return captureManager != null ? captureManager.CameraStreamState : CameraStreamState.NotStreaming; } }

        private MediaCapture captureManager;
        private VideoEncodingProperties videoProperties;
        private FaceTracker faceTracker;
        private ThreadPoolTimer frameProcessingTimer;
        private SemaphoreSlim frameProcessingSemaphore = new SemaphoreSlim(1);

        public CameraControl()
        {
            this.InitializeComponent();
        }

        #region Camera stream processing

        public async Task StartStreamAsync()
        {
            try
            {
                await this.DeleteCapturedPhotosAsync();

                if (captureManager == null || captureManager.CameraStreamState == CameraStreamState.Shutdown)
                {
                    captureManager = new MediaCapture();

                    MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
                    var allCameras = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                    var selectedCamera = allCameras.FirstOrDefault(c => c.Name == SettingsHelper.Instance.CameraName);
                    if (selectedCamera != null)
                    {
                        settings.VideoDeviceId = selectedCamera.Id;
                    }

                    await captureManager.InitializeAsync(settings);

                    // This is now done via FlowDirection="RightToLeft" on the camera, as the video effect
                    // did not work in certain machines. It might be because it is hardware dependent.

                    // Add a video effect to flip the video horizontally, so people looking at a big screen
                    // get the same effect of looking into a mirror.
                    //Windows.Media.Effects.VideoTransformEffectDefinition flipTransform =
                    //    new Windows.Media.Effects.VideoTransformEffectDefinition();
                    //flipTransform.Mirror = MediaMirroringOptions.Horizontal;
                    //await captureManager.AddVideoEffectAsync(flipTransform, MediaStreamType.VideoPreview);

                    await SetVideoEncodingToHighestResolution();

                    this.webCamCaptureElement.Source = captureManager;
                }

                if (captureManager.CameraStreamState == CameraStreamState.NotStreaming)
                {
                    if (this.faceTracker == null)
                    {
                        this.faceTracker = await FaceTracker.CreateAsync();
                    }

                    this.videoProperties = this.captureManager.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview) as VideoEncodingProperties;

                    await captureManager.StartPreviewAsync();

                    if (this.frameProcessingTimer != null)
                    {
                        this.frameProcessingTimer.Cancel();
                        frameProcessingSemaphore.Release();
                    }
                    TimeSpan timerInterval = TimeSpan.FromMilliseconds(66); //15fps
                    this.frameProcessingTimer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(ProcessCurrentVideoFrame), timerInterval);

                    this.webCamCaptureElement.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                await Util.GenericApiCallExceptionHandler(ex, "Error starting the camera.");
            }
        }

        private async Task SetVideoEncodingToHighestResolution()
        {
            VideoEncodingProperties highestVideoEncodingSetting = this.captureManager.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.VideoPreview).Cast<VideoEncodingProperties>().OrderByDescending(v => v.Width * v.Height * (v.FrameRate.Numerator / v.FrameRate.Denominator)).First();
            if (highestVideoEncodingSetting != null)
            {
                this.CameraAspectRatio = (double)highestVideoEncodingSetting.Width / (double)highestVideoEncodingSetting.Height;
                await this.captureManager.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.VideoPreview, highestVideoEncodingSetting);
            }
        }

        private async void ProcessCurrentVideoFrame(ThreadPoolTimer timer)
        {
            if (captureManager.CameraStreamState != Windows.Media.Devices.CameraStreamState.Streaming ||
                !frameProcessingSemaphore.Wait(0))
            {
                return;
            }

            try
            {
                IEnumerable<DetectedFace> faces = null;

                // Create a VideoFrame object specifying the pixel format we want our capture image to be (NV12 bitmap in this case).
                // GetPreviewFrame will convert the native webcam frame into this format.
                const BitmapPixelFormat InputPixelFormat = BitmapPixelFormat.Nv12;
                using (VideoFrame previewFrame = new VideoFrame(InputPixelFormat, (int)this.videoProperties.Width, (int)this.videoProperties.Height))
                {
                    await this.captureManager.GetPreviewFrameAsync(previewFrame);

                    // The returned VideoFrame should be in the supported NV12 format but we need to verify this.
                    if (FaceDetector.IsBitmapPixelFormatSupported(previewFrame.SoftwareBitmap.BitmapPixelFormat))
                    {
                        faces = await this.faceTracker.ProcessNextFrameAsync(previewFrame);

                        this.NumFacesOnLastFrame = faces.Count();

                        // Create our visualization using the frame dimensions and face results but run it on the UI thread.
                        var previewFrameSize = new Windows.Foundation.Size(previewFrame.SoftwareBitmap.PixelWidth, previewFrame.SoftwareBitmap.PixelHeight);
                        var ignored = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            this.ShowFaceTrackingVisualization(previewFrameSize, faces);
                        });
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                frameProcessingSemaphore.Release();
            }
        }

        private void ShowFaceTrackingVisualization(Windows.Foundation.Size framePixelSize, IEnumerable<DetectedFace> detectedFaces)
        {
            this.FaceTrackingVisualizationCanvas.Children.Clear();

            double actualWidth = this.FaceTrackingVisualizationCanvas.ActualWidth;
            double actualHeight = this.FaceTrackingVisualizationCanvas.ActualHeight;

            if (captureManager.CameraStreamState == Windows.Media.Devices.CameraStreamState.Streaming &&
                detectedFaces != null && actualWidth != 0 && actualHeight != 0)
            {
                double widthScale = framePixelSize.Width / actualWidth;
                double heightScale = framePixelSize.Height / actualHeight;

                foreach (DetectedFace face in detectedFaces)
                {
                    // Create a rectangle element for displaying the face box but since we're using a Canvas
                    // we must scale the rectangles according to the frames's actual size.
                    Rectangle box = new Rectangle();
                    box.Width = (uint)(face.FaceBox.Width / widthScale);
                    box.Height = (uint)(face.FaceBox.Height / heightScale);
                    box.Stroke = new SolidColorBrush(Colors.White);
                    box.StrokeThickness = 1;
                    box.Margin = new Thickness((uint)(face.FaceBox.X / widthScale), (uint)(face.FaceBox.Y / heightScale), 0, 0);

                    this.FaceTrackingVisualizationCanvas.Children.Add(box);
                }
            }
        }
        public async Task StopStreamAsync()
        {
            try
            {
                if (this.frameProcessingTimer != null)
                {
                    this.frameProcessingTimer.Cancel();
                }

                if (captureManager != null && captureManager.CameraStreamState == Windows.Media.Devices.CameraStreamState.Streaming)
                {
                    this.FaceTrackingVisualizationCanvas.Children.Clear();
                    await this.captureManager.StopPreviewAsync();

                    this.FaceTrackingVisualizationCanvas.Children.Clear();
                    this.webCamCaptureElement.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                //await Util.GenericApiCallExceptionHandler(ex, "Error stopping the camera.");
            }
        }

        private async Task CapturePhotoAsync()
        {
            try
            {
                ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();

                // create storage file in local app storage
                StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(
                    "CapturePhoto.jpg",
                    CreationCollisionOption.GenerateUniqueName);

                // take photo
                await captureManager.CapturePhotoToStorageFileAsync(imgFormat, file);
                ImageWithFace imageWithFace = new ImageWithFace(file)
                {
                    ShowDialogOnFaceApiErrors = this.ShowDialogOnApiErrors,
                };

                this.OnImageCaptured(imageWithFace);
            }
            catch (Exception ex)
            {
                if (this.ShowDialogOnApiErrors)
                {
                    await Util.GenericApiCallExceptionHandler(ex, "Error capturing photo.");
                }
            }
        }

        private async Task DeleteCapturedPhotosAsync()
        {
            try
            {
                IEnumerable<StorageFile> files = (await ApplicationData.Current.TemporaryFolder.GetFilesAsync()).Where(f => f.Name.StartsWith("CapturePhoto"));
                foreach (var file in files)
                {
                    await file.DeleteAsync();
                }
            }
            catch (Exception)
            {
            }
        }

        private void OnImageCaptured(ImageWithFace imageWithFace)
        {
            if (this.ImageCaptured != null)
            {
                this.ImageCaptured(this, imageWithFace);
            }
        }

        #endregion

        private async void CameraControlButtonClick(object sender, RoutedEventArgs e)
        {
            if (captureManager.CameraStreamState == Windows.Media.Devices.CameraStreamState.Streaming)
            {
                this.cameraControlSymbol.Symbol = Symbol.Play;
                await CapturePhotoAsync();
            }
            else
            {
                this.cameraControlSymbol.Symbol = Symbol.Pause;
                await StartStreamAsync();

                if (this.CameraRestarted != null)
                {
                    this.CameraRestarted(this, EventArgs.Empty);
                }
            }
        }
    }
}
