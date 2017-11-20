/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// ------------------------------------------------------
// <copyright file="IMetricsSettingsProvider.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>
// ------------------------------------------------------

using System;
using System.Collections.Generic;
using MicrosoftSportsScience.Constants;
using MicrosoftSportsScience.Data;

namespace MicrosoftSportsScience.Services
{
    public interface IMetricsSettingsProvider
    {
        MetricSettings Get();

        void Save(MetricSettings settings);
    }

    public class CachedMetricsSettingsProvider : IMetricsSettingsProvider
    {
        private readonly ICacheService cache;

        public CachedMetricsSettingsProvider(ICacheService cache)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            this.cache = cache;
        }

        public MetricSettings Get()
        {
            return
                this.cache.GetItem<MetricSettings>(Defines.CACHE_KEY_METRICSETTINGS) ??
                    new MetricSettings
                    {
                        MetricSubjectsRequest = new MetricSubjectsRequest
                        {
                                        new MetricSubjectRequest { SubjectType = PerformanceSubjectType.Player, SubjectId = 1 },
                                        new MetricSubjectRequest { SubjectType = PerformanceSubjectType.Team, SubjectId = 1 },
                        },
                        VisibleMetrics = new Dictionary<int, int[]>
                        {
                                        { 1, new[] { 1, 2, 3, 4, 5, 6 } },
                                        { 2, new[] { 1, 2, 3, 4, 5, 6 } },
                        },
                    };
        }

        public void Save(MetricSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            this.cache.SetItem(Defines.CACHE_KEY_METRICSETTINGS, settings);
        }
    }
}