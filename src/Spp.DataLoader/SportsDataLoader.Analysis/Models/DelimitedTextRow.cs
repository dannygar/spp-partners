/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsDataLoader.FileProcessing.Models
{
    public class DelimitedTextRow
    {
        public DelimitedTextRow()
        {
            Cells = new List<string>();
        }

        public DelimitedTextRow(IEnumerable<string> cells)
        {
            if (cells == null)
                throw new ArgumentNullException(nameof(cells));

            Cells = cells.ToList();
        }

        public List<string> Cells { get; set; }
    }
}