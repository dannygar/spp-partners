/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using SportsDataLoader.Model.Enumerations;

namespace SportsDataLoader.Model
{
    public class ColumnAnalysis
    {
        public ColumnAnalysis()
        {
            ColumnId = Guid.NewGuid().ToString();
            PossibleColumnDataTypes = new List<DataType>();
        }

        public string ColumnId { get; set; }
        public string ColumnName { get; set; }
        public DataType ColumnDataType { get; set; }
        public List<DataType> PossibleColumnDataTypes { get; set; }
    }
}