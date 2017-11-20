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
    /// Converts percentage int value to string like "28%"
    /// </summary>
    public class PercentageConverter : IValueConverter
    {
        private const string DefaultNullRepresentation = "N/A";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return DefaultNullRepresentation;
            }

            var percentage = value.ToString().ToNullableInt();
            if (percentage.HasValue)
            {
                return string.Format("{0}%", percentage);
            }

            return DefaultNullRepresentation;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
