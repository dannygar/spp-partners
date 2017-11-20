/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Serilog;
using SportsDataLoader.Messaging.Azure.Constants;
using SportsDataLoader.Messaging.Azure.Interfaces;
using SportsDataLoader.Messaging.Interfaces;
using SportsDataLoader.Shared.Interfaces;

namespace SportsDataLoader.Messaging.Azure.Receivers
{
    public class AzureServiceBusSubscriptionMessageReceiver : IMessageReceiver
    {
        private readonly IConfiguration configuration;
        private readonly ILogger logger;
        private readonly IDirectory<IBrokeredMessageSubscriber> subscriberDirectory;

        public AzureServiceBusSubscriptionMessageReceiver(IConfiguration configuration,
                                                          ILogger logger,
                                                          IDirectory<IBrokeredMessageSubscriber> subscriberDirectory)
        {
            this.logger = logger.ForContext<AzureServiceBusSubscriptionMessageReceiver>();

            this.configuration = configuration;
            this.subscriberDirectory = subscriberDirectory;

            this.logger.Debug("Azure Service Bus Subscription Message Receiver Configuration: [{@configuration}]",
                              configuration);
        }

        public Task StartReceiving()
        {
            logger.Debug("Starting to receive messages...");

            Start();

            return Task.FromResult(0);
        }

        private void Start()
        {
            var subscriptionClient = CreateSubscriptionClient(configuration);

            var messageOptions = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = configuration.AutoRenewTimeout
            };

            subscriptionClient.OnMessageAsync(OnMessage, messageOptions);
        }

        private async Task OnMessage(BrokeredMessage brokeredMessage)
        {
            try
            {
                var messageId = brokeredMessage.MessageId;

                try
                {
                    logger.Debug("Message [{messageId}] received.", brokeredMessage.MessageId);

                    if (brokeredMessage.DeliveryCount >= configuration.DeadLetterDeliveryCount)
                    {
                        logger.Warning("Message [{messageId}] may be a poison message [delivery count: {deliveryCount}]. " +
                                       "Sending message to dead letter queue...", messageId, brokeredMessage.DeliveryCount);

                        await brokeredMessage.DeadLetterAsync();
                    }
                    else
                    {
                        if (brokeredMessage.Properties.ContainsKey(MessageProperties.MessageType) == false)
                        {
                            throw new InvalidOperationException(
                                $"Unable to process message [{brokeredMessage.MessageId}]. " +
                                $"[{MessageProperties.MessageType}] property not configured.");
                        }

                        var messageType = brokeredMessage.Properties[MessageProperties.MessageType].ToString();

                        logger.Debug("Message [{messageId}] is [{messageType}].", messageId, messageType);

                        var subscriber = subscriberDirectory[messageType];

                        await subscriber.ProcessMessage(brokeredMessage).ConfigureAwait(false);
                        await brokeredMessage.CompleteAsync().ConfigureAwait(false);

                        logger.Debug("Message [{messageId}] processing complete.", messageId);
                    }
                }
                catch
                {
                    logger.Error("An error occurred while processing message [{meessageId}]. " +
                                 "Abandoning message...",
                                 brokeredMessage.MessageId);

                    await brokeredMessage.AbandonAsync().ConfigureAwait(false);

                    throw;
                }
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred while processing a message: [{@ex}].", ex);
            }
        }

        private SubscriptionClient CreateSubscriptionClient(IConfiguration configuration)
        {
            var namespaceManager =
                NamespaceManager.CreateFromConnectionString(configuration.ServiceBusConnectionString);

            if (namespaceManager.TopicExists(configuration.TopicName) == false)
                namespaceManager.CreateTopic(configuration.TopicName);

            if (namespaceManager.SubscriptionExists(configuration.TopicName, configuration.SubscriptionName) == false)
                namespaceManager.CreateSubscription(configuration.TopicName, configuration.SubscriptionName);

            return SubscriptionClient.CreateFromConnectionString(configuration.ServiceBusConnectionString,
                                                                 configuration.TopicName, configuration.SubscriptionName);
        }

        public interface IConfiguration
        {
            int DeadLetterDeliveryCount { get; }

