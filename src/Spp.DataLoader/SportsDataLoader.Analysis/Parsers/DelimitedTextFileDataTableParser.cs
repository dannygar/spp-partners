/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Shared.Extensions;

namespace SportsDataLoader.FileProcessing.Parsers
{
    public class DelimitedTextFileDataTableParser : IDelimitedTextFileDataTableParser
    {
        public Task<DataTable> Parse(DelimitedTextFile delimitedTextFile,
                                     int headerRowIndex = 0)
        {
            if (delimitedTextFile == null)
                throw new ArgumentNullException(nameof(delimitedTextFile));

            var dataTable = new DataTable();

            for (var i = headerRowIndex; i < delimitedTextFile.Rows.Count; i++)
            {
                var delimitedTextRow = delimitedTextFile.Rows[i];

                if (dataTable.Columns.None())
                    dataTable.Columns = GetHeaders(delimitedTextRow).ToList();
                else
                    dataTable.Rows.Add(GetRow(dataTable, delimitedTextRow));
            }

            return Task.FromResult(dataTable);
        }

        private Dictionary<string, DataValue> GetRow(DataTable dataTable,
                                                     DelimitedTextRow delimitedTextRow)
        {
            var dataRow = new Dictionary<string, DataValue>();

            for (var fi = 0; fi < delimitedTextRow.Cells.Count; fi++)
            {
                var column = dataTable.Columns.ElementAtOrDefault(fi);

                if (column != null)
                    dataRow.Add(column, DataValue.FromString(delimitedTextRow.Cells[fi]));
            }

            return dataRow;
        }

        private IEnumerable<string> GetHeaders(DelimitedTextRow delimitedTextRow)
        {
            var originalHeaderNames = new List<string>();

            for (var i = 0; i < delimitedTextRow.Cells.Count; i++)
            {
                var cellText = delimitedTextRow.Cells[i];
                var headerName = string.IsNullOrEmpty(cellText) ? $"Column_{i}" : cellText;

                var headerNameCount = originalHeaderNames.Count(
                    n => string.Equals(n, headerName, StringComparison.InvariantCultureIgnoreCase));

                originalHeaderNames.Add(headerName);

                yield return ((headerNameCount > 0) ? $"{headerName}_{headerNameCount}" : headerName);
            }
        }
    }
}