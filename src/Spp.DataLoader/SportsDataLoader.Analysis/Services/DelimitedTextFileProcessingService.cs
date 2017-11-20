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
    public class DelimitedTextFileProcessingService<TParser> : BaseTabularFileProcessingService,
                                                               IDelimitedTextFileProcessingService<TParser>
        where TParser : IDelimitedTextFileParser
    {
        private readonly IDelimitedTextFileAnalysisProcessor[] analysisProcessors;
        private readonly TParser fileParser;

        public DelimitedTextFileProcessingService(IDelimitedTextFileAnalysisProcessor[] analysisProcessors,
                                                  IFileMetadataRepository fileMetadataRepository,
                                                  IFileRepository fileRepository,
                                                  TParser fileParser,
                                                  IDataTableImportProcessorFactory importProcessorFactory,
                                                  ILogger logger)
            : base(fileMetadataRepository,
                  fileRepository,
                   importProcessorFactory,
                   logger.ForContext<DelimitedTextFileProcessingService<TParser>>())
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

                var delimitedTextFile = await ParseDelimitedTextFile(fileMetadata, fileStream);
                var analyses = await AnalyzeFile(fileMetadata, delimitedTextFile);

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

        private async Task<DelimitedTextFile> ParseDelimitedTextFile(FileMetadata fileMetadata, Stream fileStream)
        {
            try
            {
                return await fileParser.Parse(fileStream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"File [{fileMetadata}] is not a valid file. " +
                                                    $"More informtion: [{ex.Message}].");
            }
        }

        private async Task<IEnumerable<TabularFileAnalysis>> AnalyzeFile(FileMetadata fileMetadata,
                                                                         DelimitedTextFile delimitedTextFile)
        {
            var analysisProcessor = analysisProcessors
                .OrderByDescending(ap => ap.Specificity)
                .First(ap => ap.IsFileCompatible(fileMetadata, delimitedTextFile));

            return await analysisProcessor.Process(fileMetadata, delimitedTextFile);
        }
    }
}