/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Threading.Tasks;
using Spp.Presentation.User.Client.Data;

namespace Spp.Presentation.User.Client.ViewModels
{
    public class AthleteExerciseSetViewModel : NotificationBase<AthleteExerciseSet>
    {
        public AthleteExerciseSetViewModel(AthleteExerciseSet set, int order) : base(set)
        {
            Order = order;
        }

        public int Sets => This.Sets;

        public int Reps
        {
            get { return This.Reps; }
            set
            {
                SetProperty(This.Reps, value, () => This.Reps = value);
                This.TrainingLoad = CalculateTrainingLoad(Reps, Weight);
            }
        }
        public float Weight
        {
            get { return This.Weight; }
            set
            {
                SetProperty(This.Weight, value, () => This.Weight = value);
                This.TrainingLoad = CalculateTrainingLoad(Reps, Weight);
            }
        }


        public int Order { get; set; }

        public override Task Load() => null;


        public static int CalculateTrainingLoad(int reps, float weight)
        {
            return (int)(weight * (float)(1 + (0.033 * reps)));
        }
    }
}
