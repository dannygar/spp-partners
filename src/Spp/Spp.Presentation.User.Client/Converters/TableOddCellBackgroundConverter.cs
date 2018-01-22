/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Spp.Presentation.User.Client.Converters
{
    public class TableOddCellBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var index = int.Parse(value.ToString());
            if (index % 2 == 0)
            {
                return Application.Current.Resources["TableEvenColumnBackground"] as Brush;
            }
            else
            {
                return Application.Current.Resources["TableOddColumnBackground"] as Brush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
