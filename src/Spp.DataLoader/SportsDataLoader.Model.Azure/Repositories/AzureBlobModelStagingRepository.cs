/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SportsDataLoader.Model.Interfaces;

namespace SportsDataLoader.Model.Azure.Repositories
{
    public class AzureBlobModelStagingRepository : IModelStagingRepository
    {
        private readonly CloudBlobClient blobClient;

        public AzureBlobModelStagingRepository(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            blobClient = CreateCloudBlobClient(configuration);
        }

        public async Task DeleteAllTenantModels(string tenantId)
        {
            ValidateTenantId(tenantId);

            var container = blobClient.GetContainerReference(GetContainerName(tenantId));

            await container.DeleteIfExistsAsync().ConfigureAwait(false);
        }

        public async Task DeleteModel(string tenantId, string modelId)
        {
            ValidateTenantId(tenantId);
            ValidateModelId(modelId);

            var container = blobClient.GetContainerReference(GetContainerName(tenantId));

            if (await container.ExistsAsync().ConfigureAwait(false))
            {
                var blob = container.GetBlockBlobReference(modelId);

                await blob.DeleteIfExistsAsync().ConfigureAwait(false);
            }
        }

        public async Task<T> GetModel<T>(string tenantId, string modelId)
        {
            ValidateTenantId(tenantId);
            ValidateModelId(modelId);

            var container = blobClient.GetContainerReference(GetContainerName(tenantId));

            if (await container.ExistsAsync().ConfigureAwait(false))
            {
                var blob = container.GetBlockBlobReference(modelId);

                if (await blob.ExistsAsync().ConfigureAwait(false))
                    return DeserializeModel<T>(await blob.DownloadTextAsync().ConfigureAwait(false));
            }

            return default(T);
        }

        public async Task UpsertModel<T>(T model, string tenantId, string modelId)
        {
            ValidateTenantId(tenantId);
            ValidateModelId(modelId);

            var container = blobClient.GetContainerReference(GetContainerName(tenantId));

            await container.CreateIfNotExistsAsync().ConfigureAwait(false);

            var blob = container.GetBlockBlobReference(modelId);

            await blob.UploadTextAsync(SerializeModel(model)).ConfigureAwait(false);
        }

        private T DeserializeModel<T>(string serializedModel)
        {
            return JsonConvert.DeserializeObject<T>(serializedModel,
                                                    new JsonSerializerSettings
                                                    {
                                                        NullValueHandling = NullValueHandling.Ignore
                                                    });
        }

        private string SerializeModel<T>(T model)
        {
            return JsonConvert.SerializeObject(model,
                                               new JsonSerializerSettings
                                               {
                                                   NullValueHandling = NullValueHandling.Ignore
                                               });
        }

        private string GetContainerName(string tenantId)
        {
            return $"{tenantId}-model-staging";
        }

        private void ValidateTenantId(string tenantId)
        {
            if (tenantId == null)
                throw new ArgumentNullException(nameof(tenantId));

            if (tenantId.Length == 0)
                throw new ArgumentException($"[{nameof(tenantId)}] can not be empty.", nameof(tenantId));
        }

        private void ValidateModelId(string modelId)
        {
            if (modelId == null)
                throw new ArgumentNullException(nameof(modelId));

            if (modelId.Length == 0)
                throw new ArgumentException($"[{nameof(modelId)}] can not be empty.", nameof(modelId));
        }

        private CloudBlobClient CreateCloudBlobClient(IConfiguration configuration)
        {
            return CloudStorageAccount.Parse(configuration.StorageConnectionString).CreateCloudBlobClient();
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