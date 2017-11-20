/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// ------------------------------------------------------
// <copyright file="ComboBoxItemConvert.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>
// ------------------------------------------------------

namespace MicrosoftSportsScience.Converters
{
    using System;
    using Windows.UI.Xaml.Data;

    public class ComboBoxItemConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}