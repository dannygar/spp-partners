/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;

namespace SportsDataLoader.FileProcessing.Models
{
    public class DataTable
    {
        public DataTable(string name = null)
        {
            Name = name;
            Columns = new List<string>();
            Rows = new List<Dictionary<string, DataValue>>();
        }

        public string Name { get; set; }
        public List<string> Columns { get; set; }
        public List<Dictionary<string, DataValue>> Rows { get; set; }
    }
}