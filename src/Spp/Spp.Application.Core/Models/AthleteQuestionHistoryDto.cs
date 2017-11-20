/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Spp.Application.Core.Models
{
    public class AthleteQuestionHistoryDto
    {
        public AthleteQuestionDto Question { get; set; }
        public IList<QuestionResponseDto> Responses { get; set; }
    }
}
