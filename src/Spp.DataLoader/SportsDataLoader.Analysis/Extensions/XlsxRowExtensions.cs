/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Globalization;
using SportsDataLoader.FileProcessing.Models;

namespace SportsDataLoader.FileProcessing.Extensions
{
    public static class XlsxRowExtensions
    {
        public static DataValue TryParseDoubleCellValue(this XlsxRow row, string cellName,
                                                        CultureInfo cultureInfo = null)
        {
            ValidateRow(row);
            ValidateCellName(cellName);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromDouble(row.Cells.ContainsKey(cellName)
                ? (row.Cells[cellName].ToString().TryParseDouble(cultureInfo) ??
                   row.Cells[cellName].ToString().TryParseDouble(CultureInfo.InvariantCulture))
                : null);
        }

        public static DataValue TryParseIntegerCellValue(this XlsxRow row, string cellName,
                                                         CultureInfo cultureInfo = null)
        {
            ValidateRow(row);
            ValidateCellName(cellName);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromInteger(row.Cells.ContainsKey(cellName)
                ? (row.Cells[cellName].ToString().TryParseInteger(cultureInfo) ??
                   row.Cells[cellName].ToString().TryParseInteger(CultureInfo.InvariantCulture))
                : null);
        }

        public static DataValue TryParsePercentCellValue(this XlsxRow row, string cellName,
                                                         CultureInfo cultureInfo = null)
        {
            ValidateRow(row);
            ValidateCellName(cellName);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            if (row.Cells.ContainsKey(cellName))
            {
                var tempDouble = (row.Cells[cellName].ToString().TryParseDouble(cultureInfo) ??
                                  row.Cells[cellName].ToString().TryParseDouble(CultureInfo.InvariantCulture));

                if (tempDouble != null)
                    return DataValue.FromDouble(tempDouble / 100);
            }

            return DataValue.FromDouble(null);
        }

        public static DataValue TryParseDateTimeCellValue(this XlsxRow row, string cellName,
                                                          CultureInfo cultureInfo = null)
        {
            ValidateRow(row);
            ValidateCellName(cellName);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromDateTime(row.Cells.ContainsKey(cellName)
                ? (row.Cells[cellName].ToString().TryParseDateTime(cultureInfo) ??
                   row.Cells[cellName].ToString().TryParseDateTime(CultureInfo.InvariantCulture))
                : null);
        }

        public static DataValue TryParseDateTimeOffsetCellValue(this XlsxRow row, string cellName,
                                                                CultureInfo cultureInfo = null)
        {
            ValidateRow(row);
            ValidateCellName(cellName);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromDateTimeOffsetValue(row.Cells.ContainsKey(cellName)
                ? (row.Cells[cellName].ToString().TryParseDateTimeOffset(cultureInfo) ??
                   row.Cells[cellName].ToString().TryParseDateTimeOffset(CultureInfo.InvariantCulture))
                : null);
        }

        public static DataValue TryParseTimeSpanCellValue(this XlsxRow row, string cellName,
                                                          CultureInfo cultureInfo = null)
        {
            ValidateRow(row);
            ValidateCellName(cellName);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromTimeSpan(row.Cells.ContainsKey(cellName)
                ? (row.Cells[cellName].ToString().TryParseTimeSpan(cultureInfo) ??
                   row.Cells[cellName].ToString().TryParseTimeSpan(CultureInfo.InvariantCulture))
                : null);
        }

        public static DataValue TryParseBooleanCellValue(this XlsxRow row, string cellName,
                                                         CultureInfo cultureInfo = null)
        {
            ValidateRow(row);
            ValidateCellName(cellName);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromBoolean(row.Cells.ContainsKey(cellName)
                ? (row.Cells[cellName].ToString().TryParseBoolean(cultureInfo) ??
                   row.Cells[cellName].ToString().TryParseBoolean(CultureInfo.InvariantCulture))
                : null);
        }

        public static DataValue TryParseGuidCellValue(this XlsxRow row, string cellName,
                                                      CultureInfo cultureInfo = null)
        {
            ValidateRow(row);
            ValidateCellName(cellName);

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            return DataValue.FromGuid(row.Cells.ContainsKey(cellName)
                ? (row.Cells[cellName].ToString().TryParseGuid(cultureInfo) ??
                   row.Cells[cellName].ToString().TryParseGuid(CultureInfo.InvariantCulture))
                : null);
        }

        public static DataValue TryParseStringCellValue(this XlsxRow row, string cellName)
        {
            ValidateRow(row);
            ValidateCellName(cellName);

            return DataValue.FromString(row.Cells.ContainsKey(cellName)
                ? row.Cells[cellName].ToString()
                : null);
        }

        private static void ValidateCellName(string cellName)
        {
            if (cellName == null)
                throw new ArgumentNullException(nameof(cellName));

            if (cellName.Length == 0)
                throw new ArgumentException($"[{nameof(cellName)}] can not be empty.", cellName);
        }

        private static void ValidateRow(XlsxRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));
        }
    }
}