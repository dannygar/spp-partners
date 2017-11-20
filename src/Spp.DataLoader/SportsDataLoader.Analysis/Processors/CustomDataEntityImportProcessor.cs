/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Model;

namespace SportsDataLoader.FileProcessing.Processors
{
    public class CustomDataEntityImportProcessor : IDataTableImportProcessor
    {
        private readonly IImporter<CustomDataEntity> importer;

        public CustomDataEntityImportProcessor(IImporter<CustomDataEntity> importer)
        {
            this.importer = importer;
        }

        public async Task ImportDataTable(DataTableImportMetadata importMetadata)
        {
            if (importMetadata == null)
                throw new ArgumentNullException(nameof(importMetadata));

            await importer.Import(new ImportMetadata<CustomDataEntity>
            {
                Entities = importMetadata.DataTable.Rows.Select(ToCustomDataEntity).ToList(),
                ImportId = importMetadata.ImportId,
                Schema = importMetadata.Schema,
                TenantId = importMetadata.TenantId
            });
        }

        private CustomDataEntity ToCustomDataEntity(Dictionary<string, DataValue> dataRow)
        {
            return new CustomDataEntity
            {
                Columns = dataRow.ToDictionary(dr => dr.Key,
                                               dr => ToCustomDataValue(dr.Value)),
                CreatedDateTimeUtc = DateTime.UtcNow,
                EntityId = Guid.NewGuid(),
                LastModifiedDateTimeUtc = DateTime.UtcNow
            };
        }

        private CustomDataValue ToCustomDataValue(DataValue dataValue)
        {
            return new CustomDataValue
            {
                BooleanValue = dataValue.BooleanValue,
                DateTimeOffsetValue = dataValue.DateTimeOffsetValue,
                DateTimeValue = dataValue.DateTimeValue,
                DoubleValue = dataValue.DoubleValue,
                GuidValue = dataValue.GuidValue,
                IntegerValue = dataValue.IntegerValue,
                StringValue = dataValue.StringValue,
                TimeSpanValue = dataValue.TimeSpanValue
            };
        }
    }
}