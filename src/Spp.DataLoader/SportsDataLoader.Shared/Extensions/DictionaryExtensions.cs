/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;

namespace SportsDataLoader.Shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue ElementAtOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            return (dictionary.ContainsKey(key) ? dictionary[key] : default(TValue));
        }
    }
}