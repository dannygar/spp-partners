/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Spp.Application.Core.Models
{
    public class BenchmarkDto : ModelBase
    {
        public string Controller { get; set; }
        public string Operation { get; set; }
        public string Method { get; set; }
        public int Time { get; set; }
        public DateTime LastRun { get; set; }
    }
}
