/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Threading.Tasks;

namespace SportsDataLoader.Model.Interfaces
{
    public interface IModelStagingRepository
    {
        Task DeleteAllTenantModels(string tenantId);
        Task DeleteModel(string tenantId, string modelId);
        Task UpsertModel<T>(T model, string tenantId, string modelId);
        Task<T> GetModel<T>(string tenantId, string modelId);
    }
}