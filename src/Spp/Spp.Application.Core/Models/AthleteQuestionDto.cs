/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Spp.Application.Core.Models
{
    public class AthleteQuestionDto : ModelBase
    {
        public string Text { get; set; }
        public int SequenceOrder { get; set; }
        public KeyValuePair<string, int> MinCaptionValue { get; set; }
        public KeyValuePair<string, int> MidCaptionValue { get; set; }
        public KeyValuePair<string, int> MaxCaptionValue { get; set; }
    }
}
