/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Globalization;
using System.IO;
using SportsDataLoader.Model.Enumerations;

namespace SportsDataLoader.FileProcessing.Extensions
{
    public static class StringExtensions
    {
        public static DataType SqlDataTypeNameToDataType(this string source)
        {
            source = source.ToLower();

            switch (source)
            {
                case "uniqueidentifier":
                    return DataType.Guid;
                case "int":
                    return DataType.Integer;
                case "float":
                case "decimal":
                case "real":
                    return DataType.Double;
                case "datetime":
                    return DataType.DateTime;
                case "datetimeoffset":
                    return DataType.DateTimeOffset;
                default:
                    return DataType.String;
            }
        }

        public static string GetFileExtension(this string source)
        {
            return Path.GetExtension(source)?.ToLower().Trim('.');
        }

        public static decimal? TryParseDecimal(this string source, CultureInfo cultureInfo = null)
        {
            if (string.IsNullOrEmpty(source) == false)
            {
                cultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;

                var isPercent = source.Contains(cultureInfo.NumberFormat.PercentSymbol);

                if (isPercent)
                    source = source.Replace(cultureInfo.NumberFormat.PercentSymbol, string.Empty);

                decimal tempDecimal;

                if (decimal.TryParse(source, NumberStyles.Any, cultureInfo.NumberFormat, out tempDecimal))
                    return isPercent ? tempDecimal / 100 : tempDecimal;
            }

            return null;
        }

        public static double? TryParseDouble(this string source, CultureInfo cultureInfo = null)
        {
            if (string.IsNullOrEmpty(source) == false)
            {
                cultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;

                var isPercent = source.Contains(cultureInfo.NumberFormat.PercentSymbol);

                if (isPercent)
                    source = source.Replace(cultureInfo.NumberFormat.PercentSymbol, string.Empty);

                double tempDouble;

                if (double.TryParse(source, NumberStyles.Any, cultureInfo.NumberFormat, out tempDouble))
                    return isPercent ? tempDouble/100 : tempDouble;
            }

            return null;
        }

        public static int GetExcelColumnIndexByName(this string columnName)
        {
            var number = 0;
            var pow = 1;

            for (var i = (columnName.Length - 1); i >= 0; i--)
            {
                number += (columnName[i] - 'A' + 1)*pow;
                pow *= 26;
            }

            return number;
        }

        public static int? TryParseInteger(this string source, CultureInfo cultureInfo = null)
        {
            const NumberStyles numberStyles = NumberStyles.AllowExponent |
                                              NumberStyles.AllowLeadingWhite |
                                              NumberStyles.AllowTrailingWhite |
                                              NumberStyles.AllowLeadingSign |
                                              NumberStyles.AllowTrailingSign |
                                              NumberStyles.AllowParentheses |
                                              NumberStyles.AllowThousands;

            if (string.IsNullOrEmpty(source) == false)
            {
                cultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;

                int tempInt;

                if (int.TryParse(source, numberStyles, cultureInfo.NumberFormat, out tempInt))
                    return tempInt;
            }

            return null;
        }

        public static DateTime? TryParseDateTime(this string source, CultureInfo cultureInfo = null)
        {
            if (string.IsNullOrEmpty(source) == false)
            {
                cultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;

                DateTime tempDateTime;

                if (DateTime.TryParse(source, cultureInfo.DateTimeFormat, DateTimeStyles.None,
                                      out tempDateTime))
                    return tempDateTime;
            }

            return null;
        }

        public static DateTimeOffset? TryParseDateTimeOffset(this string source, CultureInfo cultureInfo = null)
        {
            if (string.IsNullOrEmpty(source) == false)
            {
                cultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;

                DateTimeOffset tempDateTimeOffset;

                if (DateTimeOffset.TryParse(source, cultureInfo.DateTimeFormat, DateTimeStyles.None,
                                            out tempDateTimeOffset))
                {
                    return tempDateTimeOffset;
                }
            }

            return null;
        }

        public static TimeSpan? TryParseTimeSpan(this string source, CultureInfo cultureInfo = null)
        {
            if (string.IsNullOrEmpty(source) == false)
            {
                cultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;

                TimeSpan tempTimeSpan;

                if (source.Contains(cultureInfo.DateTimeFormat.TimeSeparator) &&
                    TimeSpan.TryParse(source, cultureInfo.DateTimeFormat, out tempTimeSpan))
                    return tempTimeSpan;
            }

            return null;
        }

        public static bool? TryParseBoolean(this string source, CultureInfo cultureInfo = null)
        {
            if (string.IsNullOrEmpty(source) == false)
            {
                bool tempBool;

                if (bool.TryParse(source, out tempBool))
                    return tempBool;
            }

            return null;
        }

        public static Guid? TryParseGuid(this string source, CultureInfo cultureInfo = null)
        {
            if (string.IsNullOrEmpty(source) == false)
            {
                source = source.Trim('{', '}');

                Guid tempGuid;

                if (Guid.TryParse(source, out tempGuid))
                    return tempGuid;
            }

            return null;
        }
    }
}