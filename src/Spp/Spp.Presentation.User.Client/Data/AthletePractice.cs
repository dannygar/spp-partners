/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Spp.Presentation.User.Client.Data
{
    public class AthletePractice
    {
        public string Name { get; set; }
        public string Topic { get; set; }
        public string SubTopic { get; set; }
        public int SessionId { get; set; }
        public int? EstimatedTrainingLoad { get; set; }
        public int? RecommendedTrainingLoad { get; set; }
        public string Side { get; set; }
        public bool IsModified { get; set; }
        public int TeamId { get; set; }
        public int Id { get; set; }

        public IList<PracticeDrill> PracticeDrills { get; set; }
        public Note Note { get; set; }
        public User Coach { get; set; }
        public Session Session { get; set; }

    }
}
