/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("DayType")]
    public class DayType : EntityBase
    {
        [Column(TypeName = "text")]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Column(TypeName = "text")]
        public string RecurringDayofWeek { get; set; }

        public int? TeamId { get; set; }

        public int? SubTeamId { get; set; }
    }
}
