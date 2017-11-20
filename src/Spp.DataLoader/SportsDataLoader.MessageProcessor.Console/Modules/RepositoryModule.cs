/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Ninject.Modules;
using SportsDataLoader.FileManagement.Azure.Repositories;
using SportsDataLoader.FileManagement.Interfaces;
using SportsDataLoader.Model.Azure.Repositories;
using SportsDataLoader.Model.Interfaces;

namespace SportsDataLoader.MessageProcessor.Console.Modules
{
    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<AzureTableFileMetadataRepository.IConfiguration>()
                .To<AzureTableFileMetadataRepository.LocalConfiguration>()
                .InSingletonScope();

            Bind<IFileMetadataRepository>()
                .To<AzureTableFileMetadataRepository>()
                .InTransientScope();

            Bind<AzureBlobFileRepository.IConfiguration>()
                .To<AzureBlobFileRepository.LocalConfiguration>()
                .InSingletonScope();

            Bind<IFileRepository>()
                .To<AzureBlobFileRepository>()
                .InTransientScope();

            Bind<AzureDocumentDbSchemaRepository.IConfiguration>()
                .To<AzureDocumentDbSchemaRepository.LocalConfiguration>()
                .InSingletonScope();

            Bind<ISchemaRepository>()
                .To<AzureDocumentDbSchemaRepository>()
                .InTransientScope();

            Bind<AzureBlobModelStagingRepository.IConfiguration>()
                .To<AzureBlobModelStagingRepository.LocalConfiguration>()
                .InSingletonScope();

            Bind<IModelStagingRepository>()
                .To<AzureBlobModelStagingRepository>()
                .InTransientScope();
        }
    }
}