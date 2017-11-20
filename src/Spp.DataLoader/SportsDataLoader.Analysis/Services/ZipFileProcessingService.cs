/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
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
    public class ZipFileProcessingService : BaseFileProcessingService,
                                            IZipFileProcessingService
    {
        private readonly IZipFileParser fileParser;
        private readonly IZipFileProcessor[] fileProcessors;

        public ZipFileProcessingService(IFileMetadataRepository fileMetadataRepository,
                                        IFileRepository fileRepository,
                                        ILogger logger,
                                        IZipFileParser fileParser,
                                        IZipFileProcessor[] fileProcessors)
            : base(fileMetadataRepository,
                   fileRepository,
                   logger)
        {
            this.fileParser = fileParser;
            this.fileProcessors = fileProcessors;
        }

        public async Task Process(FileMetadata fileMetadata, Stream fileStream)
        {
            try
            {
                if (fileMetadata == null)
                    throw new ArgumentNullException(nameof(fileMetadata));

                if (fileStream == null)
                    throw new ArgumentNullException(nameof(fileStream));

                await OnProcessing(fileMetadata);

                var zipFile = await ParseZipFile(fileMetadata, fileStream);

                await ProcessFile(fileMetadata, zipFile);
                await OnProcessed(fileMetadata);
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred while processing file [{fileMetadata}]: [{@ex}].",
                             fileMetadata, ex);

                await OnProcessingFailed(fileMetadata);
            }
        }

        private async Task<ZipFile> ParseZipFile(FileMetadata fileMetadata, Stream fileStream)
        {
            try
            {
                return await fileParser.Parse(fileStream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"File [{fileMetadata}] is not a valid zip file." +
                                                    $"More information: [{ex.Message}].");
            }
        }

        private async Task ProcessFile(FileMetadata fileMetadata, ZipFile zipFile)
        {
            var fileProcessor = fileProcessors
                .OrderByDescending(p => p.Specificity)
                .First(p => p.IsFileCompatible(fileMetadata, zipFile));

            await fileProcessor.Process(fileMetadata, zipFile);
        }
    }
}