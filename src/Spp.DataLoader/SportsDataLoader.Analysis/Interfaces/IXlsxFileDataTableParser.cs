/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.FileProcessing.Parsers;

namespace SportsDataLoader.FileProcessing.Interfaces
{
    public interface IXlsxFileDataTableParser
    {
        Task<IEnumerable<DataTable>> Parse(XlsxFile xlsxFile, XlsxFileDataTableParserOptions options = null);
    }
}