/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Serilog;

namespace SportsDataLoader.Shared.Interfaces
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger<T>();
    }
}