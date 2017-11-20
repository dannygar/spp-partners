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

    [Table("ExerciseData")]
    public partial class ExerciseData
    {
        public int Id { get; set; }

        public bool Done { get; set; }

        public bool Modified { get; set; }

        public int ExerciseId { get; set; }

        public int WorkoutDataId { get; set; }

        public virtual Exercise Exercise { get; set; }

        public virtual WorkoutData WorkoutData { get; set; }
    }
}
