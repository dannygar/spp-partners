/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
namespace SportsDataLoader.Model.Sql
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SportsDataModel : DbContext
    {
        public SportsDataModel()
            : base("name=SportsDataModel")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<DayType> DayTypes { get; set; }
        public virtual DbSet<Drill> Drills { get; set; }
        public virtual DbSet<DrillData> DrillDatas { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<ExerciseData> ExerciseDatas { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Mesocycle> Mesocycles { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Microcycle> Microcycles { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<PlayerPosition> PlayerPositions { get; set; }
        public virtual DbSet<PlayerSquad> PlayerSquads { get; set; }
        public virtual DbSet<Practice> Practices { get; set; }
        public virtual DbSet<PracticeData> PracticeDatas { get; set; }
        public virtual DbSet<PracticeDrill> PracticeDrills { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<QuestionResponse> QuestionResponses { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Workout> Workouts { get; set; }
        public virtual DbSet<WorkoutData> WorkoutDatas { get; set; }
        public virtual DbSet<WorkoutExercise> WorkoutExercises { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Image)
                .HasForeignKey(e => e.PhotoId);

            modelBuilder.Entity<PracticeData>()
                .HasOptional(e => e.Session)
                .WithRequired(e => e.PracticeData);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.Questionnaires)
                .WithMany(e => e.Questions)
                .Map(m => m.ToTable("QuestionnaireQuestion").MapLeftKey("QuestionId").MapRightKey("QuestionnaireId"));

            modelBuilder.Entity<Questionnaire>()
                .HasMany(e => e.Sessions)
                .WithMany(e => e.Questionnaires)
                .Map(m => m.ToTable("SessionQuestionnaire").MapLeftKey("QuestionnaireId").MapRightKey("SessionId"));

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.FromId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.ToId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WorkoutData>()
                .HasOptional(e => e.Session)
                .WithRequired(e => e.WorkoutData);
        }
    }
}
