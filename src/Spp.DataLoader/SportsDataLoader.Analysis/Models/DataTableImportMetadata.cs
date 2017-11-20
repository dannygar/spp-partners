/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using SportsDataLoader.Model;

namespace SportsDataLoader.FileProcessing.Models
{
    public class DataTableImportMetadata
    {
        public string ImportId { get; set; }
        public string TenantId { get; set; }
        public DataTable DataTable { get; set; }
        public Schema Schema { get; set; }
    }
}