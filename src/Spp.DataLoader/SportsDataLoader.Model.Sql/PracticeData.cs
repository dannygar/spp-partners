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

    [Table("PracticeData")]
    public partial class PracticeData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PracticeData()
        {
            DrillDatas = new HashSet<DrillData>();
        }

        public int Id { get; set; }

        public int? NoteId { get; set; }

        public int PracticeId { get; set; }

        public int SessionId { get; set; }

        public int DurationInMinutes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DrillData> DrillDatas { get; set; }

        public virtual Note Note { get; set; }

        public virtual Practice Practice { get; set; }

        public virtual Session Session { get; set; }
    }
}
