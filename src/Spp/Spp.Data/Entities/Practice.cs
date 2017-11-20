/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("Practice")]
    public class Practice : EntityBase
    {
        [ForeignKey("Session")]
        public int SessionId { get; set; }

        public string Name { get; set; }

        public string Topic { get; set; }

        public string SubTopic { get; set; }

        public int? EstimatedTrainingLoad { get; set; }

        public int? RecommendedTrainingLoad { get; set; }

        public string Side { get; set; }

        public bool Modified { get; set; }

        [ForeignKey("Coach")]
        public int? CoachId { get; set; }

        [ForeignKey("Note")]
        public int? NoteId { get; set; }

        //Navigation properties
        public Session Session { get; set; }
        public User Coach { get; set; }
        public Note Note { get; set; }

    }
}
