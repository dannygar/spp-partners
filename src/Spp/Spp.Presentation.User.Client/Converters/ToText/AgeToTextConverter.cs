/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Spp.Presentation.User.Client.Constants;
using Spp.Presentation.User.Client.Data;
using Windows.UI.Xaml.Data;

namespace Spp.Presentation.User.Client.Converters.ToText
{
    public class AgeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var player = value as Athlete;

            if (player?.Age == null)
            {
                return DataConsts.DefaultAgeText;
            }

            return player.Age.Value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
