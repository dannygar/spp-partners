/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftSportsScience.ViewModels
{
    public class CalendarSectionViewModel : NotificationBase
    {
        Random random = new Random();
        bool _filled;
        string _backgroundColor;

        public CalendarSectionViewModel()
        {
            _filled = (random.Next(2) == 1) ? false : true;
            _backgroundColor = (_filled == true) ? "#FF0000" : "#00FF00";
        }

        public string BackgroundColor => _backgroundColor;
        
        public override Task Load() => null;
        public double Height;
        public double Width;
    }
}
