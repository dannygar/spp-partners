/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsDataLoader.FileProcessing.Models
{
    public class DelimitedTextFile
    {
        public DelimitedTextFile()
        {
            Rows = new List<DelimitedTextRow>();
        }

        public DelimitedTextFile(IEnumerable<DelimitedTextRow> rows)
        {
            if (rows == null)
                throw new ArgumentNullException(nameof(rows));

            Rows = rows.ToList();
        }

        public List<DelimitedTextRow> Rows { get; set; }
    }
}