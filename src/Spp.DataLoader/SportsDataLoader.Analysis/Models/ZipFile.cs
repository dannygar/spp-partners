/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;
using System.IO;

namespace SportsDataLoader.FileProcessing.Models
{
    public class ZipFile
    {
        public ZipFile()
        {
            Entries = new Dictionary<string, Stream>();
        }

        public Dictionary<string, Stream> Entries { get; set; }
    }
}