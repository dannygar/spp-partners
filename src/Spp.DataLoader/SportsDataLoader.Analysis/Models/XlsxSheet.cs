/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsDataLoader.FileProcessing.Models
{
    public class XlsxSheet
    {
        public XlsxSheet()
        {
            Rows = new List<XlsxRow>();
        }

        public XlsxSheet(IEnumerable<XlsxRow> rows)
        {
            if (rows == null)
                throw new ArgumentNullException(nameof(rows));

            Rows = rows.ToList();
        }

        public List<XlsxRow> Rows { get; set; } 
    }
}