/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SportsDataLoader.Model.Azure.Entities;
using SportsDataLoader.Model.Azure.Extensions;
using SportsDataLoader.Model.Interfaces;
using SportsDataLoader.Shared.Extensions;

namespace SportsDataLoader.Model.Azure.Repositories
{
    public class AzureTableFileMetadataRepository : IFileMetadataRepository
    {
        private readonly CloudTable table;

        public AzureTableFileMetadataRepository(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            table = CreateCloudTableReference(configuration);
        }

        public async Task UpsertAllFileMetadata(IEnumerable<FileMetadata> fileMetadata)
        {
            if (fileMetadata == null)
                throw new ArgumentNullException(nameof(fileMetadata));

            var entityGroups = fileMetadata
                .Select(m => m.ToFileMetadataTableEntity())
                .GroupBy(m => m.PartitionKey)
                .ToList();

            foreach (var entityGroup in entityGroups)
            {
                foreach (var entityChunk in entityGroup.Chunk(100))
                {
                    var batchOp = new TableBatchOperation();

                    foreach (var entity in entityChunk)
                        batchOp.InsertOrReplace(entity);

                    await table.ExecuteBatchAsync(batchOp).ConfigureAwait(false);
                }
            }
        }

        public async Task UpsertFileMetadata(FileMetadata fileMetadata)
        {
            if (fileMetadata == null)
                throw new ArgumentNullException(nameof(fileMetadata));

            var upsertOp = TableOperation.InsertOrReplace(fileMetadata.ToFileMetadataTableEntity());

            await table.ExecuteAsync(upsertOp).ConfigureAwait(false);
        }

        public async Task DeleteAllTenantFileMetadata(string tenantId)
        {
            ValidateTenantId(tenantId);

            var query = new TableQuery<FileMetadataTableEntity>()
                .Where(TableQuery.GenerateFilterCondition(nameof(FileMetadataTableEntity.PartitionKey),
                                                          QueryComparisons.Equal, tenantId));

            var entities = table.ExecuteQuery(query).ToList();

            foreach (var entityChunk in entities.Chunk(100))
            {
                var batchOp = new TableBatchOperation();

                foreach (var entity in entityChunk)
                    batchOp.Delete(entity);

                await table.ExecuteBatchAsync(batchOp).ConfigureAwait(false);
            }
        }

        public async Task DeleteFileMetadata(string tenantId, string fileId)
        {
            ValidateTenantId(tenantId);
            ValidateFileId(fileId);

            var retrieveOp = TableOperation.Retrieve<FileMetadataTableEntity>(tenantId, fileId);
            var retrieveResult = await table.ExecuteAsync(retrieveOp).ConfigureAwait(false);

            if (retrieveResult.Result != null)
            {
                var entity = (retrieveResult.Result as FileMetadataTableEntity);
                var deleteOp = TableOperation.Delete(entity);

                await table.ExecuteAsync(deleteOp).ConfigureAwait(false);
            }
        }

        public Task<IEnumerable<FileMetadata>> GetAllTenantFileMetadata(string tenantId)
        {
            ValidateTenantId(tenantId);

            var query = new TableQuery<FileMetadataTableEntity>()
                .Where(TableQuery.GenerateFilterCondition(nameof(FileMetadataTableEntity.PartitionKey),
                                                          QueryComparisons.Equal, tenantId));

            return Task.FromResult(table.ExecuteQuery(query).Select(e => e.ToFileMetadata()));
        }

        public async Task<FileMetadata> GetFileMetadata(string tenantId, string fileId)
        {
            ValidateTenantId(tenantId);
            ValidateFileId(fileId);

            var retrieveOp = TableOperation.Retrieve<FileMetadataTableEntity>(tenantId, fileId);
            var retrieveResult = await table.ExecuteAsync(retrieveOp).ConfigureAwait(false);

            return (retrieveResult.Result as FileMetadataTableEntity)?.ToFileMetadata();
        }

        private CloudTable CreateCloudTableReference(IConfiguration configuration)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(configuration.StorageConnectionString);
            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            var tableReference = cloudTableClient.GetTableReference(configuration.TableName);

            tableReference.CreateIfNotExists();

            return tableReference;
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
            string TableName { get; }
        }

        public class Configuration : IConfiguration
        {
            public string StorageConnectionString { get; set; }
            public string TableName { get; set; }
        }

        public class LocalConfiguration : IConfiguration
        {
            public const string StorageConnectionStringConfigurationKey = "StorageConnectionString";
            public const string TableNameConfigurationKey = "FileMetadataTableName";
            public const string DefaultTableName = "FileMetadata";

            public LocalConfiguration()
            {
                ConfigureStorageConnectionString();
                ConfigureTableName();
            }

            public string StorageConnectionString { get; private set; }
            public string TableName { get; private set; }

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

            private void ConfigureTableName()
            {
                TableName = (ConfigurationManager.AppSettings[TableNameConfigurationKey] ??
                             DefaultTableName);
            }
        }
    }
}