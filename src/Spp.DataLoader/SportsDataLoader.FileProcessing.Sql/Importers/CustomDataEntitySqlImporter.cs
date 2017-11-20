/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;
using SportsDataLoader.FileProcessing.Extensions;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.FileProcessing.Sql.Extensions;
using SportsDataLoader.FileProcessing.Sql.Interfaces;
using SportsDataLoader.FileProcessing.Sql.Models;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Enumerations;
using DataTable = System.Data.DataTable;

namespace SportsDataLoader.FileProcessing.Sql.Importers
{
    public class CustomDataEntitySqlImporter : IImporter<CustomDataEntity>
    {
        private readonly IConfiguration configuration;
        private readonly ILogger logger;
        private readonly IRelationshipDiscoverer[] relationshipDiscoverers;
        private readonly ISqlConnectionStringProvider sqlConnectionStringProvider;

        public CustomDataEntitySqlImporter(IConfiguration configuration,
                                           ILogger logger,
                                           IRelationshipDiscoverer[] relationshipDiscoverers,
                                           ISqlConnectionStringProvider sqlConnectionStringProvider)
        {


            this.configuration = configuration;
            this.logger = logger;
            this.relationshipDiscoverers = relationshipDiscoverers;
            this.sqlConnectionStringProvider = sqlConnectionStringProvider;

            this.logger.Debug("Custom Data Entity Sql Importer Configuration: [{@configuration}].", configuration);
        }

        public async Task Import(ImportMetadata<CustomDataEntity> importMetadata)
        {
            if (importMetadata == null)
                throw new ArgumentNullException(nameof(importMetadata));

            logger.Debug("Importing [{importId}] into SQL Server...", importMetadata.ImportId);

            var sqlConnectionString =
                await sqlConnectionStringProvider.GetTenantPrimaryDbConnectionString(importMetadata.TenantId);

            if (sqlConnectionString == null)
            {
                throw new InvalidOperationException(
                    $"Tenant [{importMetadata.TenantId}] primary database connection string not found.");
            }

            logger.Debug("Tenant SQL Server connection string is [{connectionString}].",
                         sqlConnectionString);

            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                await CreateSchemaTableIfNotExists(sqlConnection, importMetadata.Schema);

                logger.Debug("Importing data into SQL server...");

                await InsertDataTable(sqlConnection, ToDataTable(importMetadata));

                if (configuration.TryToDiscoverRelationships)
                {
                    foreach (var relationshipDiscoverer in relationshipDiscoverers)
                    {
                        var discoveredRelationship = await relationshipDiscoverer
                            .TryToDiscoverRelationshipAsync(importMetadata.Entities, importMetadata.TenantId);

                        if (discoveredRelationship != null)
                        {
                            var discoveredSqlRelationship =
                                new DiscoveredSqlRelationship(discoveredRelationship, importMetadata.Schema);

                            await CreateRelationshipTableIfNotExists(sqlConnection, discoveredSqlRelationship);
                            await InsertDataTable(sqlConnection, ToDataTable(discoveredSqlRelationship));
                        }
                    }
                }
            }
        }

