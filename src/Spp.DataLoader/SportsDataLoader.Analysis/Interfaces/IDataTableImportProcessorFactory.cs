/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Model;

namespace SportsDataLoader.FileProcessing.Interfaces
{
    public interface IDataTableImportProcessorFactory
    {
        Task<IDataTableImportProcessor> GetImportProcessor(DataTableImportMetadata importMetadata);
    }
}