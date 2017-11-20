/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
namespace SportsDataLoader.Model.Sql
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WorkoutExercise")]
    public partial class WorkoutExercise
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public int ExerciseId { get; set; }

        public int WorkoutId { get; set; }

        public virtual Exercise Exercise { get; set; }

        public virtual Workout Workout { get; set; }
    }
}
