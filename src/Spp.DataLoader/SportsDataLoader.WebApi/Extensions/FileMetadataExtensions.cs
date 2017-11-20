/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using SportsDataLoader.Model;
using SportsDataLoader.WebApi.Models;

namespace SportsDataLoader.WebApi.Extensions
{
    public static class FileMetadataExtensions
    {
        public static FileMetadataModel ToFileMetadataModel(this FileMetadata fileMetadata)
        {
            if (fileMetadata == null)
                throw new ArgumentNullException(nameof(fileMetadata));

            return new FileMetadataModel
            {
                CreatedDateTimeUtc = fileMetadata.CreatedDateTimeUtc,
                FileCultureCode = fileMetadata.FileCultureCode,
                FileDataType = fileMetadata.FileSchemaName,
                FileId = fileMetadata.FileId,
                FileMimeType = fileMetadata.FileMimeType,
                FileName = fileMetadata.FileName,
                FileSize = fileMetadata.FileSize,
                FileStatus = fileMetadata.FileStatus.ToString(),
                LastModifiedDateTimeUtc = fileMetadata.LastModifiedDateTimeUtc,
                TenantId = fileMetadata.TenantId
            };
        }
    }
}