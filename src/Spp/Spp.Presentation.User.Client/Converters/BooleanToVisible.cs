/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Spp.Presentation.User.Client.Converters
{
    public class BooleanToVisible : IValueConverter
    {
        public bool IsReversed { get; set; }

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            var val = System.Convert.ToBoolean(value);
            if (this.IsReversed)
                val = !val;

            return val
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            string culture)
        {
            throw new NotImplementedException();
        }
    }
}
