/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using SportsDataLoader.Model.Enumerations;

namespace SportsDataLoader.Model
{
    public class SchemaColumn
    {
        public SchemaColumn()
        {
            Id = Guid.NewGuid().ToString();
            LocalizedNames = new Dictionary<string, string>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public DataType DataType { get; set; }
        public string SqlDataType { get; set; }
        public Dictionary<string, string> LocalizedNames { get; set; }
    }
}