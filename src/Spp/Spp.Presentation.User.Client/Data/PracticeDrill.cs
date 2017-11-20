/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Data
{
    public class PracticeDrill
    {
        public int Id { get; set; }

        public int PracticeId { get; set; }

        public int DrillId { get; set; }

        public int NoteId { get; set; }

        public bool IsModified { get; set; }

        public int? Duration { get; set; }

        public string Size { get; set; }

        public int? NumberOfPlayers { get; set; }

        public int Sequence { get; set; }

        public int? CalculatedTrainingLoad { get; set; }


        //Navigation Entities
        public Drill Drill { get; set; }

        public Note Note { get; set; }

        public Practice Practice { get; set; }


    }
}