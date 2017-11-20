/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Spp.Application.Core.Models
{
    public class MetricSubjectDataDto
    {
        public string SubjectName { get; set; }
        public List<MetricSetDataDto> Sets { get; set; }
    }

    public class MetricSetDataDto
    {
        public int Id { get; set; }
        public List<MetricDataDto> Metrics { get; set; }
    }

    public class MetricDataDto
    {
        public int Id { get; set; }
        public double? Value { get; set; }
    }
}
