/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("TeamReadiness")]
    public class TeamReadiness : EntityBase
    {
        public int? TeamId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TrackedDate { get; set; }

        public int? Value { get; set; }
    }
}
