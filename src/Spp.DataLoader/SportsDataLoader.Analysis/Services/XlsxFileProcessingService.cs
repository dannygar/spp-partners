/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using SportsDataLoader.FileManagement.Interfaces;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Interfaces;

namespace SportsDataLoader.FileProcessing.Services
{
    public class XlsxFileProcessingService : BaseTabularFileProcessingService,
                                             IXlsxFileProcessingService
    {
        private readonly IXlsxFileAnalysisProcessor[] analysisProcessors;
        private readonly IXlsxFileParser fileParser;

        public XlsxFileProcessingService(IXlsxFileAnalysisProcessor[] analysisProcessors,
                                         IFileMetadataRepository fileMetadataRepository,
                                         IFileRepository fileRepository,
                                         IDataTableImportProcessorFactory importProcessorFactory,
                                         IXlsxFileParser fileParser,
                                         ILogger logger)
            : base(fileMetadataRepository,
                   fileRepository,
                   importProcessorFactory,
                   logger.ForContext<XlsxFileProcessingService>())
        {
            this.analysisProcessors = analysisProcessors;
            this.fileParser = fileParser;
        }

        public async Task Process(FileMetadata fileMetadata, Stream fileStream)
        {
            try
            {
                if (fileMetadata == null)
                    throw new ArgumentNullException(nameof(fileMetadata));

                await OnProcessing(fileMetadata);

                var xlsxFile = await ParseXlsxFile(fileMetadata, fileStream);
                var analyses = await AnalyzeFile(fileMetadata, xlsxFile);

                foreach (var analysis in analyses)
                    await ImportFile(analysis);

                await OnProcessed(fileMetadata);
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred while processing file [{fileMetadata}]: [{@ex}].",
                             fileMetadata, ex);

                await OnProcessingFailed(fileMetadata);
            }
        }

        private async Task<XlsxFile> ParseXlsxFile(FileMetadata fileMetadata, Stream fileStream)
        {
            try
            {
                return await fileParser.Parse(fileStream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"File [{fileMetadata}] is not a valid XLSX file. " +
                                                    $"More informtion: [{ex.Message}].");
            }
        }

        private async Task<IEnumerable<TabularFileAnalysis>> AnalyzeFile(FileMetadata fileMetadata, XlsxFile xlsxFile)
        {
            var analysisProcessor = analysisProcessors
                .OrderByDescending(ap => ap.Specificity)
                .First(ap => ap.IsFileCompatible(fileMetadata, xlsxFile));

            return await analysisProcessor.Process(fileMetadata, xlsxFile);
        }
    }
}