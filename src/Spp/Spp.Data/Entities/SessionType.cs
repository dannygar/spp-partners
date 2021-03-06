/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("Type")]
    public class SessionType : EntityBase
    {
        [Column(TypeName = "text")]
        [Required]
        public string Description { get; set; }
    }
}
