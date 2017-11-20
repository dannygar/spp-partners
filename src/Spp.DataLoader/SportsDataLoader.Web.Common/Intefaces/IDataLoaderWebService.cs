/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using SportsDataLoader.Web.Common.Models;

namespace SportsDataLoader.Web.Common.Intefaces
{
    public interface IDataLoaderWebService
    {
        Task<IEnumerable<FileMetadataModel>> UploadFiles(IEnumerable<HttpContent> files,
                                                         Guid tenantId, CultureInfo cultureInfo);

        Task<IEnumerable<FileMetadataModel>> GetAllFileMetadata(Guid tenantId);
        Task<FileMetadataModel> GetFileMetadata(Guid tenantId, Guid fileId);
    }
}