/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;

namespace SportsDataLoader.Web.Models
{
    public class TenantMetadataViewModel
    {
        public TenantMetadataViewModel()
        {
            FileMetadataList = new List<FileMetadataViewModel>();
        }

        public string CultureCode { get; set; }
        public List<FileMetadataViewModel> FileMetadataList { get; set; }
        public Guid TenantId { get; set; }
    }
}