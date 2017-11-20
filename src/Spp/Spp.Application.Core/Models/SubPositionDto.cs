/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public class SubPositionDto : ModelBase
    {
        public string Name { get; set; }
        public int PositionId { get; set; }
        public string Abbreviation { get; set; }
    }
}
