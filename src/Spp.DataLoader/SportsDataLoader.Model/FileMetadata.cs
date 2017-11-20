/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using SportsDataLoader.Messaging.Interfaces;
using SportsDataLoader.Model.Enumerations;

namespace SportsDataLoader.Model
{
    public class FileMetadata
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileSchemaId { get; set; }
        public string FileSchemaName { get; set; }
        public string FileSchemaType { get; set; }
        public string FileMimeType { get; set; }
        public string FileCultureCode { get; set; }
        public string TenantId { get; set; }
        public long FileSize { get; set; }
        public FileStatus FileStatus { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime LastModifiedDateTimeUtc { get; set; }

        public void SetSchema(Schema schema)
        {
            if (schema == null)
                throw new ArgumentNullException(nameof(schema));

            FileSchemaId = schema.SchemaId;
            FileSchemaName = schema.SchemaName;
            FileSchemaType = schema.SchemaType;
        }

        public override string ToString()
        {
            return $"{TenantId}/{FileId}";
        }
    }
}