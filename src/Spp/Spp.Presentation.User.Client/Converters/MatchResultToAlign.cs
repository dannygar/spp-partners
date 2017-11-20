/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Converters
{
    using System;
    using MicrosoftSportsScience.Constants;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class MatchResultToAlign : IValueConverter
    {
        /// <summary>
        /// Returns an align value for Match result element at matches history at the NextMatch view.
        /// </summary>
        /// <param name="value">Current match result.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Match result column's height.</param>
        /// <param name="language">A Language.</param>
        /// <returns>An align for Match result title element.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var matchResult = (string)value;
            var height = int.Parse(parameter.ToString());

            if (matchResult == UiConsts.MatchWinTitle)
            {
                return new Thickness(0, 0, 0, 0);
            }

            if (matchResult == UiConsts.MatchDrawTitle1)
            {
                return new Thickness(0, height / 2, 0, 0);
            }

            if (matchResult == UiConsts.MatchDrawTitle2)
            {
                return new Thickness(0, height / 2, 0, 0);
            }

            return new Thickness(0, height, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
