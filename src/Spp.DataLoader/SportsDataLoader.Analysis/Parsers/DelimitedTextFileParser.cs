/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Shared.Extensions;

namespace SportsDataLoader.FileProcessing.Parsers
{
    public abstract class DelimitedTextFileParser
    {
        protected DelimitedTextFile Parse(Stream fileStream, string delimiter)
        {
            var delimitedTextFile = new DelimitedTextFile();

            if (fileStream.CanSeek)
                fileStream.Position = 0;

            using (var csvParser = new TextFieldParser(fileStream))
            {
                csvParser.SetDelimiters(delimiter);
                csvParser.HasFieldsEnclosedInQuotes = true;

                while (csvParser.EndOfData == false)
                {
                    var fields = csvParser.ReadFields();

                    if (fields != null)
                        delimitedTextFile.Rows.Add(new DelimitedTextRow(fields));
                }
            }

            return delimitedTextFile;
        }
    }
}