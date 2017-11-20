/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Extensions;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Shared.Extensions;

namespace SportsDataLoader.FileProcessing.Parsers
{
    public class SheetModel
    {
        public SheetModel(string sheetName)
        {
            SheetName = sheetName;

            Columns = new Dictionary<string, string>();
            Rows = new List<Dictionary<string, DataValue>>();
        }

        public string SheetName { get; set; }
        public Dictionary<string, string> Columns { get; set; }
        public List<Dictionary<string, DataValue>> Rows { get; set; }
    }

    public class XlsxFileDataTableParser : IXlsxFileDataTableParser
    {
        public async Task<IEnumerable<DataTable>> Parse(XlsxFile xlsxFile,
                                                        XlsxFileDataTableParserOptions options = null)
        {
            var dataTables = new List<DataTable>();
            var headerRowIndex = (options?.HeaderRowIndex ?? 0);

            foreach (var sheetName in xlsxFile.Sheets.Keys)
            {
                var sheet = xlsxFile.Sheets[sheetName];
                var sheetModel = new SheetModel(sheetName);

                for (var i = headerRowIndex; i < sheet.Rows.Count; i++)
                {
                    var row = sheet.Rows[i];

                    if (sheetModel.Columns.None())
                        sheetModel.Columns = ToColumns(row, options);
                    else
                        sheetModel.Rows.Add(ToRow(row, sheetModel));
                }

                dataTables.Add(ToDataTable(sheetModel));
            }

            return dataTables;
        }

        private DataTable ToDataTable(SheetModel sheetModel)
        {
            return new DataTable(sheetModel.SheetName)
            {
                Columns = sheetModel.Columns.Values.ToList(),
                Rows = sheetModel.Rows
            };
        }

        private Dictionary<string, DataValue> ToRow(XlsxRow row, SheetModel sheetModel)
        {
            var rowDictionary = new Dictionary<string, DataValue>();

            foreach (var columnKey in row.Cells.Keys)
            {
                if (sheetModel.Columns.ContainsKey(columnKey))
                    rowDictionary[sheetModel.Columns[columnKey]] = row.Cells[columnKey];
            }

            return rowDictionary;
        }

        private Dictionary<string, string> ToColumns(XlsxRow row, XlsxFileDataTableParserOptions options)
        {
            var firstColumnIndex = options?.FirstColumnName.GetExcelColumnIndexByName();
            var lastColumnIndex = options?.FirstColumnName.GetExcelColumnIndexByName();

            var columnNames = new List<string>();
            var columnDictionary = new Dictionary<string, string>();

            foreach (var columnKey in row.Cells.Keys)
            {
                if (((firstColumnIndex == null) ||
                     (firstColumnIndex <= columnKey.GetExcelColumnIndexByName())) &&
                    ((lastColumnIndex == null) ||
                     (lastColumnIndex >= columnKey.GetExcelColumnIndexByName())))
                {
                    var cell = row.Cells[columnKey];
                    var columnName = (string.IsNullOrEmpty(cell.StringValue) ? $"Column_{columnKey}" : cell.StringValue);

                    var columnNameCount = columnNames.Count(
                        c => (string.Equals(c, columnName, StringComparison.CurrentCultureIgnoreCase)));

                    columnNames.Add(columnName);

                    columnDictionary.Add(columnKey,
                                         columnNameCount > 0 ? $"{columnName}_{columnNameCount}" : columnName);
                }
            }

            return columnDictionary;
        }
    }
}