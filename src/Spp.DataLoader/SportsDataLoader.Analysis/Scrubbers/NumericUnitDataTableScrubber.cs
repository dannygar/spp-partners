/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Extensions;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;

namespace SportsDataLoader.FileProcessing.Scrubbers
{
    public class NumericUnitDataTableScrubber : IDataTableScrubber
    {
        public Task<DataTable> ScrubDataTable(DataTable dataTable, CultureInfo cultureInfo = null)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable));

            cultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;

            foreach (var column in dataTable.Columns)
            {
                var dataRows = dataTable
                    .Rows
                    .Where(r => r.ContainsKey(column) &&
                                (string.IsNullOrEmpty(r[column]?.StringValue) == false))
                    .Select(r => new
                    {
                        DataRow = r,
                        Prefix = string.Concat(r[column].StringValue.TakeWhile(MayBePartOfNumber)).Trim(),
                        Suffix = string.Concat(r[column].StringValue.SkipWhile(MayBePartOfNumber)).Trim()
                    })
                    .Where(r => r.Prefix.Length > 0)
                    .ToList();

                if (dataRows.Any() &&
                    dataRows.All(r => ((r.Prefix.TryParseDouble(cultureInfo)) ??
                                       (r.Prefix.TryParseDouble(CultureInfo.InvariantCulture))) != null))
                {
                    var suffixes = dataRows.Select(r => r.Suffix).Distinct().ToList();

                    if ((suffixes.Count == 1) || (MayBeUnit(suffixes[0])))
                    {
                        for (var i = 0; i < dataRows.Count; i++)
                        {
                            var dataRow = dataRows[i];

                            dataRow.DataRow[column] = DataValue.FromString(dataRow.Prefix);
                        }
                    }
                }
            }

            return Task.FromResult(dataTable);
        }

        private bool MayBeUnit(string source)
        {
            return (source.All(c => (char.IsLetter(c)) ||
                                    (c == ' ') ||
                                    (c == '.') ||
                                    (c == '/')));
        }

        private bool MayBePartOfNumber(char source)
        {
            return (char.IsLetter(source) == false) &&
                   (source != '\'');
        }
    }
}