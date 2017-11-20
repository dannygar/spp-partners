/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("Benchmark")]
    public class Benchmark : EntityBase
    {
        [Required]
        public string Controller { get; set; }

        [Required]
        public string Operation { get; set; }

        [Required]
        public string Method { get; set; }

        [Required]
        public int Time { get; set; }

        [Required]
        public DateTime LastRun { get; set; }
    }
}
