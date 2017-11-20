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

    [Table("Session")]
    public partial class Session
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Session()
        {
            QuestionResponses = new HashSet<QuestionResponse>();
            Questionnaires = new HashSet<Questionnaire>();
        }

        public int Id { get; set; }

        public int Type { get; set; }

        public DateTime StartTime { get; set; }

        public int? LocationId { get; set; }

        public int? MesocycleId { get; set; }

        public int? MicrocycleId { get; set; }

        public int? DayTypeId { get; set; }

        public virtual DayType DayType { get; set; }

        public virtual Location Location { get; set; }

        public virtual Mesocycle Mesocycle { get; set; }

        public virtual Microcycle Microcycle { get; set; }

        public virtual PracticeData PracticeData { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionResponse> QuestionResponses { get; set; }

        public virtual WorkoutData WorkoutData { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Questionnaire> Questionnaires { get; set; }
    }
}
