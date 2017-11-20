/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Spp.Application.Core.Models
{
    public class PlayerResponseDto
    {
        public int SessionId { get; set; }
        public int PlayerId { get; set; }
        public int QuestionnaireId { get; set; }
        public List<QuestionResponseDto> Answers { get; set; }
    }
}
