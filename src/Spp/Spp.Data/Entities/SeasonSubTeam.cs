/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("SeasonSubTeam")]
    public class SeasonSubTeam : EntityBase
    {
        public int SubTeamId { get; set; }
        public int SeasonId { get; set; }
    }
}
