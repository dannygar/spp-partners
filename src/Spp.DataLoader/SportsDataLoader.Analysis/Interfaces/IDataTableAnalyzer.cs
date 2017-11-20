/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Model;
using SportsDataLoader.Shared.Interfaces;

namespace SportsDataLoader.FileProcessing.Interfaces
{
    public interface IDataTableAnalyzer
    {
        Task<Analysis> AnalyzeDataTableAsync(DataTable dataTable, FileMetadata fileMetadata);
    }
}