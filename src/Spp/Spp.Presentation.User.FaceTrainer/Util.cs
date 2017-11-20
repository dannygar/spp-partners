/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace FaceAPITrainer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ProjectOxford.Face;

    using Windows.Devices.Enumeration;
    using Windows.UI.Popups;
    using System.Net.Http;
    using System.IO;
    internal class Util
    {
        public static string CapitalizeString(string s)
        {
            return string.Join(" ", s.Split(' ').Select(word => !string.IsNullOrEmpty(word) ? char.ToUpper(word[0]) + word.Substring(1) : string.Empty));
        }

        public static async Task ConfirmActionAndExecute(string message, Func<Task> action)
        {
            var messageDialog = new MessageDialog(message);

            messageDialog.Commands.Add(new UICommand("Yes", async c => await action()));
            messageDialog.Commands.Add(new UICommand("Cancel", c => { }));

            messageDialog.DefaultCommandIndex = 1;
            messageDialog.CancelCommandIndex = 1;

            await messageDialog.ShowAsync();
        }

        public static async Task<IEnumerable<string>> GetAvailableCameraNamesAsync()
        {
            DeviceInformationCollection deviceInfo = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            return deviceInfo.OrderBy(d => d.Name).Select(d => d.Name);
        }

        internal static async Task GenericApiCallExceptionHandler(Exception ex, string errorTitle)
        {
            string errorDetails = ex.Message;

            FaceAPIException faceApiException = ex as FaceAPIException;
            if (faceApiException != null)
            {
                errorDetails = faceApiException.ErrorMessage;
            }

            await new MessageDialog(errorDetails, errorTitle).ShowAsync();
        }

        internal static async Task GenericErrorHandler(string errorTitle, string errorDetails)
        {
            var meaasgeBody = $"{errorDetails}\n\n"
                              + "This means that any synchronization with service could work wrong.\n"
                              + "Please, check application settings and Internet connection carefully.\n"
                              + "If there are alright, please, notify the application developers.";

            await new MessageDialog(meaasgeBody, errorTitle).ShowAsync();
        }

        internal static async Task GenericWarningHandler(string warningTitle, string warningDetails)
        {
            await new MessageDialog(warningDetails, warningTitle).ShowAsync();
        }

        internal static async Task<Stream> GetPhoto(string url)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetStreamAsync(url);
            }
        }
    }
}
