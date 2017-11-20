/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using SportsDataLoader.Model.Interfaces;

namespace SportsDataLoader.Model.Azure.Repositories
{
    public class AzureDocumentDbSchemaRepository : ISchemaRepository
    {
        private readonly IConfiguration configuration;
        private readonly DocumentClient documentClient;

        public AzureDocumentDbSchemaRepository(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.configuration = configuration;

            documentClient = new DocumentClient(new Uri(configuration.DocumentDbEndpointUrl),
                                                configuration.DocumentDbAuthKey);
        }

        public async Task DeleteSchema(string schemaId)
        {
            ValidateSchemaId(schemaId);

            var collectionUri = UriFactory.CreateDocumentCollectionUri(configuration.DocumentDbSchemaDatabaseId,
                                                                       configuration.DocumentDbSchemaCollectionId);

            var schema = documentClient.CreateDocumentQuery<Schema>(collectionUri)
                .Where(d => (d.SchemaId == schemaId))
                .AsEnumerable()
                .FirstOrDefault();


            if (schema != null)
            {
                var documentUri = UriFactory.CreateDocumentUri(configuration.DocumentDbSchemaDatabaseId,
                                                               configuration.DocumentDbSchemaCollectionId,
                                                               schema.SchemaId);

                await documentClient.DeleteDocumentAsync(documentUri).ConfigureAwait(false);
            }
        }

        public Task<Schema> GetSchemaById(string schemaId)
        {
            ValidateSchemaId(schemaId);

            var collectionUri = UriFactory.CreateDocumentCollectionUri(configuration.DocumentDbSchemaDatabaseId,
                                                                       configuration.DocumentDbSchemaCollectionId);

            return Task.FromResult(documentClient.CreateDocumentQuery<Schema>(collectionUri)
                                       .Where(d => (d.SchemaId == schemaId))
                                       .AsEnumerable()
                                       .FirstOrDefault());
        }

        public Task<Schema> GetSchemaByName(string schemaName)
        {
            ValidateSchemaName(schemaName);

            var collectionUri = UriFactory.CreateDocumentCollectionUri(configuration.DocumentDbSchemaDatabaseId,
                                                                       configuration.DocumentDbSchemaCollectionId);

            return Task.FromResult(documentClient.CreateDocumentQuery<Schema>(collectionUri)
                                       .Where(d => (d.SchemaName == schemaName))
                                       .AsEnumerable()
                                       .FirstOrDefault());
        }

        public async Task UpsertSchema(Schema schema)
        {
            if (schema == null)
                throw new ArgumentNullException(nameof(schema));

            var collectionUri = UriFactory.CreateDocumentCollectionUri(configuration.DocumentDbSchemaDatabaseId,
                                                                       configuration.DocumentDbSchemaCollectionId);

            await documentClient.UpsertDocumentAsync(collectionUri, schema).ConfigureAwait(false);
        }

        public Task<IEnumerable<Schema>> GetAllPublicSchemas()
        {
            var collectionUri = UriFactory.CreateDocumentCollectionUri(configuration.DocumentDbSchemaDatabaseId,
                                                                       configuration.DocumentDbSchemaCollectionId);

            return Task.FromResult(documentClient.CreateDocumentQuery<Schema>(collectionUri)
                                       .Where(d => (d.TenantId == null))
                                       .AsEnumerable());
        }

        public Task<IEnumerable<Schema>> GetAllTenantSchemas(string tenantId)
        {
            if (tenantId == null)
                throw new ArgumentNullException(nameof(tenantId));

            if (tenantId.Length == 0)
                throw new ArgumentException($"[{nameof(tenantId)}] can not be empty.", nameof(tenantId));

            var collectionUri = UriFactory.CreateDocumentCollectionUri(configuration.DocumentDbSchemaDatabaseId,
                                                                       configuration.DocumentDbSchemaCollectionId);

            return Task.FromResult(documentClient.CreateDocumentQuery<Schema>(collectionUri)
                                       .Where(d => (d.TenantId == tenantId))
                                       .AsEnumerable());
        }

        private void ValidateSchemaId(string schemaId)
        {
            if (schemaId == null)
                throw new ArgumentNullException(nameof(schemaId));

            if (schemaId.Length == 0)
                throw new ArgumentException($"[{nameof(schemaId)}] can not be empty.", nameof(schemaId));
        }

        private void ValidateSchemaName(string schemaName)
        {
            if (schemaName == null)
                throw new ArgumentNullException(nameof(schemaName));

            if (schemaName.Length == 0)
                throw new ArgumentException($"[{nameof(schemaName)}] can not be empty.", nameof(schemaName));
        }

        public interface IConfiguration
        {
            string DocumentDbEndpointUrl { get; }
            string DocumentDbAuthKey { get; }
            string DocumentDbSchemaDatabaseId { get; }
            string DocumentDbSchemaCollectionId { get; }
        }

        public class Configuration : IConfiguration
        {
            public string DocumentDbEndpointUrl { get; set; }
            public string DocumentDbAuthKey { get; set; }
            public string DocumentDbSchemaDatabaseId { get; set; }
            public string DocumentDbSchemaCollectionId { get; set; }
        }

        public class LocalConfiguration : IConfiguration
        {
            public LocalConfiguration()
            {
                ConfigureDocumentDbEndpointUrl();
                ConfigureDocumentDbAuthKey();
                ConfigureDocumentDbSchemaDatabaseId();
                ConfigureDocumentDbSchemaCollectionId();
            }

            public string DocumentDbEndpointUrl { get; private set; }
            public string DocumentDbAuthKey { get; private set; }
            public string DocumentDbSchemaDatabaseId { get; private set; }
            public string DocumentDbSchemaCollectionId { get; private set; }

            private void ConfigureDocumentDbEndpointUrl()
            {
                var endpointUrl = ConfigurationManager.AppSettings[nameof(DocumentDbEndpointUrl)];

                if (string.IsNullOrEmpty(endpointUrl))
                    throw new ConfigurationErrorsException($"[{nameof(DocumentDbEndpointUrl)}] not configured.");

                DocumentDbEndpointUrl = endpointUrl;
            }

            private void ConfigureDocumentDbAuthKey()
            {
                var authKey = ConfigurationManager.AppSettings[nameof(DocumentDbAuthKey)];

                if (string.IsNullOrEmpty(authKey))
                    throw new ConfigurationErrorsException($"[{nameof(DocumentDbAuthKey)}] not configured.");

                DocumentDbAuthKey = authKey;
            }

            private void ConfigureDocumentDbSchemaDatabaseId()
            {
                var databaseId = ConfigurationManager.AppSettings[nameof(DocumentDbSchemaDatabaseId)];

                if (string.IsNullOrEmpty(databaseId))
                    throw new ConfigurationErrorsException($"[{nameof(DocumentDbSchemaDatabaseId)}] not configured.");

                DocumentDbSchemaDatabaseId = databaseId;
            }

            private void ConfigureDocumentDbSchemaCollectionId()
            {
                var collectionId = ConfigurationManager.AppSettings[nameof(DocumentDbSchemaCollectionId)];

                if (string.IsNullOrEmpty(collectionId))
                    throw new ConfigurationErrorsException($"[{nameof(DocumentDbSchemaCollectionId)}] not configured.");

                DocumentDbSchemaCollectionId = collectionId;
            }
        }
    }
}