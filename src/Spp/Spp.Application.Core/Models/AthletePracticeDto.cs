/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Spp.Application.Core.Models
{
    public class AthletePracticeDto
    {
        public string Topic { get; set; }
        public string SubTopic { get; set; }
        public int SessionId { get; set; }
        public int? EstimatedTrainingLoad { get; set; }
        public List<AthleteDrillDto> Drills { get; set; }
    }
}
