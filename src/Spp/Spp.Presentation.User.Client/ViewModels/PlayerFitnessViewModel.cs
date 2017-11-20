/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.ComponentModel;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using System.Linq;
using System.Collections.Generic;
using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Models;

namespace MicrosoftSportsScience.ViewModels
{
    public class PlayerFitnessViewModel : NotificationBase
    {
        public string PlayerImage;
        public string PlayerName;
        public string PlayerTrendNumber;
        public string Color;
        public string PlayerAge;
        public string PlayerPosition;
        public string PlayerHeight;
        public string PlayerJerseyNum;

        public PlayerFitnessViewModel()
        {
        }

        public override async Task Load()
        {
        }
    }
}