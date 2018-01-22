/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Dynamic;

namespace Spp.Presentation.User.Client.Models.PropertyDescriptor
{
    public class PropertyDescriptor
    {
        public ExpandoObject Expando { get; set; }

        public List<PropertyOptions> Options { get; private set; }

        public PropertyDescriptor()
        {
            Options = new List<PropertyOptions>();
        }
    }
}
