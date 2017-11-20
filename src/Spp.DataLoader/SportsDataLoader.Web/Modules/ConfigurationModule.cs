/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Ninject.Modules;
using SportsDataLoader.Web.Configuration;
using SportsDataLoader.Web.Interfaces;

namespace SportsDataLoader.Web.Modules
{
    public class ConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IWebConfiguration>()
                .To<LocalWebConfiguration>()
                .InSingletonScope();
        }
    }
}