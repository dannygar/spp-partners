/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("UserEmotion")]
    public partial class UserEmotion : EntityBase
    {
        public int UserId { get; set; }

        public double Anger { get; set; }

        public double Contempt { get; set; }

        public double Disgust { get; set; }

        public double Fear { get; set; }

        public double Happiness { get; set; }

        public double Neutral { get; set; }

        public double Sadness { get; set; }

        public double Surprise { get; set; }

        public DateTime TakenOn { get; set; }
    }
}
