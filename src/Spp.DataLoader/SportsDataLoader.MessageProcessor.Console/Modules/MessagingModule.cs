/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Ninject.Modules;
using SportsDataLoader.MessageProcessor.Processors;
using SportsDataLoader.Messaging.Azure.Interfaces;
using SportsDataLoader.Messaging.Azure.Receivers;
using SportsDataLoader.Messaging.Azure.Senders;
using SportsDataLoader.Messaging.Azure.Subscribers;
using SportsDataLoader.Messaging.Interfaces;
using SportsDataLoader.Model.Events;

namespace SportsDataLoader.MessageProcessor.Console.Modules
{
    public class MessagingModule : NinjectModule
    {
        public override void Load()
        {
            LoadBrokeredMessageSubscribers();
            LoadMessageProcessors();
            LoadMessageReceiver();
            LoadMessageSenders();
        }

        private void LoadBrokeredMessageSubscribers()
        {
            LoadBrokeredMessageSubscriber<FileUploaded>();
            LoadBrokeredMessageSubscriber<FileAnalyzed>();
        }

        private void LoadBrokeredMessageSubscriber<T>() where T : IMessage
        {
            Bind<IBrokeredMessageSubscriber>()
                .To<BrokeredMessageSubscriber<T>>()
                .InSingletonScope()
                .Named(typeof(T).ToString());
        }

        private void LoadMessageProcessors()
        {
            Bind<IMessageProcessor<FileUploaded>>()
                .To<FileAnalysisMessageProcessor>()
                .InTransientScope();
        }

        private void LoadMessageReceiver()
        {
            Bind<AzureServiceBusSubscriptionMessageReceiver.IConfiguration>()
                .To<AzureServiceBusSubscriptionMessageReceiver.LocalConfiguration>()
                .InSingletonScope();

            Bind<IMessageReceiver>()
                .To<AzureServiceBusSubscriptionMessageReceiver>()
                .InTransientScope();
        }

        private void LoadMessageSenders()
        {
            LoadMessageSender<FileUploaded>();
            LoadMessageSender<FileAnalyzed>();
        }

        private void LoadMessageSender<T>() where T : IMessage
        {
            Bind<AzureServiceBusTopicMessageSender<T>.IConfiguration>()
                .To<AzureServiceBusTopicMessageSender<T>.LocalConfiguration>()
                .InSingletonScope();

            Bind<IMessageSender<T>>()
                .To<AzureServiceBusTopicMessageSender<T>>()
                .InTransientScope();
        }
    }
}