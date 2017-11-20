/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("SubTeam")]
    public class SubTeam : EntityBase
    {
        public int TeamId { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [Column(TypeName = "text")]
        public string Name { get; set; }

        public int? GameMinutes { get; set; }

        [Column(TypeName = "text")]
        public string Abbreviation { get; set; }

        public int? LeagueId { get; set; }

        public int? CoachId { get; set; }

        public int? PlayerCaptainId { get; set; }
    }
}
