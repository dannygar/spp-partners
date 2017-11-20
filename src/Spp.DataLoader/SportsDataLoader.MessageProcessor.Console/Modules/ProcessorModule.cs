/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Ninject.Modules;
using SportsDataLoader.FileProcessing.Analyzers;
using SportsDataLoader.FileProcessing.Factories;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Processors;
using SportsDataLoader.FileProcessing.Scrubbers;
using SportsDataLoader.FileProcessing.Services;

namespace SportsDataLoader.MessageProcessor.Console.Modules
{
    public class ProcessorModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileProcessingServiceFactory>()
                .To<FileProcessingServiceFactory>()
                .InTransientScope();

            Bind<IDataTableAnalyzer>()
                .To<DataTableAnalyzer>()
                .InTransientScope();

            LoadCustomAnalysisProcessors();
            LoadStandardAnalysisProcessors();
            LoadDataTableScrubbers();
            LoadProcessingServices();
        }

        private void LoadProcessingServices()
        {
            Bind<IDelimitedTextFileProcessingService<ICsvFileParser>>()
                .To<DelimitedTextFileProcessingService<ICsvFileParser>>()
                .InTransientScope();

            Bind<IDelimitedTextFileProcessingService<ITsvFileParser>>()
                .To<DelimitedTextFileProcessingService<ITsvFileParser>>()
                .InTransientScope();

            Bind<IXlsxFileProcessingService>()
                .To<XlsxFileProcessingService>()
                .InTransientScope();

            Bind<IZipFileProcessingService>()
                .To<ZipFileProcessingService>()
                .InTransientScope();
        }

        private void LoadStandardAnalysisProcessors()
        {
            Bind<IDelimitedTextFileAnalysisProcessor>()
                .To<TabularFileAnalysisProcessor>()
                .InTransientScope();

            Bind<IXlsxFileAnalysisProcessor>()
                .To<TabularFileAnalysisProcessor>()
                .InTransientScope();

            Bind<IZipFileProcessor>()
                .To<ZipFileProcessor>()
                .InTransientScope();
        }

        private void LoadCustomAnalysisProcessors()
        {
        }

        private void LoadDataTableScrubbers()
        {
            Bind<IDataTableScrubber>()
                .To<JunkValueDataTableScrubber>()
                .InTransientScope();

            Bind<IDataTableScrubber>()
                .To<NumericUnitDataTableScrubber>()
                .InTransientScope();

            Bind<IDataTableScrubber>()
                .To<TimeZoneCodeDataTableScrubber>()
                .InTransientScope();
        }
    }
}