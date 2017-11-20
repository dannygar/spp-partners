/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using SportsDataLoader.Model.Azure.Entities;
using SportsDataLoader.Model.Enumerations;

namespace SportsDataLoader.Model.Azure.Extensions
{
    public static class FileMetadataTableEntityExtensions
    {
        public static FileMetadata ToFileMetadata(this FileMetadataTableEntity tableEntity)
        {
            if (tableEntity == null)
                throw new ArgumentNullException(nameof(tableEntity));

            return new FileMetadata
            {
                CreatedDateTimeUtc = tableEntity.CreateDateTimeUtc,
                FileCultureCode = tableEntity.FileCultureCode,
                FileSchemaName = tableEntity.FileSchemaName,
                FileId = tableEntity.RowKey,
                FileMimeType = tableEntity.FileMimeType,
                FileName = tableEntity.FileName,
                FileSize = tableEntity.FileSize,
                FileStatus = ((FileStatus) (tableEntity.FileStatus)),
                LastModifiedDateTimeUtc = tableEntity.LastModifiedDateTimeUtc,
                TenantId = tableEntity.PartitionKey
            };
        }
    }
}