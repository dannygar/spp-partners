/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Spp.Presentation.User.Client.Data
{
    public class AthleteQuestionHistory
    {
        public AthleteQuestion Question { get; set; }
        public IList<AthleteQuestionHistoryEntry> Responses { get; set; }

        public AthleteQuestionHistory()
        {
            Responses = new List<AthleteQuestionHistoryEntry>();
        }
    }
}
