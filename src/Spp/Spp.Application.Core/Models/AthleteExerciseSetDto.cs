/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public class AthleteExerciseSetDto : ModelBase
    {
        public int Order { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public float Weight { get; set; }
        public int? RecoveryTimeInMin { get; set; }
        public int TrainingLoad { get; set; }
    }
}
