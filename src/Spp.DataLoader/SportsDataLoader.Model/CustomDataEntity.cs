/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;

namespace SportsDataLoader.Model
{
    public class CustomDataEntity
    {
        public CustomDataEntity()
        {
            EntityId = Guid.NewGuid();
            Columns = new Dictionary<string, CustomDataValue>();
            CreatedDateTimeUtc = LastModifiedDateTimeUtc = DateTime.UtcNow;
        }

        public Guid EntityId { get; set; }
        public Dictionary<string, CustomDataValue> Columns { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime LastModifiedDateTimeUtc { get; set; }
    }
}