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
    public class AthleteQuestion
    {
        public int Id { get; set; }
        public String Text { get; set; }
        public int SequenceOrder { get; set; }
        public KeyValuePair<string, int> MinCaptionValue { get; set; }
        public KeyValuePair<string, int> MidCaptionValue { get; set; }
        public KeyValuePair<string, int> MaxCaptionValue { get; set; }

    }
}
