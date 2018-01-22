/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Spp.Presentation.User.Client.Constants;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Helpers;
using Windows.UI.Xaml.Data;

namespace Spp.Presentation.User.Client.Converters.ToText
{
    public class HeightToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var player = value as Athlete;

            var heightText = player?.Height == null ? DataConsts.DefaultHeightText : player.Height.Value.ToString();

            var measure = ResourceHelper.GetString("HeightMeasure");

            if (!string.IsNullOrWhiteSpace(measure))
            {
                return $"{heightText} {measure}";
            }

            return heightText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
