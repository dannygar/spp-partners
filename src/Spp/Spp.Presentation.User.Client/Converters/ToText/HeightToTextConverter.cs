/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Converters.ToText
{
    using System;
    using Constants;
    using Data;
    using Helpers;
    using Windows.UI.Xaml.Data;

    public class HeightToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var player = value as Athlete;

            var heightText = player?.Height == null ? DataConsts.DefaultHeightText : player.Height.Value.ToString();

            var measure = ResourceHelper.GetString("HeightMeasure");

            if (!string.IsNullOrWhiteSpace(measure))
            {
                return $"{heightText} {measure}";
            }

            return heightText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
