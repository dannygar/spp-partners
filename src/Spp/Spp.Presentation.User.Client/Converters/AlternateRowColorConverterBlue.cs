/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Spp.Presentation.User.Client.Converters
{
    public class AlternateRowColorConverterBlue : IValueConverter
    {
        bool isAlternate;
        SolidColorBrush even = new SolidColorBrush(Color.FromArgb(255, 0, 71, 127));
        SolidColorBrush odd = new SolidColorBrush(Color.FromArgb(255, 0, 67, 122));

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            isAlternate = !isAlternate;
            return isAlternate ? even : odd;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
