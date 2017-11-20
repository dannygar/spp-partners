/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Xml.Linq;
using SportsDataLoader.FileProcessing.Xml;

namespace SportsDataLoader.FileProcessing.Interfaces
{
    public interface IXmlContextParser
    {
        XmlContext ParseContext(XDocument document, XmlContext parentContext = null);
        XmlContext ParseContext(XElement element, XmlContext parentContext = null);
    }
}