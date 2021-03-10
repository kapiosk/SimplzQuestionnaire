using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplzQuestionnaire.Model
{
    // Add-Migration M2 -v
    public class SQContext : DbContext
    {
        public SQContext(DbContextOptions<SQContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserAnswer>()
                .HasKey(c => new { c.QuestionnaireUserId, c.AnswerId });
            modelBuilder.Entity<UserAnswer>()
                .HasIndex(c => new { c.QuestionnaireUserId, c.AnswerId });
        }

        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuestionnaireUser> QuestionnaireUsers { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

    }

    public enum Progression
    {
        Admin = 0,
        Auto = 1,
        OnSubmit = 2
    }

    public class Question
    {
        public int QuestionId { get; set; }
        public string Description { get; set; }
        public int Timeout { get; set; }
        public int QuestionnaireId { get; set; }
        public int MaxAnswers { get; set; }
        public int MaxPoints { get; set; }
        public Questionnaire Questionnaire { get; set; }
        public ICollection<Answer> Answer { get; set; }
    }

    public class Answer
    {
        public int AnswerId { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }

    public class Questionnaire
    {
        public int QuestionnaireId { get; set; }
        public string Name { get; set; }
        public Guid Code { get; set; }
        public Progression Progression { get; set; }

        [ForeignKey("QuestionnaireUser")]
        public string QuestionnaireUserId { get; set; }
        public QuestionnaireUser QuestionnaireUser { get; set; }
        public ICollection<Question> Question { get; set; }
    }

    public class QuestionnaireUser : IdentityUser
    {
        [ForeignKey("QuestionnaireUserId")]
        public ICollection<Questionnaire> Questionnaire { get; set; }
        public ICollection<IdentityUserClaim<string>> Claims { get; set; }
    }

    public class QuestionnaireUserClaim : IdentityUserClaim<string>
    {
        public QuestionnaireUser QuestionnaireUser { get; set; }
    }

    public class UserAnswer
    {
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }

        [ForeignKey("QuestionnaireUser")]
        public string QuestionnaireUserId { get; set; }
        public QuestionnaireUser QuestionnaireUser { get; set; }
        public int TimeTaken { get; set; }
    }
}