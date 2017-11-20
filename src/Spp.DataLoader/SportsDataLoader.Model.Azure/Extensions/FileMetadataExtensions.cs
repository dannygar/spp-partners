/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using SportsDataLoader.Model.Azure.Entities;

namespace SportsDataLoader.Model.Azure.Extensions
{
    public static class FileMetadataExtensions
    {
        public static FileMetadataTableEntity ToFileMetadataTableEntity(this FileMetadata fileMetadata)
        {
            if (fileMetadata == null)
                throw new ArgumentNullException(nameof(fileMetadata));

            return new FileMetadataTableEntity
            {
                CreateDateTimeUtc = fileMetadata.CreatedDateTimeUtc,
                FileCultureCode = fileMetadata.FileCultureCode,
                FileSchemaName = fileMetadata.FileSchemaName,
                FileMimeType = fileMetadata.FileMimeType,
                FileName = fileMetadata.FileName,
                FileSize = fileMetadata.FileSize,
                FileStatus = ((int) (fileMetadata.FileStatus)),
                LastModifiedDateTimeUtc = fileMetadata.LastModifiedDateTimeUtc,
                PartitionKey = fileMetadata.TenantId,
                RowKey = fileMetadata.FileId
            };
        }
    }
}