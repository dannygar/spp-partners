/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Globalization;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Models;

namespace SportsDataLoader.FileProcessing.Interfaces
{
    public interface IDataTableScrubber
    {
        Task<DataTable> ScrubDataTable(DataTable dataTable, CultureInfo cultureInfo = null);
    }
}