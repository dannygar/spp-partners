/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using SportsDataLoader.Messaging.Azure.Constants;
using SportsDataLoader.Messaging.Interfaces;
using SportsDataLoader.Shared.Interfaces;

namespace SportsDataLoader.Messaging.Azure.Senders
{
    public class AzureServiceBusTopicMessageSender<T> : IMessageSender<T> where T : IMessage
    {
        private readonly string messageType;
        private readonly TopicClient topicClient;

        public AzureServiceBusTopicMessageSender(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            messageType = typeof(T).ToString();
            topicClient = CreateTopicClient(configuration);
        }

        public async Task SendMessage(T message)
        {
            await
                topicClient.SendAsync(new BrokeredMessage(message)
                {
                    Properties = { [MessageProperties.MessageType] = messageType }
                });
        }

        private TopicClient CreateTopicClient(IConfiguration configuration)
        {
            var namespaceManager =
                NamespaceManager.CreateFromConnectionString(configuration.ServiceBusConnectionString);

            if (namespaceManager.TopicExists(configuration.TopicName) == false)
                namespaceManager.CreateTopic(configuration.TopicName);

            return TopicClient.CreateFromConnectionString(configuration.ServiceBusConnectionString,
                                                          configuration.TopicName);
        }

        public interface IConfiguration
        {
            string ServiceBusConnectionString { get; }
            string TopicName { get; }
        }

        public class Configuration : IConfiguration
        {
            public string ServiceBusConnectionString { get; set; }
            public string TopicName { get; set; }
        }

        public class LocalConfiguration : IConfiguration
        {
            public const string ServiceBusConnectionStringConfigurationKey = "ServiceBusConnectionString";

            public readonly string TopicNameConfigurationKey;

            public LocalConfiguration()
            {
                var typeName = typeof(T).Name;

                TopicNameConfigurationKey = $"{typeName}.SenderTopicName";

                ConfigureServiceBusConnectionString();
                ConfigureTopicName();
            }

            public string ServiceBusConnectionString { get; private set; }
            public string TopicName { get; private set; }

            private void ConfigureServiceBusConnectionString()
            {
                var sbConnectionString =
                    ConfigurationManager.ConnectionStrings[ServiceBusConnectionStringConfigurationKey];

                if (string.IsNullOrEmpty(sbConnectionString?.ConnectionString))
                {
                    throw new ConfigurationErrorsException(
                        $"[{ServiceBusConnectionStringConfigurationKey}] not configured.");
                }

                ServiceBusConnectionString = sbConnectionString.ConnectionString;
            }

            private void ConfigureTopicName()
            {
                var topicName = ConfigurationManager.AppSettings[TopicNameConfigurationKey];

                if (string.IsNullOrEmpty(topicName))
                    throw new ConfigurationErrorsException($"[{TopicNameConfigurationKey}] not configured.");

                TopicName = topicName;
            }
        }
    }
}