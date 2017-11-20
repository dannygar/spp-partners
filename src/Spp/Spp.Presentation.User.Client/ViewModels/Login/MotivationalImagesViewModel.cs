/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using GalaSoft.MvvmLight.Ioc;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftSportsScience.ViewModels
{
    public class MotivationalImagesViewModel : NotificationBase
    {
        private MotivationalImagesModel _imagesModel;
        private AppSessionModel _sessionModel;

        public MotivationalImagesViewModel() : base()
        {
            _imagesModel = SimpleIoc.Default.GetInstance<MotivationalImagesModel>();
            _sessionModel = SimpleIoc.Default.GetInstance<AppSessionModel>();
        }

        private List<string> _motivationalImages;

        public List<string> MotivationalImages
        {
            get { return _motivationalImages; }
            set { SetProperty(ref _motivationalImages, value); }
        }

        private string _currentImage;
        public string CurrentImage
        {
            get { return _currentImage == null ? String.Empty : _currentImage; }
            set { SetProperty(ref _currentImage, value); }
        }

        public void MoveNext()
        {
            if (MotivationalImages.Any())
            {
                int currentIndex = MotivationalImages.IndexOf(_currentImage) + 1;
                if (currentIndex >= MotivationalImages.Count() - 1)
                    currentIndex = 0;
                CurrentImage = MotivationalImages[currentIndex];
            }
        }


        public override async Task Load()
        {
            var ti = await _imagesModel.GetImages();

            if (ti != null && ti.Any())
            {
                Random r = new Random();
                MotivationalImages = ti.Select(i => i.Url).OrderBy(i => r.Next()).Distinct().ToList();
                CurrentImage = MotivationalImages.First();
            }

        }
    }
}
