/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsDataLoader.Model.Interfaces
{
    public interface IFileMetadataRepository
    {
        Task UpsertAllFileMetadata(IEnumerable<FileMetadata> fileMetadata);
        Task UpsertFileMetadata(FileMetadata fileMetadata);
        Task DeleteAllTenantFileMetadata(string tenantId);
        Task DeleteFileMetadata(string tenantId, string fileId);
        Task<IEnumerable<FileMetadata>> GetAllTenantFileMetadata(string tenantId);
        Task<FileMetadata> GetFileMetadata(string tenantId, string fileId);
    }
}
