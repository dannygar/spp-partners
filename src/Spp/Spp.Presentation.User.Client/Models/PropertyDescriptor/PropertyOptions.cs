/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Presentation.User.Client.Models.PropertyDescriptor
{
    public class PropertyOptions
    {
        public string Name { get; set; }

        public string OriginalString { get; set; }

        public PropertyKind Kind { get; set; }

        public object DefaultValue { get; set; }
    }
}
