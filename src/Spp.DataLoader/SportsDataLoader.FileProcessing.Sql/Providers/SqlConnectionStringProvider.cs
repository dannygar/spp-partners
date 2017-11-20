/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Configuration;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Sql.Interfaces;

namespace SportsDataLoader.FileProcessing.Sql.Providers
{
    public class SqlConnectionStringProvider : ISqlConnectionStringProvider
    {
        private readonly IConfiguration configuration;

        public SqlConnectionStringProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<string> GetTenantPrimaryDbConnectionString(string tenantId)
        {
            if (tenantId == null)
                throw new ArgumentNullException(nameof(tenantId));

            return Task.FromResult(configuration.SqlConnectionString);
        }

        public interface IConfiguration
        {
            string SqlConnectionString { get; }
        }

        public class Configuration : IConfiguration
        {
            public string SqlConnectionString { get; set; }
        }

        public class LocalConfiguration : IConfiguration
        {
            public LocalConfiguration()
            {
                ConfigureSqlConnectionString();
            }

            public string SqlConnectionString { get; private set; }

            private void ConfigureSqlConnectionString()
            {
                var connectionString = ConfigurationManager.ConnectionStrings[nameof(SqlConnectionString)];

                if (string.IsNullOrEmpty(connectionString?.ConnectionString))
                    throw new ConfigurationErrorsException($"[{nameof(SqlConnectionString)}] not configured.");

                SqlConnectionString = connectionString.ConnectionString;
            }
        }
    }
}