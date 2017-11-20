/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("Session")]
    public class Session : EntityBase
    {
        public int Type { get; set; }

        public DateTime StartTime { get; set; }

        [ForeignKey("Location")]
        public int? LocationId { get; set; }

        public Location Location { get; set; }
    }
}
