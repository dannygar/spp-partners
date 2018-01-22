/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Presentation.User.Client.Data
{
    public class AthleteExercise
    {
        public int Id { get; set; }
        public int ExerciseDataId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        //public List<AthleteExerciseSet> Sets { get; set; }
        public AthleteExerciseSet Sets { get; set; }
        public bool IsDone { get; set; }
        public bool IsModified { get; set; }
        public int Duration { get; set; }
        public Note Note { get; set; }
        public string TrainingLoad { get; set; }
    }
}
