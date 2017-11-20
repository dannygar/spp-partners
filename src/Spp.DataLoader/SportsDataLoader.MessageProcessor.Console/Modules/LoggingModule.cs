/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Configuration;
using Ninject.Modules;
using Serilog;

namespace SportsDataLoader.MessageProcessor.Console.Modules
{
    public class LoggingModule : NinjectModule
    {
        private readonly string storageConnectionString;

        public LoggingModule()
        {
            storageConnectionString = ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
        }

        public override void Load()
        {
            Bind<ILogger>()
                .ToMethod(c => CreateLogger());
        }

        private ILogger CreateLogger()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.AzureTableStorageWithProperties(storageConnectionString,
                                                         storageTableName: "ProcessorLog")
                .WriteTo.ColoredConsole()
                .CreateLogger();
        }
    }
}