/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Extensions;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Enumerations;
using SportsDataLoader.Shared.Extensions;

namespace SportsDataLoader.FileProcessing.Analyzers
{
    public class DataTableAnalyzer : IDataTableAnalyzer
    {
        private const double MinColumnDataTypeMatch = .9;

        private readonly Dictionary<DataType, Func<string, CultureInfo, bool>> dataTypeDetectors;

        public DataTableAnalyzer()
        {
            dataTypeDetectors = new Dictionary<DataType, Func<string, CultureInfo, bool>>
            {
                [DataType.Boolean] = (s, ci) => s.TryParseBoolean(ci) != null,
                [DataType.DateTime] = (s, ci) => s.TryParseDateTime(ci) != null,
                [DataType.DateTimeOffset] = MayBeDateTimeOffset,
                [DataType.Double] = (s, ci) => s.TryParseDouble(ci) != null,
                [DataType.Integer] = (s, ci) => s.TryParseInteger(ci) != null,
                [DataType.String] = (s, ci) => true,
                [DataType.TimeSpan] = (s, ci) => s.TryParseTimeSpan(ci) != null
            };
        }

        public Task<Analysis> AnalyzeDataTableAsync(DataTable dataTable, FileMetadata fileMetadata)
        {
            ValidateDataTable(dataTable);

            if (fileMetadata == null)
                throw new ArgumentNullException(nameof(fileMetadata));

            return Task.FromResult(CreateAnalysis(CreateDataTypeAnalysis(dataTable, fileMetadata), fileMetadata));
        }

        private bool MayBeDateTimeOffset(string source, CultureInfo cultureInfo)
        {
            // Checking for common date/time formats that include time zone information.
            // e.g., +05:00, -0700

            return (Regex.IsMatch(source, @"(Z|[+-]\d{2}:?\d{2})$")) &&
                   (source.TryParseDateTimeOffset(cultureInfo) != null);
        }

        public DataTableAnalysis CreateDataTypeAnalysis(DataTable dataTable, FileMetadata fileMetadata)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(fileMetadata.FileCultureCode);
            var tableAnalysis = new DataTableAnalysis {RowCount = dataTable.Rows.Count};

            foreach (var column in dataTable.Columns)
            {
                var columnAnalysis = new DataColumnAnalysis();

                var columnValues = dataTable.Rows.Where(r => r.ContainsKey(column))
                                            .Select(r => r[column])
                                            .ToList();

                columnAnalysis.ValueCount = columnValues.Count;

                foreach (var columnValue in columnValues)
                {
                    var columnDataType = columnValue.DataType;

                    if ((columnDataType == DataType.String) || (columnDataType == DataType.Undefined))
                    {
                        foreach (var dataType in dataTypeDetectors.Keys)
                        {
                            var columnValueString = columnValue.ToString();

                            if (dataTypeDetectors[dataType](columnValueString, cultureInfo) ||
                                dataTypeDetectors[dataType](columnValueString, CultureInfo.InvariantCulture))
                                columnAnalysis.PossibleColumnDataTypes[dataType]++;
                        }
                    }
                    else
                    {
                        columnAnalysis.PossibleColumnDataTypes[columnDataType]++;
                    }
                }

                tableAnalysis.Columns.Add(column, columnAnalysis);
            }

            return tableAnalysis;
        }

        public Analysis CreateAnalysis(DataTableAnalysis dataTypeAnalysis, FileMetadata fileMetadata)
        {
            var tableAnalysis = new Analysis
            {
                AnalysisDateTimeUtc = DateTime.UtcNow,
                AnalysisId = Guid.NewGuid().ToString(),
                Columns = new List<ColumnAnalysis>(),
                FileId = fileMetadata.FileId,
                FileName = fileMetadata.FileName,
                TenantId = fileMetadata.TenantId
            };

            foreach (var dataColumnAnalysisKey in dataTypeAnalysis.Columns.Keys)
            {
                var dataColumnAnalysis = dataTypeAnalysis.Columns[dataColumnAnalysisKey];

                tableAnalysis.Columns.Add(new ColumnAnalysis
                {
                    ColumnId = Guid.NewGuid().ToString(),
                    ColumnName = dataColumnAnalysisKey,
                    ColumnDataType = GetBestGuessDataType(dataColumnAnalysis),
                    PossibleColumnDataTypes = GetPossibleDataTypes(dataColumnAnalysis).ToList()
                });
            }

            return tableAnalysis;
        }

        private bool IsPossibleDataType(double dataTypeCount, double valueCount)
        {
            return ((dataTypeCount/valueCount) >= MinColumnDataTypeMatch);
        }

        private IEnumerable<DataType> GetPossibleDataTypes(DataColumnAnalysis dataColumnAnalysis)
        {
            return dataColumnAnalysis.PossibleColumnDataTypes
                                     .Where(dt => IsPossibleDataType(dt.Value, dataColumnAnalysis.ValueCount))
                                     .Select(dt => dt.Key)
                                     .ToList();
        }

        private DataType GetBestGuessDataType(DataColumnAnalysis dataColumnAnalysis)
        {
            var possibleDataTypes =
                dataColumnAnalysis.PossibleColumnDataTypes
                                  .Where(dt => IsPossibleDataType(dt.Value, dataColumnAnalysis.ValueCount))
                                  .ToDictionary(dt => dt.Key, dt => dt.Value);

            var dateTimeDataTypes = possibleDataTypes
                .Where(dt => (dt.Key == DataType.DateTime) ||
                             (dt.Key == DataType.DateTimeOffset))
                .ToDictionary(dt => dt.Key, dt => dt.Value);

            if (dateTimeDataTypes.Any())
            {
                var dateTimeCt = (dateTimeDataTypes.Max(dt => dt.Value));

                if ((possibleDataTypes.ContainsKey(DataType.Integer)) &&
                    (possibleDataTypes[DataType.Integer] >= dateTimeCt))
                    return DataType.Integer;

                if ((possibleDataTypes.ContainsKey(DataType.Double)) &&
                    (possibleDataTypes[DataType.Double] >= dateTimeCt))
                    return DataType.Double;
            }

            return possibleDataTypes.Keys.OrderByDescending(k => k).FirstOrDefault();
        }

        private void ValidateDataTable(DataTable dataTable)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable));

            if (dataTable.Columns?.None() == true)
                throw new ArgumentException($"[{nameof(dataTable)}] must have at least one header.", nameof(dataTable));

            if (dataTable.Rows?.None() == true)
                throw new ArgumentException($"[{nameof(dataTable)}] must have at least one row.", nameof(dataTable));
        }

        public class DataTableAnalysis
        {
            public DataTableAnalysis()
            {
                Columns = new Dictionary<string, DataColumnAnalysis>();
            }

            public int RowCount { get; set; }
            public Dictionary<string, DataColumnAnalysis> Columns { get; set; }
        }

        public class DataColumnAnalysis
        {
            public DataColumnAnalysis()
            {
                PossibleColumnDataTypes = new Dictionary<DataType, int>
                {
                    [DataType.Guid] = 0,
                    [DataType.Undefined] = 0,
                    [DataType.Boolean] = 0,
                    [DataType.DateTime] = 0,
                    [DataType.DateTimeOffset] = 0,
                    [DataType.Double] = 0,
                    [DataType.Integer] = 0,
                    [DataType.String] = 0,
                    [DataType.TimeSpan] = 0
                };
            }

            public int ValueCount { get; set; }
            public Dictionary<DataType, int> PossibleColumnDataTypes { get; set; }
        }
    }
}