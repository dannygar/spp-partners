/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spp.Presentation.User.Client.Data;

namespace Spp.Presentation.User.Client.ViewModels
{
    public class CalendarDayViewModel : NotificationBase
    {
        Session _session;
        List<AthletePractice> _practices;
        List<AthletePracticeViewModel> _practiceModels;
        List<AthleteWorkoutViewModel> _workoutModels;
        List<AthleteWorkout> _workouts;
        DateTime _date;

        public CalendarDayViewModel(Session session, List<AthletePractice> practices, List<AthleteWorkout> workouts, DateTime date)
        {
            _session = session;
            _practices = practices;
            _workouts = workouts;
            _date = date;

            _practiceModels = _practices.Select(x => new AthletePracticeViewModel(x)).ToList();
            _workoutModels = workouts.Select(x => new AthleteWorkoutViewModel(x)).ToList();
        }

        public int SessionId => _session != null ? _session.Id : -1;
        public DateTime Date => _date;
        public bool HasSessions => _session != null;
        public bool HasPractice => _practices.Count > 0;
        public bool HasWorkout => _workouts.Count > 0;
        public List<AthleteWorkoutViewModel> Workouts => _workoutModels;
        public List<AthletePracticeViewModel> Practices => _practiceModels;
        public override Task Load() => null;
    }
}
