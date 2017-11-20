/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Models;

namespace SportsDataLoader.FileProcessing.Extensions
{
    public static class DelimitedTextRowExtensions
    {
        public static DataValue TryParseDoubleCellValue(this DelimitedTextRow row, int cellIndex,
                                                        CultureInfo cultureInfo = null)
        {
            ValidateRow(row);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromDouble(
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseDouble(cultureInfo) ??
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseDouble(CultureInfo.InvariantCulture));
        }

        public static DataValue TryParsePercentCellValue(this DelimitedTextRow row, int cellIndex,
                                                         CultureInfo cultureInfo = null)
        {
            ValidateRow(row);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            var tempDouble =
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseDouble(cultureInfo) ??
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseDouble(CultureInfo.InvariantCulture);

            if (tempDouble == null)
                return DataValue.FromDouble(null);

            return DataValue.FromDouble(tempDouble / 100);
        }

        public static DataValue TryParseIntegerCellValue(this DelimitedTextRow row, int cellIndex,
                                                         CultureInfo cultureInfo = null)
        {
            ValidateRow(row);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromInteger(
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseInteger(cultureInfo) ??
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseInteger(CultureInfo.InvariantCulture));
        }

        public static DataValue TryParseDateTimeCellValue(this DelimitedTextRow row, int cellIndex,
                                                          CultureInfo cultureInfo = null)
        {
            ValidateRow(row);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromDateTime(
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseDateTime(cultureInfo) ??
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseDateTime(CultureInfo.InvariantCulture));
        }

        public static DataValue TryParseDateTimeOffsetCellValue(this DelimitedTextRow row, int cellIndex,
                                                                CultureInfo cultureInfo = null)
        {
            ValidateRow(row);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromDateTimeOffsetValue(
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseDateTimeOffset(cultureInfo) ??
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseDateTimeOffset(CultureInfo.InvariantCulture));
        }

        public static DataValue TryParseTimeSpanCellValue(this DelimitedTextRow row, int cellIndex,
                                                          CultureInfo cultureInfo = null)
        {
            ValidateRow(row);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromTimeSpan(
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseTimeSpan(cultureInfo) ??
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseTimeSpan(CultureInfo.InvariantCulture));
        }

        public static DataValue TryParseBooleanCellValue(this DelimitedTextRow row, int cellIndex,
                                                         CultureInfo cultureInfo = null)
        {
            ValidateRow(row);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromBoolean(
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseBoolean(cultureInfo) ??
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseBoolean(CultureInfo.InvariantCulture));
        }

        public static DataValue TryParseGuidCellValue(this DelimitedTextRow row, int cellIndex,
                                                      CultureInfo cultureInfo = null)
        {
            ValidateRow(row);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromGuid(
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseGuid(cultureInfo) ??
                row.Cells.ElementAtOrDefault(cellIndex)?.TryParseGuid(CultureInfo.InvariantCulture));
        }

        public static DataValue TryParseStringCellValue(this DelimitedTextRow row, int cellIndex)
        {
            ValidateRow(row);

            return DataValue.FromString(row.Cells.ElementAtOrDefault(cellIndex));
        }

        private static void ValidateRow(DelimitedTextRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));
        }
    }
}