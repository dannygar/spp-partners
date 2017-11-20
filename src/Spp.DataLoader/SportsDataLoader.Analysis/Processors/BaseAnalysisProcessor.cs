/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Linq;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Extensions;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Enumerations;
using SportsDataLoader.Model.Interfaces;

namespace SportsDataLoader.FileProcessing.Processors
{
    public abstract class BaseAnalysisProcessor
    {
        private readonly ISchemaRepository schemaRepository;

        protected BaseAnalysisProcessor(ISchemaRepository schemaRepository, string schemaName)
        {
            if (schemaName == null)
                throw new ArgumentNullException(nameof(schemaName));

            if (schemaName.Length == 0)
                throw new ArgumentException($"[{nameof(schemaName)}] can not be empty.", nameof(schemaName));

            this.schemaRepository = schemaRepository;

            SchemaName = schemaName;
        }

        public string SchemaName { get; }

        protected abstract Schema CreateSchema(FileMetadata fileMetadata);

        protected virtual DataTable CreateDataTable(Schema schema)
        {
            return new DataTable
            {
                Name = schema.SchemaName,
                Columns = schema.SchemaColumns.Select(sc => sc.Name).ToList()
            };
        }

        protected virtual async Task<Schema> GetSchema(FileMetadata fileMetadata)
        {
            var schema = await schemaRepository.GetSchemaByName(SchemaName);

            if (schema == null)
            {
                schema = CreateSchema(fileMetadata);

                await schemaRepository.UpsertSchema(schema);
            }

            return schema;
        }

        protected SchemaColumn CreateSchemaColumn(string columnName, DataType dataType)
        {
            return new SchemaColumn
            {
                DataType = dataType,
                Id = Guid.NewGuid().ToString(),
                Name = columnName,
                SqlDataType = dataType.ToSqlDataTypeName()
            };
        }
    }
}