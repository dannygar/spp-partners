/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;

namespace SportsDataLoader.FileProcessing.Models
{
    public class XlsxRow
    {
        public XlsxRow()
        {
            Cells = new Dictionary<string, DataValue>();
        }

        public Dictionary<string, DataValue> Cells { get; set; }
    }
}