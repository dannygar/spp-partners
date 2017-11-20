/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace SportsDataLoader.Web.Models
{
    public class FileMetadataViewModel
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileSchemaName { get; set; }
        public string FileStatusClass { get; set; }
        public string FileStatusDescription { get; set; }
        public string FileStatusIcon { get; set; }
        public string FileUploadDateTime { get; set; }
    }
}