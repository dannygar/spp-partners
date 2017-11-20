/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;

namespace MicrosoftSportsScience.Data
{
    public class Practice
    {
        public int Id { get; set; }

        public int SessionId { get; set; }

        public string Name { get; set; }

        public string Topic { get; set; }

        public string SubTopic { get; set; }

        public int? EstimatedTrainingLoad { get; set; }

        public int? RecommendedTrainingLoad { get; set; }

        public string Side { get; set; }

        public bool Modified { get; set; }

        public int? CoachId { get; set; }

        public int? NoteId { get; set; }

        //Navigation Entities
        public IList<PracticeDrill> PracticeDrills { get; set; }
        public Note Note { get; set; }

    }
}