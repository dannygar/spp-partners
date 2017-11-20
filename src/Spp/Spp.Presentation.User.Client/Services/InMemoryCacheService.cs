/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;

namespace MicrosoftSportsScience.Services
{
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


        public void SetItem<T>(string cacheKey, int hashKey, T item)
        {
            if (!this._internalCache.ContainsKey(cacheKey))
                this._internalCache.Add(cacheKey, item);
            else
            {
                var cachedItem = (Dictionary<int, object>)this._internalCache[cacheKey];

                if (cachedItem != null)
                {
                    if (!cachedItem.ContainsKey(hashKey))
                        cachedItem.Add(hashKey, item);
                    else
                        cachedItem[hashKey] = item;
                }

                //Update the cache
                this._internalCache[cacheKey] = cachedItem;

            }

            this._logService.Info("Cached object for key: " + cacheKey + " and the hash key: " + hashKey, this);
        }


        public void ClearCache(string cacheKey)
        {
            if (this._internalCache.ContainsKey(cacheKey))
                this._internalCache.Remove(cacheKey);
        }

    }
}
