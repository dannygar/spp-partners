/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace Spp.Application.Core.Models
{
    public class AthleteQuestionnaireDto : ModelBase
    {
        public string Name { get; set; }
        public int SessionId { get; set; }
        public int SequenceOrder { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public IList<AthleteQuestionDto> Questions { get; set; }
    }
}
