/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Extensions;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.FileProcessing.Normalizers;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Constants;
using SportsDataLoader.Model.Interfaces;

namespace SportsDataLoader.FileProcessing.Processors
{
    public class TabularFileAnalysisProcessor : IDelimitedTextFileAnalysisProcessor,
                                                IXlsxFileAnalysisProcessor
    {
        private readonly IDataTableAnalyzer dataTableAnalyzer;
        private readonly IDataTableScrubber[] dataTableScrubbers;
        private readonly IDelimitedTextFileDataTableParser delimitedTextFileDataTableParser;
        private readonly ISchemaRepository schemaRepository;
        private readonly IXlsxFileDataTableParser xlsxFileDataTableParser;

        public TabularFileAnalysisProcessor(IDataTableAnalyzer dataTableAnalyzer,
                                            IDataTableScrubber[] dataTableScrubbers,
                                            IDelimitedTextFileDataTableParser delimitedTextFileDataTableParser,
                                            ISchemaRepository schemaRepository,
                                            IXlsxFileDataTableParser xlsxFileDataTableParser)
        {
            this.dataTableAnalyzer = dataTableAnalyzer;
            this.dataTableScrubbers = dataTableScrubbers;
            this.delimitedTextFileDataTableParser = delimitedTextFileDataTableParser;
            this.schemaRepository = schemaRepository;
            this.xlsxFileDataTableParser = xlsxFileDataTableParser;
        }

        public int Specificity => 0;

        public async Task<IEnumerable<TabularFileAnalysis>> Process(FileMetadata fileMetadata,
                                                                    DelimitedTextFile delimitedTextFile)
        {
            if (fileMetadata == null)
                throw new ArgumentNullException(nameof(fileMetadata));

            if (delimitedTextFile == null)
                throw new ArgumentNullException(nameof(delimitedTextFile));

            var dataTable = await delimitedTextFileDataTableParser.Parse(delimitedTextFile);

            return await Process(fileMetadata, new[] {dataTable});
        }

        public bool IsFileCompatible(FileMetadata fileMetadata, DelimitedTextFile delimitedTextFile)
        {
            return true;
        }

        public async Task<IEnumerable<TabularFileAnalysis>> Process(FileMetadata fileMetadata, XlsxFile xlsxFile)
        {
            if (fileMetadata == null)
                throw new ArgumentNullException(nameof(fileMetadata));

            if (xlsxFile == null)
                throw new ArgumentNullException(nameof(xlsxFile));

            var dataTables = (await xlsxFileDataTableParser.Parse(xlsxFile)).ToList();

            if (dataTables.Count == 1)
                dataTables.First().Name = null;

            return await Process(fileMetadata, dataTables);
        }

        public bool IsFileCompatible(FileMetadata fileMetadata, XlsxFile xlsxFile)
        {
            return true;
        }

        public async Task<IEnumerable<TabularFileAnalysis>> Process(FileMetadata fileMetadata,
                                                                    IEnumerable<DataTable> dataTables)
        {
            if (fileMetadata == null)
                throw new ArgumentNullException(nameof(fileMetadata));

            if (dataTables == null)
                throw new ArgumentNullException(nameof(dataTables));

            var dataTableList = dataTables.ToList();
            var fileAnalysisList = new List<TabularFileAnalysis>();

            var cultureInfo = string.IsNullOrEmpty(fileMetadata.FileCultureCode)
                                  ? CultureInfo.GetCultureInfo(fileMetadata.FileCultureCode)
                                  : CultureInfo.CurrentCulture;

            foreach (var dataTable in dataTableList)
                fileAnalysisList.Add(await ProcessDataTable(fileMetadata, cultureInfo, dataTable));

            return fileAnalysisList;
        }

        private async Task<TabularFileAnalysis> ProcessDataTable(FileMetadata fileMetadata,
                                                                 CultureInfo cultureInfo,
                                                                 DataTable dataTable)
        {
            dataTable = await ScrubDataTable(dataTable, cultureInfo);

            var dataTableSchema = await GetSchema(dataTable, fileMetadata);
            var dataTableNormalizer = new DataTableNormalizer(cultureInfo, dataTableSchema);

            dataTable = dataTableNormalizer.Normalize(dataTable);

            fileMetadata.SetSchema(dataTableSchema);

            return new TabularFileAnalysis
            {
                FileMetadata = fileMetadata,
                Model = dataTable,
                Schema = dataTableSchema
            };
        }

        private async Task<Schema> GetSchema(DataTable dataTable, FileMetadata fileMetadata)
        {
            var dataTableAnalysis = await dataTableAnalyzer.AnalyzeDataTableAsync(dataTable, fileMetadata);

            var dataTableSchema = (await schemaRepository.GetAllTenantSchemas(fileMetadata.TenantId))
                .ToList()
                .FirstOrDefault(dataTableAnalysis.DoesConformToSchema);

            if (dataTableSchema == null)
            {
                dataTableSchema = CreateSchemaFromAnalysis(dataTableAnalysis, fileMetadata, dataTable);

                await schemaRepository.UpsertSchema(dataTableSchema);
            }

            return dataTableSchema;
        }

        private async Task<DataTable> ScrubDataTable(DataTable dataTable, CultureInfo cultureInfo)
        {
            foreach (var dataTableScrubber in dataTableScrubbers)
                dataTable = await dataTableScrubber.ScrubDataTable(dataTable, cultureInfo);

            return dataTable;
        }


        private Schema CreateSchemaFromAnalysis(Analysis analysis, FileMetadata fileMetadata, DataTable dataTable)
        {
            var schema = new Schema
            {
                SchemaId = Guid.NewGuid().ToString(),
                SchemaName = GetCustomSchemaName(fileMetadata, dataTable),
                SchemaType = SchemaTypes.Custom,
                TenantId = fileMetadata.TenantId
            };

            foreach (var columnAnalysis in analysis.Columns)
            {
                schema.SchemaColumns.Add(new SchemaColumn
                {
                    DataType = columnAnalysis.ColumnDataType,
                    Id = Guid.NewGuid().ToString(),
                    Name = columnAnalysis.ColumnName,
                    LocalizedNames = {[fileMetadata.FileCultureCode] = columnAnalysis.ColumnName},
                    SqlDataType = columnAnalysis.ColumnDataType.ToSqlDataTypeName()
                });
            }

            return schema;
        }

        private string GetCustomSchemaName(FileMetadata fileMetadata, DataTable dataTable)
        {
            var nameBuilder = new StringBuilder();
            var fileNameParts = fileMetadata.FileName.Split('.');

            nameBuilder.Append(fileNameParts.Length >= 3
                                   ? fileNameParts.Reverse().Skip(1).First()
                                   : Path.GetFileNameWithoutExtension(fileMetadata.FileName));

            if (string.IsNullOrEmpty(dataTable.Name) == false)
                nameBuilder.Append($"_{dataTable.Name}");

            return nameBuilder.ToString();
        }
    }
}