/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using SportsDataLoader.Messaging.Interfaces;

namespace SportsDataLoader.Model.Events
{
    public class FileUploaded : IMessage
    {
        public FileUploaded()
        {
            Id = Guid.NewGuid().ToString();
        }

        public FileUploaded(FileMetadata fileMetadata)
        {
            if (fileMetadata == null)
                throw new ArgumentNullException(nameof(fileMetadata));

            Id = Guid.NewGuid().ToString();
            TenantId = fileMetadata.TenantId;
            FileMetadata = fileMetadata;
        }

        public string Id { get; set; }
        public string TenantId { get; set; }

        public FileMetadata FileMetadata { get; set; }
    }
}