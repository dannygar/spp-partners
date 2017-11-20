/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Spp.Application.Core.Models
{
    public class AthleteWorkoutDto : ModelBase
    {
        public int SessionId { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public string SubTopic { get; set; }
        public string Category { get; set; }
        public IList<AthleteExerciseDto> Exercises { get; set; }
        public SessionDto Session { get; set; }
    }
}
