/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("Message")]
    public class Message : EntityBase
    {
        [Required]
        public string Text { get; set; }

        public DateTime Sent { get; set; }

        [ForeignKey("Sender")]
        public int FromId { get; set; }

        public int ToId { get; set; }

        public bool IsActive { get; set; }

        //Foreign key
        public User Sender { get; set; }
    }
}
