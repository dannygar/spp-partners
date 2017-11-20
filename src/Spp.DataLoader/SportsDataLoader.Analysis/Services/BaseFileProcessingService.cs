/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Threading.Tasks;
using Serilog;
using SportsDataLoader.FileManagement.Interfaces;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Enumerations;
using SportsDataLoader.Model.Interfaces;

namespace SportsDataLoader.FileProcessing.Services
{
    public abstract class BaseFileProcessingService
    {
        private readonly IFileMetadataRepository fileMetadataRepository;
        private readonly IFileRepository fileRepository;

        protected BaseFileProcessingService(IFileMetadataRepository fileMetadataRepository,
                                            IFileRepository fileRepository,
                                            ILogger logger)
        {
            this.fileMetadataRepository = fileMetadataRepository;
            this.fileRepository = fileRepository;

            Logger = logger;
        }

        protected ILogger Logger { get; }

        protected virtual async Task<FileMetadata> OnProcessed(FileMetadata fileMetadata)
        {
            Logger.Debug($"File [{fileMetadata}] processed.",
                         fileMetadata);

            await DeleteFile(fileMetadata);

            return await UpdateFileStatus(fileMetadata, FileStatus.Processed);
        }

        protected virtual async Task<FileMetadata> OnProcessingFailed(FileMetadata fileMetadata)
        {
            Logger.Debug("File [{fileMetadata}] processing failed.",
                         fileMetadata);

            return await UpdateFileStatus(fileMetadata, FileStatus.ProcessingFailed);
        }

        protected virtual async Task<FileMetadata> OnProcessing(FileMetadata fileMetadata)
        {
            Logger.Debug("Processing file [{fileMetadata}]...",
                         fileMetadata);

            return await UpdateFileStatus(fileMetadata, FileStatus.Processing);
        }

        private async Task<FileMetadata> UpdateFileStatus(FileMetadata fileMetadata, FileStatus fileStatus)
        {
            Logger.Debug("Changing file [{fileMetadata}] status from [{originalFileStatus}] to [{newFileStatus}]...",
                         fileMetadata, fileMetadata.FileStatus, fileStatus);

            fileMetadata.FileStatus = fileStatus;
            fileMetadata.LastModifiedDateTimeUtc = DateTime.UtcNow;

            await fileMetadataRepository.UpsertFileMetadata(fileMetadata);

            return fileMetadata;
        }

        private async Task DeleteFile(FileMetadata fileMetadata)
        {
            await fileRepository.DeleteFileAsync(fileMetadata.TenantId, fileMetadata.FileId);
        }
    }
}