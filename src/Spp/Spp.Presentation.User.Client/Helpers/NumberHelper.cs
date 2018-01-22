/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
// <copyright file="NumberHelper.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Spp.Presentation.User.Client.Helpers
{
    public class NumberHelper
    {
        private const int MaxPercent = 100;

        public static int ValueAsPercent(int value)
        {
            return value > MaxPercent
                ? MaxPercent
                : value;
        }
    }
}
