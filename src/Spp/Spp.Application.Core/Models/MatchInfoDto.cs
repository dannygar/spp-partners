/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Spp.Application.Core.Models
{
    public class MatchInfoDto
    {
        public int? FirstTeamId { get; set; }

        public int? SecondTeamId { get; set; }

        public DateTime? DateTime { get; set; }

        public int? LocationId { get; set; }

        public int? FirstTeamScore { get; set; }

        public int? SecondTeamScore { get; set; }

        public TeamDto FirstTeam { get; set; }

        public TeamDto SecondTeam { get; set; }

        public LocationDto Location { get; set; }
    }
}
