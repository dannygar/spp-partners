/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using Windows.UI.Xaml.Data;

namespace MicrosoftSportsScience.Converters
{
    public class TotalLoadHeightConverter : IValueConverter
    {
        private const double WellnessGraphHeight = 260.0;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var totalLoadValue = int.Parse(value.ToString());
            var maxValue = int.Parse(parameter.ToString());

            double result = WellnessGraphHeight * (totalLoadValue * 1.0 / maxValue);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}