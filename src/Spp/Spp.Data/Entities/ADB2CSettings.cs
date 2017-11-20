/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("ADB2CSettings")]
    public class ADB2CSettings : EntityBase
    {
        public string AadInstance { get; set; }
        public string AadGraphResourceId { get; set; }
        public string AadGraphEndpoint { get; set; }
        public string AadGraphSuffix { get; set; }
        public string AadGraphVersion { get; set; }
        public string AadTenant { get; set; }
        public string AadClientId { get; set; }
        public string AadClientSecret { get; set; }
        public string AadDefaultPassword { get; set; }
    }
}
