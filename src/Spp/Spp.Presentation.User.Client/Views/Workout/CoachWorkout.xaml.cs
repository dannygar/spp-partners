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
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MicrosoftSportsScience
{
    public sealed partial class CoachWorkout : Page
    {
        private int _imageNumber = 0;
        private int _totalNumber = 20; //hard coded for now

        public CoachWorkout()
        {
            InitializeComponent();
            UpdateImageSrc();
        }

        private void WorkoutImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UpdateImageSrc();
        }

        private void UpdateImageSrc()
        {

            //Check to see if image exists first
            //StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            //string theImage = @"Assets\Images\ClickThrough\MS-Sports-Science-Coach-Workout-0.jpg";

            if (_imageNumber <= _totalNumber)
            {
                string stringPath = string.Format("ms-appx:///Assets/Images/ClickThrough/WorkoutFlow-{0}.jpg", _imageNumber);
                Uri imageUri = new Uri(stringPath, UriKind.RelativeOrAbsolute);
                BitmapImage imageBitmap = new BitmapImage(imageUri);

                WorkoutImage.Source = imageBitmap;
                _imageNumber++;
            }

            

            
        }
    }
}
