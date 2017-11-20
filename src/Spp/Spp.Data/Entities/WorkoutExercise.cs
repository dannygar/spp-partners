/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("WorkoutExercise")]
    public class WorkoutExercise : EntityBase
    {
        [ForeignKey("Exercise")]
        public int ExerciseId { get; set; }

        public int SequenceNumber { get; set; }

        public Exercise Exercise { get; set; }
    }
}
