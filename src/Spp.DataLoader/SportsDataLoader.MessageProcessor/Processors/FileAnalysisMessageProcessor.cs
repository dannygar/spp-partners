/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.IO;
using System.Threading.Tasks;
using Serilog;
using SportsDataLoader.FileManagement.Interfaces;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.Messaging.Interfaces;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Enumerations;
using SportsDataLoader.Model.Events;
using SportsDataLoader.Model.Interfaces;

namespace SportsDataLoader.MessageProcessor.Processors
{
    public class FileAnalysisMessageProcessor : IMessageProcessor<FileUploaded>
    {
        private readonly IFileMetadataRepository fileMetadataRepository;
        private readonly IFileRepository fileRepository;
        private readonly ILogger logger;
        private readonly IFileProcessingServiceFactory serviceFactory;

        public FileAnalysisMessageProcessor(IFileMetadataRepository fileMetadataRepository,
                                            IFileRepository fileRepository,
                                            ILogger logger,
                                            IFileProcessingServiceFactory serviceFactory)
        {
            this.fileMetadataRepository = fileMetadataRepository;
            this.fileRepository = fileRepository;
            this.logger = logger;
            this.serviceFactory = serviceFactory;
        }

        public async Task ProcessMessage(FileUploaded message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            await ProcessFile(message.FileMetadata);
        }

        protected virtual async Task<Stream> LoadFile(FileMetadata fileMetadata)
        {
            try
            {
                logger.Debug("Loading file [{fileMetadata}]...",
                             fileMetadata);

                var fileStream = await fileRepository.LoadFileAsync(fileMetadata.TenantId, fileMetadata.FileId);

                if (fileStream == null)
                    throw new InvalidOperationException($"[{fileMetadata}] not found.");

                if (fileStream.Length == 0)
                    throw new InvalidOperationException($"[{fileMetadata}] is empty.");

                return fileStream;
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred while loading file [{fileMetadata}]: [{@ex}].",
                             fileMetadata, ex);

                throw;
            }
        }

        private async Task ProcessFile(FileMetadata fileMetadata)
        {
            try
            {
                if (fileMetadata == null)
                    throw new ArgumentNullException(nameof(fileMetadata));

                var service = await serviceFactory.GetProcessingService(fileMetadata);

                if (service == null)
                {
                    throw new InvalidOperationException(
                        $"Unable to process file [{fileMetadata}]. " +
                        "There is no compatible file processing service available.");
                }

                var fileStream = await LoadFile(fileMetadata);

                await service.Process(fileMetadata, fileStream);
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred while processing file [{fileMetadata}]. " +
                             "See exception for more details: [{@ex}].", fileMetadata, ex);

                await OnProcessingFailed(fileMetadata);
            }
            finally
            {
                await fileRepository.DeleteFileAsync(fileMetadata.TenantId, fileMetadata.FileId);
            }
        }

        private async Task OnProcessingFailed(FileMetadata fileMetadata)
        {
            fileMetadata.LastModifiedDateTimeUtc = DateTime.UtcNow;
            fileMetadata.FileStatus = FileStatus.ProcessingFailed;

            await fileMetadataRepository.UpsertFileMetadata(fileMetadata);
        }
    }
}