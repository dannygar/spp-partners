/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    public class TeamMatch : EntityBase
    {
        [ForeignKey("FirstTeam")]
        public int? FirstTeamId { get; set; }

        [ForeignKey("SecondTeam")]
        public int? SecondTeamId { get; set; }

        public DateTime? DateTime { get; set; }

        [ForeignKey("Location")]
        public int? LocationId { get; set; }

        public int? FirstTeamScore { get; set; }

        public int? SecondTeamScore { get; set; }

        //marks a foreign key and also points to the property holding the related entity
        public Team FirstTeam { get; set; }
        public Team SecondTeam { get; set; }
        public Location Location { get; set; }
    }
}
