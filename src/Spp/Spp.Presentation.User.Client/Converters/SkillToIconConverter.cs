/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Converters
{
    using System;
    using MicrosoftSportsScience.Helpers;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media.Imaging;

    public class SkillToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var skill = value as string;

            if (string.IsNullOrWhiteSpace(skill))
            {
                return null;
            }

            return skill.Contains("batting", StringComparison.OrdinalIgnoreCase)
                ? ResourceHelper.GetApplicationResource<BitmapImage>("BatterSkillIcon")
                : skill.Contains("bowling", StringComparison.OrdinalIgnoreCase)
                    ? ResourceHelper.GetApplicationResource<BitmapImage>("BowlerSkillIcon")
                    : skill.Contains("allrounder", StringComparison.OrdinalIgnoreCase)
                        ? ResourceHelper.GetApplicationResource<BitmapImage>("AllRounderSkillIcon")
                        : ResourceHelper.GetApplicationResource<BitmapImage>("WicketKeeperSkillIcon");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