        private async Task InsertDataTable(SqlConnection sqlConnection, DataTable dataTable)
        {
            using (var transaction = sqlConnection.BeginTransaction())
            {
                var bulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, transaction)
                {
                    BatchSize = 5000,
                    DestinationTableName = dataTable.TableName
                };

                await bulkCopy.WriteToServerAsync(dataTable).ConfigureAwait(false);

                transaction.Commit();
            }
        }

        private async Task CreateRelationshipTableIfNotExists(SqlConnection sqlConnection,
                                                              DiscoveredSqlRelationship discoveredRelationship)
        {
            var tableName = discoveredRelationship.RelationshipTableName;

            var commandText =
                "IF (" +
                "NOT EXISTS (" +
                $"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{tableName}')) " +
                "BEGIN " +
                $"CREATE TABLE [{tableName}] (" +
                $"[{discoveredRelationship.PrimaryKeyColumnName}] int NOT NULL, " +
                $"[{discoveredRelationship.ForeignKeyColumnName}] uniqueidentifier NOT NULL " +
                ") END;";

            using (var sqlCommand = new SqlCommand(commandText, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.Text;

                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        private async Task CreateSchemaTableIfNotExists(SqlConnection sqlConnection, Schema schema)
        {
            var tableName = schema.SchemaName.ToSqlIdentifier();
            var commandBuilder = new StringBuilder();

            logger.Debug("Ensuring that SQL Server table [{tableName}] already exists. " +
                         "If it does not exist, it will be created.",
                         tableName);

            commandBuilder.AppendLine(
                "IF (" +
                "NOT EXISTS (" +
                $"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{tableName}')) " +
                "BEGIN " +
                $"CREATE TABLE [{tableName}] (");

            commandBuilder.AppendLine($"[{CommonColumnNames.Id}] uniqueidentifier PRIMARY KEY NOT NULL");

            foreach (var schemaColumn in schema.SchemaColumns)
            {
                var sqlColumnName = schemaColumn.Name.ToSqlIdentifier();

                if (schemaColumn.SqlDataType != null)
                {
                    commandBuilder.AppendLine($", [{sqlColumnName}] {schemaColumn.SqlDataType} NULL");
                }
                else
                {
                    switch (schemaColumn.DataType)
                    {
                        case DataType.Boolean:
                            commandBuilder.AppendLine($", [{sqlColumnName}] bit NULL");
                            break;
                        case DataType.Guid:
                            commandBuilder.AppendLine($", [{sqlColumnName}] uniqueidentifier NULL");
                            break;
                        case DataType.DateTime:
                            commandBuilder.AppendLine($", [{sqlColumnName}] datetime2 NULL");
                            break;
                        case DataType.DateTimeOffset:
                            commandBuilder.AppendLine($", [{sqlColumnName}] datetimeoffset NULL");
                            break;
                        case DataType.Double:
                        case DataType.TimeSpan:
                            commandBuilder.AppendLine($", [{sqlColumnName}] float NULL");
                            break;
                        case DataType.Integer:
                            commandBuilder.AppendLine($", [{sqlColumnName}] int NULL");
                            break;
                        case DataType.String:
                        case DataType.Undefined:
                            commandBuilder.AppendLine($", [{sqlColumnName}] nvarchar(max) NULL");
                            break;
                    }
                }
            }

            commandBuilder.AppendLine($", [{CommonColumnNames.CreatedDateTimeUtc}] datetime2 NOT NULL");
            commandBuilder.AppendLine($", [{CommonColumnNames.LastModifiedDateTimeUtc}] datetime2 NOT NULL");
            commandBuilder.AppendLine($", [{CommonColumnNames.ImportId}] nvarchar(max) NULL");
            commandBuilder.AppendLine($", [{CommonColumnNames.Hash}] nvarchar(max) NULL");

            commandBuilder.AppendLine("); END");

            using (var sqlCommand = new SqlCommand(commandBuilder.ToString(), sqlConnection))
            {
                sqlCommand.CommandType = CommandType.Text;

                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        private DataTable ToDataTable(DiscoveredSqlRelationship discoveredRelationship)
        {
            var dataTable = new DataTable(discoveredRelationship.RelationshipTableName);

            dataTable.Columns.Add(discoveredRelationship.PrimaryKeyColumnName, typeof(int));
            dataTable.Columns.Add(discoveredRelationship.ForeignKeyColumnName, typeof(Guid));

            foreach (var fk in discoveredRelationship.RelatedRecordIds.Keys)
            {
                var pk = discoveredRelationship.RelatedRecordIds[fk];
                var dataRow = dataTable.NewRow();

                dataRow[discoveredRelationship.PrimaryKeyColumnName] = pk;
                dataRow[discoveredRelationship.ForeignKeyColumnName] = fk;

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private DataTable ToDataTable(ImportMetadata<CustomDataEntity> importMetadata)
        {
            var dataTable = new DataTable(importMetadata.Schema.SchemaName.ToSqlIdentifier());

            dataTable = AddDataTableColumns(dataTable, importMetadata.Schema);

            foreach (var dataEntity in importMetadata.Entities)
            {
                var dataRow = dataTable.NewRow();

                dataRow[CommonColumnNames.Id] = dataEntity.EntityId;

                foreach (var schemaColumn in importMetadata.Schema.SchemaColumns)
                    dataRow[schemaColumn.Name.ToSqlIdentifier()] = GetColumnValue(schemaColumn, dataEntity);

                dataRow[CommonColumnNames.CreatedDateTimeUtc] = DateTime.UtcNow;
                dataRow[CommonColumnNames.LastModifiedDateTimeUtc] = DateTime.UtcNow;
                dataRow[CommonColumnNames.ImportId] = importMetadata.ImportId;
                dataRow[CommonColumnNames.Hash] = ComputeHash(dataEntity);

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private string ComputeHash(CustomDataEntity customDataEntity)
        {
            var entityJson = JsonConvert.SerializeObject(customDataEntity.Columns,
                                                         new JsonSerializerSettings
                                                         {
                                                             NullValueHandling = NullValueHandling.Ignore
                                                         });

            var entityJsonBytes = Encoding.UTF8.GetBytes(entityJson);

            using (var sha1 = SHA1.Create())
            {
                var hashBuilder = new StringBuilder();
                var hashBytes = sha1.ComputeHash(entityJsonBytes);

                foreach (var hashByte in hashBytes)
                    hashBuilder.Append(hashByte.ToString("x2"));

                return hashBuilder.ToString();
            }
        }

        private object GetColumnValue(SchemaColumn column, CustomDataEntity dataEntity)
        {
            object columnValue = null;

            if (dataEntity.Columns.ContainsKey(column.Name))
            {
                switch (column.DataType)
                {
                    case DataType.Boolean:
                        columnValue = dataEntity.Columns[column.Name].BooleanValue;
                        break;
                    case DataType.Guid:
                        columnValue = dataEntity.Columns[column.Name].GuidValue;
                        break;
                    case DataType.DateTime:
                        columnValue = dataEntity.Columns[column.Name].DateTimeValue;
                        break;
                    case DataType.DateTimeOffset:
                        columnValue = dataEntity.Columns[column.Name].DateTimeOffsetValue;
                        break;
                    case DataType.Double:
                        columnValue = CleanDoubleValue(dataEntity.Columns[column.Name].DoubleValue);
                        break;
                    case DataType.Integer:
                        columnValue = dataEntity.Columns[column.Name].IntegerValue;
                        break;
                    case DataType.TimeSpan:
                        columnValue = dataEntity.Columns[column.Name].TimeSpanValue?.TotalSeconds;
                        break;
                    default:
                        columnValue = dataEntity.Columns[column.Name].StringValue;
                        break;
                }
            }

            return columnValue ?? DBNull.Value;
        }

        private double? CleanDoubleValue(double? source)
        {
            if ((source == null) ||
                (source == double.NaN) ||
                (source == double.NegativeInfinity) ||
                (source == double.PositiveInfinity))
                return null;

            return source;
        }

        private DataTable AddDataTableColumns(DataTable dataTable, Schema schema)
        {
            dataTable.Columns.Add(CommonColumnNames.Id, typeof(Guid));
            dataTable.Columns.AddRange(schema.SchemaColumns.Select(ToDataColumn).ToArray());
            dataTable.Columns.Add(CommonColumnNames.CreatedDateTimeUtc, typeof(DateTime));
            dataTable.Columns.Add(CommonColumnNames.LastModifiedDateTimeUtc, typeof(DateTime));
            dataTable.Columns.Add(CommonColumnNames.ImportId, typeof(string));
            dataTable.Columns.Add(CommonColumnNames.Hash, typeof(string));

            return dataTable;
        }

        private DataColumn ToDataColumn(SchemaColumn schemaColumn)
        {
            var sqlColumnName = schemaColumn.Name.ToSqlIdentifier();

            switch (schemaColumn.DataType)
            {
                case DataType.Boolean:
                    return new DataColumn(sqlColumnName, typeof(bool));
                case DataType.Guid:
                    return new DataColumn(sqlColumnName, typeof(Guid));
                case DataType.DateTime:
                    return new DataColumn(sqlColumnName, typeof(DateTime));
                case DataType.DateTimeOffset:
                    return new DataColumn(sqlColumnName, typeof(DateTimeOffset));
                case DataType.Double:
                case DataType.TimeSpan:
                    return new DataColumn(sqlColumnName, typeof(double));
                case DataType.Integer:
                    return new DataColumn(sqlColumnName, typeof(int));
                default:
                    return new DataColumn(sqlColumnName, typeof(string));
            }
        }

        public static class CommonColumnNames
        {
            public const string Id = "_Id";
            public const string CreatedDateTimeUtc = "_CreatedDateTimeUtc";
            public const string LastModifiedDateTimeUtc = "_LastModifiedDateTimeUtc";
            public const string ImportId = "_ImportId";
            public const string Hash = "_Hash";
        }

        public interface IConfiguration
        {
            bool TryToDiscoverRelationships { get; }
        }

        public class Configuration : IConfiguration
        {
            public bool TryToDiscoverRelationships { get; set; }
        }

        public class LocalConfiguration : IConfiguration
        {
            public const string TryToDiscoverRelationshipsKey = "TryToDiscoverRelationships";

            public LocalConfiguration()
            {
                ConfigureTryToDiscoverRelationships();
            }

            public bool TryToDiscoverRelationships { get; private set; }

            private void ConfigureTryToDiscoverRelationships()
            {
                TryToDiscoverRelationships = ConfigurationManager
                    .AppSettings[TryToDiscoverRelationshipsKey]
                    .TryParseBoolean()
                    .GetValueOrDefault();
            }
        }
    }
}