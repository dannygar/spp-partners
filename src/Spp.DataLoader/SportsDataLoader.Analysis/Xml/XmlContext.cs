/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SportsDataLoader.FileProcessing.Xml
{
    public class XmlContext
    {
        public XmlContext()
        {
            Children = new List<XmlContext>();
            Properties = new Dictionary<string, string>();
        }

        public XmlContext(XElement element, XmlContext parentContext = null)
            : this()
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            ParseElement(element);
        }

        public string ContextName { get; set; }
        public XName ElementName { get; set; }
        public Guid ContextId { get; set; }
        public XmlContext ParentContext { get; set; }
        public List<XmlContext> Children { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        private void ParseElement(XElement element, XmlContext parentContext = null)
        {
            ContextId = Guid.NewGuid();

            ContextName = element.Name.LocalName;
            ElementName = element.Name;

            if (parentContext == null)
            {
                ContextName = element.Name.LocalName;
            }
            else
            {
                ContextName = $"{parentContext.ContextName}_{element.Name.LocalName}";
                ParentContext = parentContext;
            }

            ParseContextProperties(element);
            ParseChildren(element);
        }

        private void ParseContextProperties(XElement element)
        {
            foreach (var attribute in element.Attributes())
            {
                Properties[attribute.Name.LocalName] = attribute.Value;
            }

            foreach (var textElement in element.Elements().Where(e => (e.NodeType == XmlNodeType.Text)))
            {
                Properties[element.Name.LocalName] = element.Value;
            }
        }

        private void ParseChildren(XElement element)
        {
            foreach (var childElement in element.Elements().Where(e => (e.Attributes().Any()) ||
                                                                       (e.Elements().Any())))
            {
                Children.Add(new XmlContext(element, this));
            }
        }
    }
}