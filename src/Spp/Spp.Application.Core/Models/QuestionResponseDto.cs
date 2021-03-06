/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace Spp.Application.Core.Models
{
    public sealed class QuestionResponseDto
    {
        public int QuestionId { get; set; }
        public DateTime AnswerDateTime { get; set; }
        public KeyValuePair<string, int> Answer { get; set; }
    }
}
