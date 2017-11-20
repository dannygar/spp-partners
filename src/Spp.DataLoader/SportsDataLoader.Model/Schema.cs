/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SportsDataLoader.Model
{
    public class Schema
    {
        public Schema()
        {
            SchemaId = Guid.NewGuid().ToString();
            SchemaColumns = new List<SchemaColumn>();
        }

        [JsonProperty(PropertyName = "id")]
        public string SchemaId { get; set; }

        public string TenantId { get; set; }
        public string SchemaName { get; set; }
        public string SchemaType { get; set; }
        public List<SchemaColumn> SchemaColumns { get; set; }
    }
}