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

    [Table("DrillData")]
    public partial class DrillData
    {
        public int Id { get; set; }

        public bool Done { get; set; }

        public bool Modified { get; set; }

        public int DrillId { get; set; }

        public int? NoteId { get; set; }

        public int PracticeDataId { get; set; }

        public virtual Drill Drill { get; set; }

        public virtual Note Note { get; set; }

        public virtual PracticeData PracticeData { get; set; }
    }
}
