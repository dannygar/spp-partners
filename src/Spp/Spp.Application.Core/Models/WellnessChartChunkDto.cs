/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Spp.Application.Core.Models
{
    public class WellnessChartChunkDto
    {
        public string Label { get; set; }
        public DateTime Date { get; set; }
        public double? WellnessValue { get; set; }
        public double? TotalLoadValue { get; set; }
    }
}
