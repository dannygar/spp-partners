/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Windows.UI.Xaml.Data;

namespace Spp.Presentation.User.Client.Converters
{
    /// <summary>
    /// Returns percent-rated base value.
    /// </summary>
    public class PercentRateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            int percentage = int.Parse(value.ToString());
            var baseValue = double.Parse(parameter.ToString());

            return (int)(baseValue * percentage / 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
