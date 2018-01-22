/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Windows.UI.Xaml.Data;

namespace Spp.Presentation.User.Client.Converters
{
    public class IndexConverter : IValueConverter
    {
        private static int Index = 0;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Index++;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
