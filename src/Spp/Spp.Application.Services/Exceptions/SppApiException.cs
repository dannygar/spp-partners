/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Spp.Application.Services.Exceptions
{
    public class SppApiException : Exception
    {
        private Exception ex;

        public SppApiException()
        {
        }

        public SppApiException(string message) : base(message)
        {
        }

        public SppApiException(Exception ex)
        {
            this.ex = ex;
        }

        public SppApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
