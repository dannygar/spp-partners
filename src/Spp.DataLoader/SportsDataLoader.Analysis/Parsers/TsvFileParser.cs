/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.IO;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;

namespace SportsDataLoader.FileProcessing.Parsers
{
    public class TsvFileParser : DelimitedTextFileParser, ITsvFileParser
    {
        public Task<DelimitedTextFile> Parse(Stream fileStream)
        {
            return Task.FromResult(Parse(fileStream, "\t"));
        }
    }
}