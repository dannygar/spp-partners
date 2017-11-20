/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace SportsDataLoader.Messaging.Azure.Interfaces
{
    public interface IBrokeredMessageSubscriber
    {
        Task ProcessMessage(BrokeredMessage brokeredMessage);
    }
}