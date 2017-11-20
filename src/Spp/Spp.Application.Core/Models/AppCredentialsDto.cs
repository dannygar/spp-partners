/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public class AppCredentialsDto
    {
        public string AADInstance { get; set; }
        public string Tenant { get; set; }
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string ClientKey { get; set; }
        public string Token { get; set; }
    }
}
