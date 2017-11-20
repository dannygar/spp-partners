/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.IO;
using System.Threading.Tasks;

namespace SportsDataLoader.FileManagement.Interfaces
{
    public interface IFileRepository
    {
        Task DeleteFileAsync(string tenantId, string fileId);
        Task DeleteAllTenantFilesAsync(string tenantId);
        Task<Stream> LoadFileAsync(string tenantId, string fileId);
        Task SaveFileAsync(Stream fileContents, string tenantId, string fileId);
    }
}