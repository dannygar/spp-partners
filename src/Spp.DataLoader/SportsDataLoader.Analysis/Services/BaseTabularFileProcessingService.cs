/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Threading.Tasks;
using Serilog;
using SportsDataLoader.FileManagement.Interfaces;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Model.Interfaces;

namespace SportsDataLoader.FileProcessing.Services
{
    public abstract class BaseTabularFileProcessingService : BaseFileProcessingService
    {
        private readonly IDataTableImportProcessorFactory importProcessorFactory;

        protected BaseTabularFileProcessingService(IFileMetadataRepository fileMetadataRepository,
                                                   IFileRepository fileRepository,
                                                   IDataTableImportProcessorFactory importProcessorFactory,
                                                   ILogger logger)
            : base(fileMetadataRepository,
                   fileRepository,
                   logger)
        {
            this.importProcessorFactory = importProcessorFactory;
        }

        protected virtual async Task ImportFile(TabularFileAnalysis fileAnalysis)
        {
            try
            {
                var importMetadata = new DataTableImportMetadata
                {
                    DataTable = fileAnalysis.Model,
                    ImportId = Guid.NewGuid().ToString(),
                    Schema = fileAnalysis.Schema,
                    TenantId = fileAnalysis.FileMetadata.TenantId
                };

                var importProcessor =
                    await importProcessorFactory.GetImportProcessor(importMetadata);

                await importProcessor.ImportDataTable(importMetadata);
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred while importing file [{fileMetadata}]: [{@ex}].",
                             fileAnalysis.FileMetadata, ex);

                throw;
            }
        }
    }
}