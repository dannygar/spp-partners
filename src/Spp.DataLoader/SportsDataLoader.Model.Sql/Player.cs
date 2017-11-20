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

    [Table("Player")]
    public partial class Player
    {
        public int Id { get; set; }

        public int? ShirtNumber { get; set; }

        public float Height { get; set; }

        public float Weight { get; set; }

        public int? Depth { get; set; }

        public int UserId { get; set; }

        public int PlayerPositionId { get; set; }

        public int PlayerSquadId { get; set; }

        public virtual PlayerPosition PlayerPosition { get; set; }

        public virtual PlayerSquad PlayerSquad { get; set; }

        public virtual User User { get; set; }
    }
}