            string ServiceBusConnectionString { get; }
            string SubscriptionName { get; }
            string TopicName { get; }

            TimeSpan AutoRenewTimeout { get; }
        }

        public class Configuration : IConfiguration
        {
            public int DeadLetterDeliveryCount { get; set; }

            public string ServiceBusConnectionString { get; set; }
            public string SubscriptionName { get; set; }
            public string TopicName { get; set; }

            public TimeSpan AutoRenewTimeout { get; set; }
        }

        public class LocalConfiguration : IConfiguration
        {
            public const string AutoRenewTimeoutConfigurationKey = "ReceiverAutoRenewTimeout";
            public const string DeadLetterDeliveryCountConfigurationKey = "ReceiverDeadLetterDeliveryCount";
            public const string ServiceBusConnectionStringConfigurationKey = "ServiceBusConnectionString";
            public const string SubscriptionNameConfigurationKey = "ReceiverSubscriptionName";
            public const string TopicNameConfigurationKey = "ReceiverTopicName";

            public readonly TimeSpan DefaultAutoRenewTimeout = TimeSpan.FromMinutes(10);
            public readonly int DefaultDeadLetterDeliveryCount = 5;

            public LocalConfiguration()
            {
                ConfigureServiceBusConnectionString();
                ConfigureSubscriptionName();
                ConfigureTopicName();
                ConfigureAutoRenewTimeout();
                ConfigureDeadLetterDeliveryCount();
            }

            public int DeadLetterDeliveryCount { get; private set; }

            public string ServiceBusConnectionString { get; private set; }
            public string SubscriptionName { get; private set; }
            public string TopicName { get; private set; }

            public TimeSpan AutoRenewTimeout { get; private set; }

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

            private void ConfigureSubscriptionName()
            {
                var subscriptionName = ConfigurationManager.AppSettings[SubscriptionNameConfigurationKey];

                if (string.IsNullOrEmpty(subscriptionName))
                    throw new ConfigurationErrorsException($"[{SubscriptionNameConfigurationKey}] not configured.");

                SubscriptionName = subscriptionName;
            }

            private void ConfigureTopicName()
            {
                var topicName = ConfigurationManager.AppSettings[TopicNameConfigurationKey];

                if (string.IsNullOrEmpty(topicName))
                    throw new ConfigurationErrorsException($"[{TopicNameConfigurationKey}] not configured.");

                TopicName = topicName;
            }

            private void ConfigureDeadLetterDeliveryCount()
            {
                var deadLetterDeliveryCountString =
                    ConfigurationManager.AppSettings[DeadLetterDeliveryCountConfigurationKey];

                if (string.IsNullOrEmpty(deadLetterDeliveryCountString))
                {
                    DeadLetterDeliveryCount = DefaultDeadLetterDeliveryCount;
                }
                else
                {
                    int deadLetterDeliveryCount;

                    if (int.TryParse(deadLetterDeliveryCountString, out deadLetterDeliveryCount) == false)
                    {
                        throw new ConfigurationErrorsException(
                            $"[{DeadLetterDeliveryCountConfigurationKey}] is invalid. " +
                            $"[{deadLetterDeliveryCountString}] is not a valid int.");
                    }

                    DeadLetterDeliveryCount = deadLetterDeliveryCount;
                }
            }

            private void ConfigureAutoRenewTimeout()
            {
                var autoRenewTimeoutString = ConfigurationManager.AppSettings[AutoRenewTimeoutConfigurationKey];

                if (string.IsNullOrEmpty(autoRenewTimeoutString))
                {
                    AutoRenewTimeout = DefaultAutoRenewTimeout;
                }
                else
                {
                    TimeSpan autoRenewTimeout;

                    if (TimeSpan.TryParse(autoRenewTimeoutString, out autoRenewTimeout) == false)
                    {
                        throw new ConfigurationErrorsException($"[{AutoRenewTimeoutConfigurationKey}] is invalid. " +
                                                               $"[{autoRenewTimeoutString}] is not a valid TimeSpan.");
                    }

                    AutoRenewTimeout = autoRenewTimeout;
                }
            }
        }
    }
}