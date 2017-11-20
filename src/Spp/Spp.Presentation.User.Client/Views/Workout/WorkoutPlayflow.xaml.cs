/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Services;
using MicrosoftSportsScience.Models;
using MicrosoftSportsScience.UserControls;
using MicrosoftSportsScience.ViewModels;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MicrosoftSportsScience
{
    public sealed partial class WorkoutPlayflow : Page
    {
        SplitView rootPage = Shell.Current;
        private int _imageNumber = 0;
        private int _totalNumber = 30; //hard coded for now

        public WorkoutPlayflow()
        {
            InitializeComponent();
            rootPage.CompactPaneLength = 50;
            UpdateImageSrc();
        }

        private void WorkoutImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UpdateImageSrc();
        }

        private void UpdateImageSrc()
        {
            //bool imageExists = await DoesImageExist();
            ////Check to see if image exists first
            //if (imageExists == false)
            //{

            //}


            if (_imageNumber <= _totalNumber)
            {
                string stringPath = string.Format("ms-appx:///Assets/Images/ClickThrough/WorkoutPlayflow-{0}.jpg", _imageNumber);
                Uri imageUri = new Uri(stringPath, UriKind.RelativeOrAbsolute);
                BitmapImage imageBitmap = new BitmapImage(imageUri);

                WorkoutImage.Source = imageBitmap;
                _imageNumber++;
            }
        }

        private async Task<bool> DoesImageExist()
        {
            bool imageIsThere = false;
            string imageFile = @"Assets\Images\ClickThrough\WorkoutPlayflow-0.jpg";
            StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await InstallationFolder.GetFileAsync(imageFile);
            if (file != null)
            {
                imageIsThere = true;
            }
            return imageIsThere;
        }
    }
}
