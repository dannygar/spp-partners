/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Spp.Presentation.User.Client.Helpers;
using Windows.UI.Xaml.Data;

namespace Spp.Presentation.User.Client.Converters
{
    public class DepthToTextConverter : IValueConverter
    {
        private const string DepthTextResourceKey = "DepthText";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var depthValue = System.Convert.ToInt32(value);
            var depthText = ResourceHelper.GetString(DepthTextResourceKey);

            return $"{depthText} {depthValue}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
