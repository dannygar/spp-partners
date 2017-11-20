/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.IO;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Enumerations;
using SportsDataLoader.Model.Interfaces;

namespace SportsDataLoader.FileProcessing.Processors
{
    public class ZipFileProcessor : IZipFileProcessor
    {
        private readonly IFileMetadataRepository fileMetadataRepository;
        private readonly IFileProcessingServiceFactory processingServiceFactory;

        public ZipFileProcessor(IFileMetadataRepository fileMetadataRepository,
                                IFileProcessingServiceFactory processingServiceFactory)
        {
            this.fileMetadataRepository = fileMetadataRepository;
            this.processingServiceFactory = processingServiceFactory;
        }

        public int Specificity => 0;

        public async Task Process(FileMetadata fileMetadata, ZipFile zipFile)
        {
            if (fileMetadata == null)
                throw new ArgumentNullException(nameof(fileMetadata));

            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));

            foreach (var entryName in zipFile.Entries.Keys)
            {
                var entryFileStream = zipFile.Entries[entryName];
                var entryFileMetadata = CreateEntryFileMetadata(fileMetadata, entryName, entryFileStream);

                await fileMetadataRepository.UpsertFileMetadata(entryFileMetadata);

                try
                {
                    var processingService = await GetFileProcessingService(entryFileMetadata);

                    await processingService.Process(entryFileMetadata, entryFileStream);
                }
                catch
                {
                    await OnProcessingFailed(entryFileMetadata);
                }
            }
        }

        public bool IsFileCompatible(FileMetadata fileMetadata, ZipFile zipFile)
        {
            return true;
        }

        private FileMetadata CreateEntryFileMetadata(FileMetadata zipFileMetadata, string entryFileName,
                                                     Stream entryFileStream)
        {
            return new FileMetadata
            {
                CreatedDateTimeUtc = zipFileMetadata.CreatedDateTimeUtc,
                FileCultureCode = zipFileMetadata.FileCultureCode,
                FileId = Guid.NewGuid().ToString(),
                FileName = entryFileName,
                FileSize = entryFileStream.Length,
                FileStatus = FileStatus.Processing,
                LastModifiedDateTimeUtc = DateTime.UtcNow,
                TenantId = zipFileMetadata.TenantId
            };
        }

        private async Task<IFileProcessingService> GetFileProcessingService(FileMetadata fileMetadata)
        {
            var processingService = await processingServiceFactory.GetProcessingService(fileMetadata);

            if (processingService == null)
            {
                throw new InvalidOperationException(
                    $"Unable to process file [{fileMetadata}]. " +
                    "There is no compatible file processing service available.");
            }

            return processingService;
        }

        private async Task<FileMetadata> OnProcessingFailed(FileMetadata fileMetadata)
        {
            fileMetadata.FileStatus = FileStatus.ProcessingFailed;
            fileMetadata.LastModifiedDateTimeUtc = DateTime.UtcNow;

            await fileMetadataRepository.UpsertFileMetadata(fileMetadata);

            return fileMetadata;
        }
    }
}