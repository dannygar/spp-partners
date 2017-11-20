/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public class PlayerMaxDto : ModelBase
    {
        public int PlayerId { get; set; }
        public float PeakSpeedMax { get; set; }
        public float PeakAccelerationMax { get; set; }
        public float MATMax { get; set; }

    }
}
