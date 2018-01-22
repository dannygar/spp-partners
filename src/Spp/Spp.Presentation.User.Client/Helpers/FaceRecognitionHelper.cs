/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Spp.Presentation.User.Shared.CognitiveServiceHelpers;
using Windows.Devices.Enumeration;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace Spp.Presentation.User.Client.Helpers
{
    internal class FaceRecognitionHelper
    {
        public static string CapitalizeString(string s)
        {
            return string.Join(" ", s.Split(' ').Select(word => !string.IsNullOrEmpty(word) ? char.ToUpper(word[0]) + word.Substring(1) : string.Empty));
        }

        internal static async Task GenericApiCallExceptionHandler(Exception ex, string errorTitle)
        {
            string errorDetails = ex.Message;

            FaceAPIException faceApiException = ex as FaceAPIException;
            if (faceApiException != null)
            {
                errorDetails = faceApiException.ErrorMessage;
            }
            else
            {
                ClientException clientException = ex as ClientException;
                if (clientException != null)
                {
                    errorDetails = clientException.Error.Message;
                }
            }

            await new MessageDialog(errorDetails, errorTitle).ShowAsync();
        }

        internal static Face FindFaceClosestToRegion(IEnumerable<Face> faces, BitmapBounds region)
        {
            return faces?.Where(f => FaceRecognitionHelper.AreFacesPotentiallyTheSame(region, f.FaceRectangle))
                                  .OrderBy(f => Math.Abs(region.X - f.FaceRectangle.Left) + Math.Abs(region.Y - f.FaceRectangle.Top)).FirstOrDefault();
        }

        internal static bool AreFacesPotentiallyTheSame(BitmapBounds face1, FaceRectangle face2)
        {
            return CoreUtil.AreFacesPotentiallyTheSame((int)face1.X, (int)face1.Y, (int)face1.Width, (int)face1.Height, face2.Left, face2.Top, face2.Width, face2.Height);
        }

        public static async Task ConfirmActionAndExecute(string message, Func<Task> action)
        {
            var messageDialog = new MessageDialog(message);

            messageDialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(async (c) => await action())));
            messageDialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler((c) => { })));

            messageDialog.DefaultCommandIndex = 1;
            messageDialog.CancelCommandIndex = 1;

            await messageDialog.ShowAsync();
        }

        public static async Task<IEnumerable<string>> GetAvailableCameraNamesAsync()
        {
            DeviceInformationCollection deviceInfo = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            return deviceInfo.OrderBy(d => d.Name).Select(d => d.Name);
        }

        async private static Task CropBitmapAsync(Stream localFileStream, Rectangle rectangle, StorageFile resultFile)
        {
            //Get pixels of the crop region
            var pixels = await GetCroppedPixelsAsync(localFileStream.AsRandomAccessStream(), rectangle);

            // Save result to new image
            using (Stream resultStream = await resultFile.OpenStreamForWriteAsync())
            {
                IRandomAccessStream randomAccessStream = resultStream.AsRandomAccessStream();
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, randomAccessStream);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                        BitmapAlphaMode.Ignore,
                                        (uint)rectangle.Width, (uint)rectangle.Height,
                                        DisplayInformation.GetForCurrentView().LogicalDpi, DisplayInformation.GetForCurrentView().LogicalDpi, pixels);

                await encoder.FlushAsync();
            }
        }

        async public static Task CropBitmapAsync(Func<Task<Stream>> localFile, Rectangle rectangle, StorageFile resultFile)
        {
            await CropBitmapAsync(await localFile(), rectangle, resultFile);
        }

        async private static Task<byte[]> GetCroppedPixelsAsync(IRandomAccessStream stream, Rectangle rectangle)
        {
            // Create a decoder from the stream. With the decoder, we can get  
            // the properties of the image. 
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

            // Create cropping BitmapTransform and define the bounds. 
            BitmapTransform transform = new BitmapTransform();
            BitmapBounds bounds = new BitmapBounds();
            bounds.X = (uint)rectangle.Left;
            bounds.Y = (uint)rectangle.Top;
            bounds.Height = (uint)rectangle.Height;
            bounds.Width = (uint)rectangle.Width;
            transform.Bounds = bounds;

            // Get the cropped pixels within the bounds of transform. 
            PixelDataProvider pix = await decoder.GetPixelDataAsync(
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Straight,
                transform,
                ExifOrientationMode.IgnoreExifOrientation,
                ColorManagementMode.ColorManageToSRgb);

            return pix.DetachPixelData();
        }
    }
}
