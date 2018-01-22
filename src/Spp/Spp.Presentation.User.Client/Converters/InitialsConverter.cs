/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Windows.UI.Xaml.Data;

namespace Spp.Presentation.User.Client.Converters
{
    public class InitialsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string s = value as string;
            string i = string.Empty;

            if (s != null)
            {
                string[] split = s.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string piece in split)
                {
                    i += piece[0];
                }
            }

            return i;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
