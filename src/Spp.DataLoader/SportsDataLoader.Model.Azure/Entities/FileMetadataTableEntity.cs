/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using Microsoft.WindowsAzure.Storage.Table;
using SportsDataLoader.Model.Enumerations;

namespace SportsDataLoader.Model.Azure.Entities
{
    public class FileMetadataTableEntity : TableEntity
    {
        public string FileName { get; set; }
        public string FileSchemaName { get; set; }
        public string FileMimeType { get; set; }
        public string FileCultureCode { get; set; }
        public long FileSize { get; set; }
        public int FileStatus { get; set; }
        public DateTime CreateDateTimeUtc { get; set; }
        public DateTime LastModifiedDateTimeUtc { get; set; }
    }
}