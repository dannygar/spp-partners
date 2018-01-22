/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Presentation.User.Client.Models.PropertyDescriptor
{
    public class SliderPropertyOptions : PropertyOptions
    {
        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public double Step { get; set; } = 1;
    }
}
