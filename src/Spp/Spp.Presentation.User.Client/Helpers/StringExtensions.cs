/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Presentation.User.Client.Helpers
{
    using System;

    /// <summary> Provides useful helper methods for working with string. </summary>
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        /// <summary>
        /// Parse string to nullable integer.
        /// </summary>
        /// <param name="source">A source string.</param>
        /// <returns>Nullable int value parsed.</returns>
        public static int? ToNullableInt(this string source)
        {
            if (source == null || source.ToLower() == "null")
            {
                return null;
            }

            return int.Parse(source);
        }
    }
}
