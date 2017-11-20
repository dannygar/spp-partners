/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SportsDataLoader.Shared.Extensions
{
    public static class StringExtensions
    {
        public static CultureInfo TryParseCultureInfo(this string cultureCode)
        {
            if (cultureCode == null)
                throw new ArgumentNullException(nameof(cultureCode));

            try
            {
                return CultureInfo.GetCultureInfo(cultureCode);
            }
            catch
            {
                return null;
            }
        }

        public static string[] Split(this string source, Func<char, bool> splitOn)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            source = source.Trim();

            var stringParts = new List<string>();
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < source.Length; i++)
            {
                var currentChar = source[i];

                if (splitOn(currentChar))
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringParts.Add(stringBuilder.ToString());
                        stringBuilder.Clear();
                    }
                }
                else
                {
                    stringBuilder.Append(currentChar);
                }
            }

            if (stringBuilder.Length > 0)
                stringParts.Add(stringBuilder.ToString());

            return stringParts.ToArray();
        }

        public static TokenizedString Tokenize(this string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new TokenizedString(source);
        }
    }
}