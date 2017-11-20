/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("Workout")]
    public class Workout : EntityBase
    {
        [Column(TypeName = "text")]
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Topic { get; set; }

        [Column(TypeName = "text")]
        public string SubTopic { get; set; }

        [Column(TypeName = "text")]
        public string Category { get; set; }
    }
}
