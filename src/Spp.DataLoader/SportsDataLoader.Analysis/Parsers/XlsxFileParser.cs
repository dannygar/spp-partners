/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SportsDataLoader.FileProcessing.Extensions;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;

namespace SportsDataLoader.FileProcessing.Parsers
{
    public class XlsxFileParser : IXlsxFileParser
    {
        private static readonly uint[] dateTimeFormatIds =
        {
            14, 15, 16, 17, 18, 19, 20, 21, 22, 27, 28, 29,
            30, 31, 32, 33, 34, 35, 36, 45, 46, 47, 50, 51, 52, 53, 54, 55, 56, 57, 58
        };

        public Task<XlsxFile> Parse(Stream stream)
        {
            var xlsxFile = new XlsxFile();

            using (var xlsxDocument = SpreadsheetDocument.Open(stream, false))
            {
                var workbookPart = xlsxDocument.WorkbookPart;

                foreach (var worksheet in workbookPart.Workbook.Sheets.Elements<Sheet>())
                {
                    var worksheetPart = workbookPart.GetPartById(worksheet.Id) as WorksheetPart;

                    if (worksheetPart != null)
                    {
                        var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                        var xlsxSheet = new XlsxSheet(
                            sheetData.Elements<Row>().Select(r => ToRow(workbookPart, r.Elements<Cell>())));

                        xlsxFile.Sheets.Add(worksheet.Name, xlsxSheet);
                    }
                }
            }

            return Task.FromResult(xlsxFile);
        }

        private XlsxRow ToRow(WorkbookPart workbookPart, IEnumerable<Cell> cells)
        {
            var xlsxRow = new XlsxRow();

            foreach (var cell in cells)
            {
                var cellReference = new CellReference(cell.CellReference);
                var dataValue = GetDataTableValue(workbookPart, cell);

                xlsxRow.Cells.Add(cellReference.ColumnName, dataValue);
            }

            return xlsxRow;
        }

        private DataValue GetDataTableValue(WorkbookPart workbook, Cell cell)
        {
            var cellDataType = cell.DataType?.Value;
            var cellValue = ((cell.CellValue?.InnerText) ?? (cell.InnerText));
            var sharedStringTablePart = workbook.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

            if (cellDataType == CellValues.Boolean)
                return DataValue.FromBoolean(cellValue != "0");

            if (IsDateCell(workbook, cell))
                return GetDateTimeCellValue(cellValue);

            if (cellDataType == CellValues.Number)
                return GetNumberCellValue(cellValue);

            if (cellDataType == CellValues.SharedString)
                return DataValue.FromString(GetSharedStringValue(sharedStringTablePart, cellValue));

            return DataValue.FromString(cellValue);
        }

        private DataValue GetDateTimeCellValue(string cellValue)
        {
            var oaDate = cellValue.TryParseDouble();

            if (oaDate == null)
                return DataValue.FromDateTime(null);

            return DataValue.FromDateTime(DateTime.FromOADate(oaDate.Value));
        }

        private DataValue GetNumberCellValue(string cellValue)
        {
            var intValue = cellValue.TryParseInteger(CultureInfo.InvariantCulture);

            if (intValue != null)
                return DataValue.FromInteger(intValue);

            return DataValue.FromDouble(cellValue.TryParseDouble(CultureInfo.InvariantCulture));
        }

        private string GetSharedStringValue(SharedStringTablePart sharedStringTablePart, string cellValue)
        {
            return sharedStringTablePart?.SharedStringTable
                                         .ElementAt(int.Parse(cellValue))
                                         .InnerText;
        }

        private bool IsDateCell(WorkbookPart workbookPart, Cell cell)
        {
            if (cell.DataType?.Value == CellValues.Date)
                return true;

            if (cell.StyleIndex != null)
            {
                var cellFormat =
                    workbookPart.WorkbookStylesPart?
                                .Stylesheet?
                                .CellFormats?
                                .ElementAt((int) (cell.StyleIndex.Value)) as CellFormat;

                var numberFormatId = cellFormat?.NumberFormatId;

                return (numberFormatId != null) &&
                       (dateTimeFormatIds.Contains(numberFormatId.Value));
            }

            return false;
        }

        public class CellReference
        {
            public CellReference(string cellReference)
            {
                Parse(cellReference);
            }

            public string ColumnName { get; set; }
            public int RowNumber { get; set; }

            public void Parse(string cellReference)
            {
                ColumnName = string.Concat(cellReference.TakeWhile(char.IsLetter));
                RowNumber = int.Parse(cellReference.Substring(ColumnName.Length));
            }
        }
    }
}