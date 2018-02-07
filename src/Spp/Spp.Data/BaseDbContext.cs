/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Data;
using Microsoft.EntityFrameworkCore;
using Spp.Data.Entities;

namespace Spp.Data
{
    public class BaseDbContext : DbContext
    {
        public IDbConnection DbConnection { get; }

        public BaseDbContext(DbContextOptions options) : base(options) { }

        public BaseDbContext(DbContextOptions options, IDbConnection dbConnection)
            : base(options)
        {
            DbConnection = dbConnection;
        }

        public virtual DbSet<Coach> Coaches { get; set; }
        public virtual DbSet<CognitiveService> CognitiveServices { get; set; }
        public virtual DbSet<DayType> DayTypes { get; set; }
        public virtual DbSet<Drill> Drills { get; set; }
        public virtual DbSet<Education> Educations { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Locale> Locales { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Mesocycle> Mesocycles { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Microcycle> Microcycles { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<PlayerSession> PlayerSessions { get; set; }
        public virtual DbSet<PlayerSquad> PlayerSquads { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Practice> Practices { get; set; }
        public virtual DbSet<PracticeData> PracticeDatas { get; set; }
        public virtual DbSet<PracticeDrill> PracticeDrills { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
        public virtual DbSet<QuestionnaireResponse> QuestionnaireResponses { get; set; }
        public virtual DbSet<QuestionnaireStatus> QuestionnaireStatus { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<SeasonSubTeam> SeasonSubTeams { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<SessionUser> SessionUsers { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<Squad> Squads { get; set; }
        public virtual DbSet<SubPosition> SubPositions { get; set; }
        public virtual DbSet<SubTeam> SubTeams { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamMatch> TeamMatches { get; set; }
        public virtual DbSet<TeamReadiness> TeamReadinesses { get; set; }
        public virtual DbSet<SessionType> Types { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserEmotion> UserEmotions { get; set; }
        public virtual DbSet<UserQuestionnaire> UserQuestionnaires { get; set; }
        public virtual DbSet<UserTeam> UserTeams { get; set; }
        public virtual DbSet<Workout> Workouts { get; set; }
        public virtual DbSet<WorkoutData> WorkoutDatas { get; set; }
        public virtual DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public virtual DbSet<WorkoutExerciseData> WorkoutExerciseDatas { get; set; }
        public virtual DbSet<TeamImages> TeamImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubTeam>()
                .Property(e => e.Gender)
                .HasMaxLength(10);

            modelBuilder.Entity<User>()
                .Property(e => e.Gender)
                .HasMaxLength(10);
        }
    }
}
