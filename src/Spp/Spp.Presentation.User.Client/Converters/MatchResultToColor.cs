/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using MicrosoftSportsScience.Constants;

namespace MicrosoftSportsScience.Converters
{
    public class MatchResultToColor : IValueConverter
    {
        /// <summary>
        /// Returns a color value for Match result element at matches history at the NextMatch view.
        /// </summary>
        /// <param name="value">Current match result.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Converter parameter.</param>
        /// <param name="language">A Language.</param>
        /// <returns>A color for Match result title element.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var matchResult = (string)value;

            switch (matchResult)
            {
                case UiConsts.MatchWinTitle:
                    return Application.Current.Resources["MatchResultWinColor"] as Brush;
                case UiConsts.MatchLoseTitle:
                    return Application.Current.Resources["MatchResultLoseColor"] as Brush;
                default:
                    return Application.Current.Resources["MatchResultDrawColor"] as Brush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
