/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Ninject.Modules;
using SportsDataLoader.Messaging.Azure.Senders;
using SportsDataLoader.Messaging.Interfaces;
using SportsDataLoader.Model.Events;

namespace SportsDataLoader.Web.Modules
{
    public class MessagingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<AzureServiceBusTopicMessageSender<FileUploaded>.IConfiguration>()
                .To<AzureServiceBusTopicMessageSender<FileUploaded>.LocalConfiguration>()
                .InSingletonScope();

            Bind<IMessageSender<FileUploaded>>()
                .To<AzureServiceBusTopicMessageSender<FileUploaded>>()
                .InTransientScope();
        }
    }
}