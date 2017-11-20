/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Helpers
{
    using System;
    using System.Collections.Generic;

    /// <summary> Provides useful helper methods for working with collections. </summary>
    public static class EnumerableExtensions
    {
        /// <summary>Applies specified action to every collection element with providing current element index. </summary>
        /// <example>Values.ForEach((x, i) => x.Id = i);</example>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            Guard.ThrowIfNull(action, nameof(action));

            var i = 0;
            foreach (var item in collection)
            {
                action(item, i++);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            Guard.ThrowIfNull(action, nameof(action));

            foreach (var item in collection)
            {
                action(item);
            }
        }
    }
}
