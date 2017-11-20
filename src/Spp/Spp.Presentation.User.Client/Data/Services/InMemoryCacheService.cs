/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Data.Services
{
    using System.Collections.Generic;

    class InMemoryCacheService : ICacheService
    {
        private Dictionary<string, object> _internalCache;
        private ILogService _logService;

        public InMemoryCacheService(ILogService logService)
        {
            this._logService = logService;
            this._internalCache = new Dictionary<string, object>();
        }

        public T GetItem<T>(string cacheKey)
        {
            if (this._internalCache.ContainsKey(cacheKey))
                return (T)this._internalCache[cacheKey];

            this._logService.Warning("Cache miss on key: " + cacheKey, this);
            return default(T);
        }

        public bool IsCached(string cacheKey)
        {
            return this._internalCache.ContainsKey(cacheKey);
        }

        public void SetItem<T>(string cacheKey, T item)
        {
            if (!this._internalCache.ContainsKey(cacheKey))
                this._internalCache.Add(cacheKey, item);
            else
                this._internalCache[cacheKey] = item;

            this._logService.Info("Cached object for key: " + cacheKey, this);
        }

        public void ClearCache(string cacheKey)
        {
            if (this._internalCache.ContainsKey(cacheKey))
                this._internalCache.Remove(cacheKey);
        }
    }
}
