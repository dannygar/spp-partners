/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Serilog;
using SportsDataLoader.Messaging.Azure.Interfaces;
using SportsDataLoader.Messaging.Interfaces;

namespace SportsDataLoader.Messaging.Azure.Subscribers
{
    public class BrokeredMessageSubscriber<T> : IBrokeredMessageSubscriber where T : IMessage
    {
        private readonly ILogger logger;
        private readonly IMessageProcessor<T> messageProcessor;

        public BrokeredMessageSubscriber(ILogger logger,
                                         IMessageProcessor<T> messageProcessor)
        {
            this.logger = logger.ForContext<BrokeredMessageSubscriber<T>>();
            this.messageProcessor = messageProcessor;
        }

        public async Task ProcessMessage(BrokeredMessage brokeredMessage)
        {
            try
            {
                if (brokeredMessage == null)
                    throw new ArgumentNullException(nameof(brokeredMessage));

                var messageBody = brokeredMessage.GetBody<T>();

                logger.Debug("Processing message [{@messageBody}]...", messageBody);

                await messageProcessor.ProcessMessage(messageBody);

                logger.Debug("Successfully processed message [{messageId}].", brokeredMessage.MessageId);
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred while processing message [{messageId}]: [{@ex}].",
                             brokeredMessage?.MessageId, ex);

                throw;
            }
        }
    }
}