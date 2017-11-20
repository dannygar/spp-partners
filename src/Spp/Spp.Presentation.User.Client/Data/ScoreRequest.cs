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
    public class ScoreRequest
    {
        public Inputs Inputs { get; set; }
        public Dictionary<string, string> GlobalParameters { get; set; }

        public ScoreRequest()
        {
            GlobalParameters = new Dictionary<string, string>();
            Inputs = new Inputs();
        }
    }

    public class Input1
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }

    public class Inputs
    {
        public Input1 input1 { get; set; }
    }
    
}
