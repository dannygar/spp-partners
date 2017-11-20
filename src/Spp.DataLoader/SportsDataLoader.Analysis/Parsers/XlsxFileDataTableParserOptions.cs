/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace SportsDataLoader.FileProcessing.Parsers
{
    public class XlsxFileDataTableParserOptions
    {
        public int? HeaderRowIndex { get; set; }
        public string FirstColumnName { get; set; }
        public string LastColumnName { get; set; }
    }
}