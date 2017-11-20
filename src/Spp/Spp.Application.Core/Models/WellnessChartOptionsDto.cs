/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Spp.Application.Core.Models
{
    public class WellnessChartOptionsDto
    {
        public DateTime ChartStartDate { get; set; }
        public string LabelFormat { get; set; }
    }
}
