/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Ninject.Modules;
using SportsDataLoader.FileManagement.Azure.Repositories;
using SportsDataLoader.FileManagement.Interfaces;

namespace SportsDataLoader.Web.Modules
{
    public class FileStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<AzureBlobFileRepository.IConfiguration>()
                .To<AzureBlobFileRepository.LocalConfiguration>()
                .InSingletonScope();

            Bind<IFileRepository>()
                .To<AzureBlobFileRepository>()
                .InTransientScope();
        }
    }
}