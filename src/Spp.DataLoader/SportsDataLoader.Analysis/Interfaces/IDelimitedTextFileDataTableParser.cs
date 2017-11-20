/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Models;

namespace SportsDataLoader.FileProcessing.Interfaces
{
    public interface IDelimitedTextFileDataTableParser
    {
        Task<DataTable> Parse(DelimitedTextFile delimitedTextFile,
                              int headerRowIndex = 0);
    }
}