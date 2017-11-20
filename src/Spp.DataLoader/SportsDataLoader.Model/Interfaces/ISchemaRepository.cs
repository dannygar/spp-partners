/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsDataLoader.Model.Interfaces
{
    public interface ISchemaRepository
    {
        Task DeleteSchema(string schemaId);
        Task UpsertSchema(Schema schema);
        Task<Schema> GetSchemaById(string schemaId);
        Task<Schema> GetSchemaByName(string schemaName);
        Task<IEnumerable<Schema>> GetAllPublicSchemas();
        Task<IEnumerable<Schema>> GetAllTenantSchemas(string tenantId);
    }
}