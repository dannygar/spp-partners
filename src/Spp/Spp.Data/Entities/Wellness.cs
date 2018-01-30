/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("Wellness")]
    public class Wellness
    {
        [Key]
        public int Season_ID { get; set; }

        public string Season { get; set; }

        [Key]
        public int Game_ID { get; set; }

        [Key]
        public int Player_ID { get; set; }

        public int Energy { get; set; }

        public int Sleep { get; set; }

        public int Stress { get; set; }

        public int Soreness { get; set; }

        public double Average_Wellness { get; set; }

        public double ReadinessProbability { get; set; }

        public int Readiness { get; set; }
    }
}
