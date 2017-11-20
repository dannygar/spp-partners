/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using SportsDataLoader.Messaging.Interfaces;

namespace SportsDataLoader.Model.Events
{
    public class FileImported : IMessage
    {
        public FileMetadata FileMetadata { get; set; }
        public string Id { get; set; }
        public string TenantId { get; set; }
    }
}