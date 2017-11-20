/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Converters
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// Converts value to Thickness value for specifying left margin.
    /// </summary>
    class ReadinessMarginConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // TODO: Allow specifying different margin dimensions using parameters
            int percentage = int.Parse(value.ToString());
            int percentBarWidth = int.Parse(parameter.ToString());
            return new Thickness((int)(percentBarWidth * percentage / 100), 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
