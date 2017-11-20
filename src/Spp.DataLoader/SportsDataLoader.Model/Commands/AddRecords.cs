/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;
using SportsDataLoader.Messaging.Interfaces;

namespace SportsDataLoader.Model.Commands
{
    public class AddRecords<T> : IMessage
    {
        public AddRecords()
        {
            Records = new List<T>();
        }

        public string Id { get; set; }

        public List<T> Records { get; set; }
        public string TenantId { get; set; }
    }
}