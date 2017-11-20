/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsDataLoader.FileProcessing.Interfaces
{
    public interface IPrimaryKeyModelProvider<T>
    {
        Task<IEnumerable<IPrimaryKeyModel<T>>> GetPrimaryKeyModels(string tenantId);
    }
}