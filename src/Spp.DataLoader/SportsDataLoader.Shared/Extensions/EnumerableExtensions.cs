/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsDataLoader.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool ContainAny<T>(this IEnumerable<T> inSet, IEnumerable<T> anySet)
        {
            if (inSet == null)
                throw new ArgumentNullException(nameof(inSet));

            if (anySet == null)
                throw new ArgumentNullException(nameof(anySet));

            return anySet.Any(inSet.Contains);
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var sourceArray = source.ToArray();

            for (var i = 0; i < sourceArray.Length; i += chunkSize)
                yield return sourceArray.Skip(i).Take(chunkSize);
        }

        public static IEnumerable<IGrouping<T, T>> Group<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.GroupBy(i => i);
        }

        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> condition = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return (source.Any(condition ?? (t => true)) == false);
        }
    }
}