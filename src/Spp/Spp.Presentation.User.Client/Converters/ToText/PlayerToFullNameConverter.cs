/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Spp.Presentation.User.Client.Data;
using Windows.UI.Xaml.Data;

namespace Spp.Presentation.User.Client.Converters.ToText
{
    public class PlayerToFullNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var player = (Athlete)value;

            if (player == null)
            {
                return string.Empty;
            }

            var fname = player.FirstName ?? string.Empty;
            var lname = player.LastName ?? string.Empty;

            return $"{fname} {lname}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
