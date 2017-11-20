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

    [Table("PracticeDrill")]
    public partial class PracticeDrill
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public int DrillId { get; set; }

        public int PracticeId { get; set; }

        public virtual Drill Drill { get; set; }

        public virtual Practice Practice { get; set; }
    }
}
