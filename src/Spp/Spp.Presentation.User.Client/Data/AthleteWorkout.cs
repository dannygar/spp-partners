/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Spp.Presentation.User.Client.Data
{
    public class AthleteWorkout
    {
        public int Id { get; set; }
        public int SessionId { get; set; }

        public string Name { get; set; }

        public string Topic { get; set; }

        public string SubTopic { get; set; }

        public string Category { get; set; }

        public List<AthleteExercise> Exercises { get; set; }

        public Session Session { get; set; }
    }
}
