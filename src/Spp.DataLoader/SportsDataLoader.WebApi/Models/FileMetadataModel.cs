/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Enumerations;

namespace SportsDataLoader.WebApi.Models
{
    public class FileMetadataModel
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileDataType { get; set; }
        public string FileMimeType { get; set; }
        public string FileCultureCode { get; set; }
        public string TenantId { get; set; }
        public long FileSize { get; set; }
        public string FileStatus { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime LastModifiedDateTimeUtc { get; set; }
    }
}