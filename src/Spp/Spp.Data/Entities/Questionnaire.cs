/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("Questionnaire")]
    public class Questionnaire : EntityBase
    {
        public string Name { get; set; }
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int? SequenceOrder { get; set; }

        public bool? isEnabled { get; set; }

        [ForeignKey("Session")]
        public int? SessionId { get; set; }

        [ForeignKey("Mesocycle")]
        public int? MesocycleId { get; set; }

        [ForeignKey("Microcycle")]
        public int? MicrocycleId { get; set; }

        [ForeignKey("DayType")]
        public int? DayTypeId { get; set; }

        //Navigation properties
        public Session Session { get; set; }
        public Mesocycle Mesocycle { get; set; }
        public Microcycle Microcycle { get; set; }
        public DayType DayType { get; set; }
    }
}
