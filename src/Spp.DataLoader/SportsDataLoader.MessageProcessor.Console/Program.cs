/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Threading;
using Ninject;
using Serilog;
using SportsDataLoader.MessageProcessor.Console.Modules;
using SportsDataLoader.Messaging.Interfaces;

namespace SportsDataLoader.MessageProcessor.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var resetEvent = new ManualResetEvent(false);

            var kernel = new StandardKernel(new ProcessorModule(),
                                            new CoreModule(),
                                            new ImportProcessorModule(),
                                            new LoggingModule(),
                                            new MessagingModule(),
                                            new ParserModule(),
                                            new RepositoryModule());

            var logger = kernel.Get<ILogger>().ForContext<Program>();

            logger.Debug("Starting processor...");

            var messageReceiver = kernel.Get<IMessageReceiver>();

            messageReceiver.StartReceiving();
            resetEvent.WaitOne();
        }
    }
}