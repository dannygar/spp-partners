/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;

namespace SportsDataLoader.FileProcessing.Models
{
    public class DiscoveredRelationship
    {
        public DiscoveredRelationship()
        {
            RelatedRecordIds = new Dictionary<Guid, int>();
        }

        public Type PrimaryKeyType { get; set; }
        public Dictionary<Guid, int> RelatedRecordIds { get; set; }
    }

    public class PossibleRelationship<T> : DiscoveredRelationship
    {
        public PossibleRelationship()
        {
            PrimaryKeyType = typeof(T);
        }
    }
}