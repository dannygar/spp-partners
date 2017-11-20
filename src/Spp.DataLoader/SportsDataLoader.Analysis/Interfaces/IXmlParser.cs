/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;
using System.Xml.Linq;
using SportsDataLoader.FileProcessing.Models;

namespace SportsDataLoader.FileProcessing.Interfaces
{
    public interface IXmlParser
    {
        IEnumerable<DataTable> ParseDocument(XDocument document);
        IEnumerable<DataTable> ParseElement(XElement element);
    }
}