/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Threading.Tasks;

namespace SportsDataLoader.Messaging.Interfaces
{
    public interface IMessageReceiver
    {
        Task StartReceiving();
    }
}