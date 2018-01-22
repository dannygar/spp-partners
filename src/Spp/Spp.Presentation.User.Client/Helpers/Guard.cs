/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Presentation.User.Client.Helpers
{
    using System;

    /// <summary>Heplers aimed to protect entities contracts. </summary>
    public static class Guard
    {
        /// <summary>Performs nullability check.</summary>
        /// <example>Guard.ThrowIfNull(action, nameof(action));</example>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="name">Checked property name.</param>
        /// <exception cref="System.ArgumentNullException">In case if value equals null.</exception>
        public static void ThrowIfNull<T>(T value, string name)
            where T : class
        {
            if (value == null) throw new ArgumentNullException(name);
        }
    }
}
