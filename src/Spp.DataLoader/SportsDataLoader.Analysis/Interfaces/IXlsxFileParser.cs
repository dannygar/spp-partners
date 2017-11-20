/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.IO;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Models;

namespace SportsDataLoader.FileProcessing.Interfaces
{
    public interface IXlsxFileParser
    {
        Task<XlsxFile> Parse(Stream fileStream);
    }
}