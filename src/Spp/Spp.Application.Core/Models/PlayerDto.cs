﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public class PlayerDto : ModelBase
    {
        public int UserId { get; set; }
        public int? JerseyNumber { get; set; }
        public int? PositionId { get; set; }
        public int? SubPositionId { get; set; }
        public int? PlayerDepth { get; set; }
        public int? DominantSkillId { get; set; }
        public bool? IsWatchlist { get; set; }
        public bool IsResting { get; set; }
        public bool IsInjured { get; set; }
        public string Availability { get; set; }
        public string IsKeeper { get; set; }
        public CricketDto Cricket { get; set; }
        public PositionDto Position { get; set; }
    }
}
