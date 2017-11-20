/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;

namespace SportsDataLoader.FileProcessing.Scrubbers
{
    public class JunkValueDataTableScrubber : IDataTableScrubber
    {
        private readonly string[] junkValues;

        public JunkValueDataTableScrubber()
        {
            junkValues = new[] {"-", "n/a", "na"};
        }

        public Task<DataTable> ScrubDataTable(DataTable dataTable, CultureInfo cultureInfo = null)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(DataTable));

            foreach (var row in dataTable.Rows)
            {
                var junkValueKeys = row
                    .Where(c => (c.Value.StringValue != null) &&
                                IsJunkValue(c.Value.StringValue))
                    .Select(c => c.Key)
                    .ToList();

                foreach (var junkValueKey in junkValueKeys)
                    row.Remove(junkValueKey);
            }

            return Task.FromResult(dataTable);
        }

        private bool IsJunkValue(string source)
        {
            source = source.Trim().ToLower();

            return (source.Length == 0) ||
                   junkValues.Contains(source);
        }
    }
}