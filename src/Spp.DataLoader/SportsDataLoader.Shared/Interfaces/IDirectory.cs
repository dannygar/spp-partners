/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace SportsDataLoader.Shared.Interfaces
{
    public interface IDirectory<out T>
    {
        T this[string name] { get; }
    }
}