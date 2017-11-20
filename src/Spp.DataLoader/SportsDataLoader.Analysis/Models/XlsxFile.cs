/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;

namespace SportsDataLoader.FileProcessing.Models
{
    public class XlsxFile
    {
        public XlsxFile()
        {
            Sheets = new Dictionary<string, XlsxSheet>();
        }

        public Dictionary<string, XlsxSheet> Sheets { get; set; }
    }
}