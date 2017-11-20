/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using SportsDataLoader.FileProcessing.Interfaces;

namespace SportsDataLoader.FileProcessing.Azure.Repositories
{
    public class AzureDocumentDbAnalysisRepository : IAnalysisRepository
    {
        private readonly IConfiguration configuration;
        private readonly DocumentClient documentClient;

        public AzureDocumentDbAnalysisRepository(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.configuration = configuration;

            documentClient = new DocumentClient(new Uri(configuration.DocumentDbEndpointUrl),
                                                configuration.DocumentDbAuthKey);
        }

        public async Task DeleteAnalysis(string analysisId)
        {
            ValidateAnalysisId(analysisId);

            var collectionUri = UriFactory.CreateDocumentCollectionUri(configuration.DocumentDbAnalysisDatabaseId,
                                                                       configuration.DocumentDbAnalysisCollectionId);

            var analysis = documentClient.CreateDocumentQuery<Model.Analysis>(collectionUri)
                .Where(d => (d.AnalysisId == analysisId))
                .AsEnumerable()
                .FirstOrDefault();


            if (analysis != null)
            {
                var documentUri = UriFactory.CreateDocumentUri(configuration.DocumentDbAnalysisDatabaseId,
                                                               configuration.DocumentDbAnalysisCollectionId,
                                                               analysis.AnalysisId);

                await documentClient.DeleteDocumentAsync(documentUri).ConfigureAwait(false);
            }
        }

        public Task<Model.Analysis> GetAnalysis(string analysisId)
        {
            ValidateAnalysisId(analysisId);

            var collectionUri = UriFactory.CreateDocumentCollectionUri(configuration.DocumentDbAnalysisDatabaseId,
                                                                       configuration.DocumentDbAnalysisCollectionId);

            return Task.FromResult(documentClient.CreateDocumentQuery<Model.Analysis>(collectionUri)
                                       .Where(d => (d.AnalysisId == analysisId))
                                       .AsEnumerable()
                                       .FirstOrDefault());
        }

        public Task<Model.Analysis> GetAnalysisByFileId(string fileId)
        {
            if (fileId == null)
                throw new ArgumentNullException(nameof(fileId));

            if (fileId.Length == 0)
                throw new ArgumentException($"[{nameof(fileId)}] can not be empty.", nameof(fileId));

            var collectionUri = UriFactory.CreateDocumentCollectionUri(configuration.DocumentDbAnalysisDatabaseId,
                                                                       configuration.DocumentDbAnalysisCollectionId);

            return Task.FromResult(documentClient.CreateDocumentQuery<Model.Analysis>(collectionUri)
                                       .Where(d => (d.FileId == fileId))
                                       .AsEnumerable()
                                       .FirstOrDefault());
        }

        public async Task UpsertAnalysis(Model.Analysis analysis)
        {
            if (analysis == null)
                throw new ArgumentNullException(nameof(analysis));

            var collectionUri = UriFactory.CreateDocumentCollectionUri(configuration.DocumentDbAnalysisDatabaseId,
                                                                       configuration.DocumentDbAnalysisCollectionId);

            await documentClient.UpsertDocumentAsync(collectionUri, analysis).ConfigureAwait(false);
        }

        private void ValidateAnalysisId(string analysisId)
        {
            if (analysisId == null)
                throw new ArgumentNullException(nameof(analysisId));

            if (analysisId.Length == 0)
                throw new ArgumentException($"[{nameof(analysisId)}] can not be empty.", nameof(analysisId));
        }

        public interface IConfiguration
        {
            string DocumentDbEndpointUrl { get; }
            string DocumentDbAuthKey { get; }
            string DocumentDbAnalysisDatabaseId { get; }
            string DocumentDbAnalysisCollectionId { get; }
        }

        public class Configuration : IConfiguration
        {
            public string DocumentDbEndpointUrl { get; set; }
            public string DocumentDbAuthKey { get; set; }
            public string DocumentDbAnalysisDatabaseId { get; set; }
            public string DocumentDbAnalysisCollectionId { get; set; }
        }

        public class LocalConfiguration : IConfiguration
        {
            public LocalConfiguration()
            {
                ConfigureDocumentDbEndpointUrl();
                ConfigureDocumentDbAuthKey();
                ConfigureDocumentDbAnalysisDatabaseId();
                ConfigureDocumentDbAnalyisCollectionId();
            }

            public string DocumentDbEndpointUrl { get; private set; }
            public string DocumentDbAuthKey { get; private set; }
            public string DocumentDbAnalysisDatabaseId { get; private set; }
            public string DocumentDbAnalysisCollectionId { get; private set; }

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

            private void ConfigureDocumentDbAnalysisDatabaseId()
            {
                var databaseId = ConfigurationManager.AppSettings[nameof(DocumentDbAnalysisDatabaseId)];

                if (string.IsNullOrEmpty(databaseId))
                    throw new ConfigurationErrorsException($"[{nameof(DocumentDbAnalysisDatabaseId)}] not configured.");

                DocumentDbAnalysisDatabaseId = databaseId;
            }

            private void ConfigureDocumentDbAnalyisCollectionId()
            {
                var collectionId = ConfigurationManager.AppSettings[nameof(DocumentDbAnalysisCollectionId)];

                if (string.IsNullOrEmpty(collectionId))
                    throw new ConfigurationErrorsException($"[{nameof(DocumentDbAnalysisCollectionId)}] not configured.");

                DocumentDbAnalysisCollectionId = collectionId;
            }
        }
    }
}