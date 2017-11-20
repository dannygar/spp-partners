/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using Spp.Application.Core.Models;

namespace Spp.Application.Core.Contracts
{
    public interface IWorkoutService : IEntityConfiguration
    {
        Task<IList<AthleteWorkoutDto>> GetWorkouts();

        Task<IList<AthleteWorkoutDto>> GetSessionWorkouts(int sessionId);

        Task<IList<AthleteExerciseDto>> GetAllExercises();

        Task<bool> CreateWorkout(AthleteWorkoutDto workoutDto);

        Task<bool> UpdateWorkout(AthleteWorkoutDto workoutDto);

        Task<bool> UpdateWorkoutExercises(AthleteWorkoutDto workoutDto);

        Task<bool> DeleteWorkout(int workoutId);

        Task<bool> DeleteAllSessionWorkouts(int sessionId);
    }
}
