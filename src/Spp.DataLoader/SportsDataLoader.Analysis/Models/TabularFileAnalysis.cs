/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using SportsDataLoader.Model;

namespace SportsDataLoader.FileProcessing.Models
{
    public class TabularFileAnalysis
    {
        public FileMetadata FileMetadata { get; set; }
        public DataTable Model { get; set; }
        public Schema Schema { get; set; }
    }
}