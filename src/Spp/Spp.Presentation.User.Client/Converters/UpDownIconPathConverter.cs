/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Windows.UI.Xaml.Data;

namespace Spp.Presentation.User.Client.Converters
{
    /// <summary>
    /// Returns Arrow angle to make Left arrow be Up or Down
    /// </summary>
    public class UpDownIconPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            // Default: up
            return int.Parse(value.ToString()) >= 0 ? 0.0 : 180.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
