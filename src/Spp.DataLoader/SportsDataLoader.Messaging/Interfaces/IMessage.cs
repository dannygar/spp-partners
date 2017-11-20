/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace SportsDataLoader.Messaging.Interfaces
{
    public interface IMessage
    {
        string Id { get; set; }
        string TenantId { get; set; }
    }
}