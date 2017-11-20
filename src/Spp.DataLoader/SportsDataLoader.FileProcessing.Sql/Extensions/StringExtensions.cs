/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Text;

namespace SportsDataLoader.FileProcessing.Sql.Extensions
{
    public static class StringExtensions
    {
        public static string ToSqlIdentifier(this string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            source = ReplaceCommonSymbols(source.Trim());

            if (source.Length == 0)
                throw new ArgumentException($"[{nameof(source)}] can not be empty.", nameof(source));

            var nameBuilder = new StringBuilder();

            for (var i = 0; i < source.Length; i++)
            {
                if ((char.IsLetter(source[i])) ||
                    (source[i] == '_') ||
                    (source[i] == '#'))
                {
                    nameBuilder.Append(source[i]);
                }
                else if (nameBuilder.Length > 0)
                {
                    if ((char.IsDigit(source[i])) ||
                        (source[i] == '@') ||
                        (source[i] == '$'))
                    {
                        nameBuilder.Append(source[i]);
                    }
                    else if (source[i] == ' ')
                    {
                        nameBuilder.Append('_');
                    }
                }
            }

            if (nameBuilder.Length <= 128)
                return nameBuilder.ToString();

            return nameBuilder.ToString().Substring(0, 128);
        }

        public static string ReplaceCommonSymbols(string source)
        {
            return source.Replace("#", "Number")
                         .Replace("%", "Percent")
                         .Replace("@", "At");
        }
    }
}