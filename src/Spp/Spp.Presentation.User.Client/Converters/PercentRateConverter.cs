/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Converters
{
    using System;
    using MicrosoftSportsScience.Helpers;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// Returns percent-rated base value.
    /// </summary>
    public class PercentRateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            var percentage = value.ToString().ToNullableInt();

            int percentageValue = percentage ?? 0;
            var baseValue = double.Parse(parameter.ToString());

            return (int)(baseValue * percentageValue / 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
