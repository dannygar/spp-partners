/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Ninject.Modules;
using SportsDataLoader.FileProcessing.Factories;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Processors;
using SportsDataLoader.FileProcessing.Sql.Importers;
using SportsDataLoader.FileProcessing.Sql.Interfaces;
using SportsDataLoader.FileProcessing.Sql.Providers;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Constants;

namespace SportsDataLoader.MessageProcessor.Console.Modules
{
    public class ImportProcessorModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataTableImportProcessorFactory>()
                .To<DataTableImportProcessorFactory>()
                .InTransientScope();

            Bind<SqlConnectionStringProvider.IConfiguration>()
                .To<SqlConnectionStringProvider.LocalConfiguration>()
                .InTransientScope();

            Bind<ISqlConnectionStringProvider>()
                .To<SqlConnectionStringProvider>()
                .InTransientScope();

            LoadImporters();
            LoadImportProcessors();
        }

        private void LoadImporters()
        {
            Bind<CustomDataEntitySqlImporter.IConfiguration>()
                .To<CustomDataEntitySqlImporter.LocalConfiguration>()
                .InSingletonScope();

            Bind<IImporter<CustomDataEntity>>()
                .To<CustomDataEntitySqlImporter>()
                .InTransientScope();
        }

        private void LoadImportProcessors()
        {
            Bind<IDataTableImportProcessor>()
                .To<CustomDataEntityImportProcessor>()
                .InTransientScope()
                .Named(SchemaTypes.Custom);
        }
    }
}