/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SportsDataLoader.FileManagement.Interfaces;

namespace SportsDataLoader.FileManagement.Azure.Repositories
{
    public class AzureBlobFileRepository : IFileRepository
    {
        private readonly CloudBlobClient blobClient;

        public AzureBlobFileRepository(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            blobClient = CreateCloudBlobClient(configuration);
        }

        public async Task DeleteAllTenantFilesAsync(string tenantId)
        {
            ValidateTenantId(tenantId);

            await blobClient.GetContainerReference(GetContainerName(tenantId))
                            .DeleteIfExistsAsync()
                            .ConfigureAwait(false);
        }

        public async Task DeleteFileAsync(string tenantId, string fileId)
        {
            ValidateTenantId(tenantId);
            ValidateFileId(fileId);

            var container = blobClient.GetContainerReference(GetContainerName(tenantId));

            if (await container.ExistsAsync().ConfigureAwait(false))
                await container.GetBlockBlobReference(fileId).DeleteIfExistsAsync().ConfigureAwait(false);
        }

        public async Task<Stream> LoadFileAsync(string tenantId, string fileId)
        {
            ValidateTenantId(tenantId);
            ValidateFileId(fileId);

            var container = blobClient.GetContainerReference(GetContainerName(tenantId));

            if (await container.ExistsAsync().ConfigureAwait(false))
            {
                var blob = container.GetBlockBlobReference(fileId);

                if (await blob.ExistsAsync().ConfigureAwait(false))
                {
                    var outputStream = new MemoryStream();

                    await blob.DownloadToStreamAsync(outputStream).ConfigureAwait(false);

                    outputStream.Position = 0;

                    return outputStream;
                }
            }

            return null;
        }

        public async Task SaveFileAsync(Stream fileContents, string tenantId, string fileId)
        {
            if (fileContents == null)
                throw new ArgumentNullException(nameof(fileContents));

            ValidateTenantId(tenantId);
            ValidateFileId(fileId);

            if (fileContents.CanSeek)
                fileContents.Position = 0;

            var container = blobClient.GetContainerReference(GetContainerName(tenantId));

            await container.CreateIfNotExistsAsync().ConfigureAwait(false);

            var blob = container.GetBlockBlobReference(fileId);

            await blob.UploadFromStreamAsync(fileContents).ConfigureAwait(false);
        }

        private CloudBlobClient CreateCloudBlobClient(IConfiguration configuration)
        {
            return CloudStorageAccount.Parse(configuration.StorageConnectionString).CreateCloudBlobClient();
        }

        private string GetContainerName(string tenantId)
        {
            return $"{tenantId}-file-staging";
        }

        private void ValidateFileId(string fileId)
        {
            if (fileId == null)
                throw new ArgumentNullException(nameof(fileId));

            if (fileId.Length == 0)
                throw new ArgumentException($"[{nameof(fileId)}] is required.", nameof(fileId));
        }

        private void ValidateTenantId(string tenantId)
        {
            if (tenantId == null)
                throw new ArgumentNullException(nameof(tenantId));

            if (tenantId.Length == 0)
                throw new ArgumentException($"[{nameof(tenantId)}] is required.", nameof(tenantId));
        }

        public interface IConfiguration
        {
            string StorageConnectionString { get; }
        }

        public class Configuration : IConfiguration
        {
            public string StorageConnectionString { get; set; }
        }

        public class LocalConfiguration : IConfiguration
        {
            public const string StorageConnectionStringConfigurationKey = "StorageConnectionString";

            public LocalConfiguration()
            {
                ConfigureStorageConnectionString();
            }

            public string StorageConnectionString { get; private set; }

            private void ConfigureStorageConnectionString()
            {
                var storageConnectionString =
                    ConfigurationManager.ConnectionStrings[StorageConnectionStringConfigurationKey];

                if (string.IsNullOrEmpty(storageConnectionString?.ConnectionString))
                {
                    throw new ConfigurationErrorsException(
                        $"[{StorageConnectionStringConfigurationKey}] not configured.");
                }

                StorageConnectionString = storageConnectionString.ConnectionString;
            }
        }
    }
}