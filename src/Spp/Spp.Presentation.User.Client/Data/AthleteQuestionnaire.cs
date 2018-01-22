/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace Spp.Presentation.User.Client.Data
{
    public class AthleteQuestionnaire
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SessionId { get; set; }
        public int SequenceOrder { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public List<AthleteQuestion> Questions { get; set; }
    }
}
