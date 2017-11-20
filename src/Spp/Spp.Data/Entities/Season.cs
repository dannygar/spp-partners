/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("Season")]
    public class Season : EntityBase
    {
        [Column(TypeName = "text")]
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public bool? isActive { get; set; }

        [Column(TypeName = "text")]
        public string Abbreviation { get; set; }

        public int? StartYear { get; set; }

        public int? EndYear { get; set; }
    }
}
