/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("Squad")]
    public class Squad : EntityBase
    {
        [Column(TypeName = "text")]
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public int? GroupId { get; set; }
    }
}
