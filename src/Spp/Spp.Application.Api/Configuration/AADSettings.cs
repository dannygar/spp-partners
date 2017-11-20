/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Api.Configuration
{
    public class AADSettings
    {
        public string AadInstance { get; set; }
        public string Tenant { get; set; }
        public string Audience { get; set; }
    }

    public class B2CSettings
    {
        public static string AadInstance { get; set; }
        public static string AadGraphResourceId { get; set; }
        public static string AadGraphEndpoint { get; set; }
        public static string AadGraphSuffix { get; set; }
        public static string AadGraphVersion { get; set; }
        public static string AadTenant { get; set; }
        public static string AadClientId { get; set; }
        public static string AadClientSecret { get; set; }
        public static string AadDefaultPassword { get; set; }
    }
}
