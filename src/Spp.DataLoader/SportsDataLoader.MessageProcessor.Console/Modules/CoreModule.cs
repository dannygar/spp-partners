/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Ninject.Modules;
using SportsDataLoader.Shared;
using SportsDataLoader.Shared.Interfaces;

namespace SportsDataLoader.MessageProcessor.Console.Modules
{
    public class CoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IDirectory<>))
                .To(typeof(Directory<>))
                .InSingletonScope();
        }
    }
}