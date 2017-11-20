/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public class LocationDto : ModelBase
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public LocationType Type { get; set; }
    }
}
