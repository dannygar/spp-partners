/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Ninject.Modules;
using SportsDataLoader.Web.Common.Intefaces;
using SportsDataLoader.Web.Common.Services;

namespace SportsDataLoader.WebApi.Modules
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataLoaderWebService>()
                .To<DataLoaderWebService>()
                .InTransientScope();
        }
    }
}