/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftSportsScience.Data
{
    public class AthleteDrill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PlannedTrainingLoad { get; set; }
        public string Size { get; set; }
        public int NumberOfPLayers { get; set; }
        //public string Note { get; set; }
        public int DurationInMinutes { get; set; }
        public string ImageUrl { get; set; }
        public int PracticeDrillId { get; set; }

        public int PracticeId { get; set; }

        public int DrillId { get; set; }

        public int NoteId { get; set; }

        public bool IsModified { get; set; }

        public int? Duration { get; set; }

        public int? NumberOfPlayers { get; set; }

        public int Sequence { get; set; }

        public int? CalculatedTrainingLoad { get; set; }


        //Navigation Entities
        public Practice Practice { get; set; }
        public Drill Drill { get; set; }
        public Note Note { get; set; }

    }
}
