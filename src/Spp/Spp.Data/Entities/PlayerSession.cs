/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("PlayerSession")]
    public class PlayerSession : EntityBase
    {
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public int? PlayerId { get; set; }

        public string NoTrainReason { get; set; }
    }
}
