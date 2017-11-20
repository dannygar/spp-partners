/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Ninject.Modules;
using SportsDataLoader.Model.Azure.Repositories;
using SportsDataLoader.Model.Interfaces;

namespace SportsDataLoader.Web.Modules
{
    public class FileMetadataModule : NinjectModule
    {
        public override void Load()
        {
            Bind<AzureTableFileMetadataRepository.IConfiguration>()
                .To<AzureTableFileMetadataRepository.LocalConfiguration>()
                .InSingletonScope();

            Bind<IFileMetadataRepository>()
                .To<AzureTableFileMetadataRepository>()
                .InTransientScope();
        }
    }
}