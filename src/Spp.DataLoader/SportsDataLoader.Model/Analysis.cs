/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SportsDataLoader.Model
{
    public class Analysis
    {
        public Analysis()
        {
            AnalysisId = Guid.NewGuid().ToString();
            Columns = new List<ColumnAnalysis>();
            AnalysisDateTimeUtc = DateTime.UtcNow;
        }

        [JsonProperty(PropertyName = "id")]
        public string AnalysisId { get; set; }

        public string TenantId { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public List<ColumnAnalysis> Columns { get; set; }
        public DateTime AnalysisDateTimeUtc { get; set; }
    }
}