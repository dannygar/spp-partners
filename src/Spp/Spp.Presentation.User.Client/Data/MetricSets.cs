/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// ------------------------------------------------------
// <copyright file="MetricSets.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>
// ------------------------------------------------------
#pragma warning disable SA1649 // File name must match first type name
#pragma warning disable SA1402 // File may only contain a single class : For some cases it is handy to keep simple models in one file.
namespace MicrosoftSportsScience.Data
{
    using System.Collections.Generic;
    using MicrosoftSportsScience.Constants;

    public class MetricSettings
    {
        public MetricSubjectsRequest MetricSubjectsRequest { get; set; }

        public Dictionary<int, int[]> VisibleMetrics { get; set; }
    }

    public class MetricSubjectsRequest : List<MetricSubjectRequest>
    {
        public MetricSubjectsRequest()
        {
        }

        public MetricSubjectsRequest(IEnumerable<MetricSubjectRequest> collection)
            : base(collection)
        {
        }
    }

    public class MetricSubjectRequest
    {
        public PerformanceSubjectType SubjectType { get; set; }

        public int SubjectId { get; set; }
    }

    public class MetricSubjectsData : List<MetricSubjectData>
    {
    }

    public class MetricSubjectData
    {
        public string SubjectName { get; set; }

        public MetricSetData[] Sets { get; set; }
    }

    public class MetricSetData
    {
        public int Id { get; set; }

        public MetricData[] Metrics { get; set; }
    }

    public class MetricData
    {
        public int Id { get; set; }

        public int Value { get; set; }
    }

    public class MetricSets : List<MetricSet>
    {
    }

    public class MetricSet
    {
        public int Id { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public string Tooltip { get; set; }

        public Metric[] Metrics { get; set; }
    }

    public class Metric
    {
        public int Id { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public string Tooltip { get; set; }
    }

    public class MetricDataModel
    {
        public MetricSets Metrics { get; set; } = new MetricSets();

        public MetricSubjectsData Data { get; set; } = new MetricSubjectsData();

        public Dictionary<int, int[]> VisibleMetrics { get; set; }
    }
}