/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;
using SportsDataLoader.Model;

namespace SportsDataLoader.FileProcessing.Models
{
    public class ImportMetadata<T>
    {
        public string ImportId { get; set; }
        public string TenantId { get; set; }
        public IEnumerable<T> Entities { get; set; }
        public Schema Schema { get; set; }
    }
}