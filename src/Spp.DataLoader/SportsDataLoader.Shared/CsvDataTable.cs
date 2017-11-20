/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using SportsDataLoader.Shared.Interfaces;

namespace SportsDataLoader.Shared
{
    public class CsvDataTable : IDataTable
    {
        public CsvDataTable(Stream csvStream)
        {
            if (csvStream == null)
                throw new ArgumentNullException(nameof(csvStream));

            DataHeaders = new List<string>();
            DataRows = new List<Dictionary<string, string>>();

            ParseCsvStream(csvStream);
        }

        public List<string> DataHeaders { get; set; }
        public List<Dictionary<string, string>> DataRows { get; set; }

        private void ParseCsvStream(Stream csvStream)
        {
            if (csvStream.CanSeek)
                csvStream.Position = 0;

            using (var csvParser = new TextFieldParser(csvStream))
            {
                csvParser.SetDelimiters(",");
                csvParser.HasFieldsEnclosedInQuotes = true;

                while (csvParser.EndOfData == false)
                {
                    var fields = csvParser.ReadFields();

                    if (fields?.Length > 0)
                    {
                        if (DataHeaders.Count == 0)
                        {
                            DataHeaders.AddRange(fields);
                        }
                        else
                        {
                            var dataRow = new Dictionary<string, string>();

                            for (var fi = 0; fi < fields.Length; fi++)
                                dataRow.Add(DataHeaders[fi], fields[fi]);

                            DataRows.Add(dataRow);
                        }
                    }
                }
            }
        }
    }
}