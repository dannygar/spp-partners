/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Data.Services
{
    public interface ICacheService
    {
        T GetItem<T>(string cacheKey);

        void SetItem<T>(string cacheKey, T item);

        bool IsCached(string cacheKey);

        void ClearCache(string cacheKey);
    }
}
