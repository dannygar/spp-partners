/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Spp.Application.Core.Models
{
    public class MesocycleDto : ModelBase
    {
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? SeasonId { get; set; }
        public int? SubTeamId { get; set; }
        public int? TeamId { get; set; }
        public int? MesocycleNumber { get; set; }
    }
}
