using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplzQuestionnaire.Model
{
    // Add-Migration M4 -v
    //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-5.0

    public class SQContext : DbContext
    {
        public SQContext(DbContextOptions<SQContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserAnswer>()
                .HasKey(c => new { c.UserId, c.AnswerId });
            modelBuilder.Entity<UserAnswer>()
                .HasIndex(c => new { c.UserId, c.AnswerId });
        }

        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<QuestionnaireUser> QuestionnaireUsers { get; set; }
        public virtual DbSet<UserAnswer> UserAnswers { get; set; }
        public virtual DbSet<QuestionnaireUserClaim> QuestionnaireUserClaims { get; set; }

    }

    public enum Progression
    {
        Admin = 0,
        Auto = 1,
        OnSubmit = 2
    }

    public class Question
    {
        public virtual int QuestionId { get; set; }
        public virtual int Rank { get; set; } = 1;
        public virtual string Description { get; set; }
        public virtual int Timeout { get; set; } = 0;
        public virtual int MaxAnswers { get; set; } = 0;
        public virtual int MaxPoints { get; set; } = 0;
        public virtual bool AcceptsCustomAnswer { get; set; }
        public virtual int QuestionnaireId { get; set; }
        public virtual Questionnaire Questionnaire { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }

    public class Answer
    {
        public virtual int AnswerId { get; set; }
        public virtual string Description { get; set; }
        public virtual int Points { get; set; }
        public virtual bool IsCustomAnswer { get; set; }
        public virtual int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }

        [NotMapped]
        public virtual bool IsSelected { get; set; }
    }

    public class Questionnaire
    {
        public virtual int QuestionnaireId { get; set; }
        public virtual string Name { get; set; }
        public virtual Guid Code { get; set; }
        public virtual Progression Progression { get; set; }
        public virtual int ActiveQuestionId { get; set; }

        [ForeignKey("QuestionnaireUser")]
        public virtual string UserId { get; set; }
        public virtual QuestionnaireUser QuestionnaireUser { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }

    public class QuestionnaireUser : IdentityUser
    {
        [ForeignKey("UserId")]
        public virtual ICollection<Questionnaire> Questionnaires { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
    }

    public class QuestionnaireUserClaim : IdentityUserClaim<string>
    {
        public virtual QuestionnaireUser User { get; set; }
    }

    public class UserAnswer
    {
        public virtual int AnswerId { get; set; }
        public virtual Answer Answer { get; set; }
        public virtual string TextAnswer { get; set; }

        [ForeignKey("QuestionnaireUser")]
        public virtual string UserId { get; set; }
        public virtual QuestionnaireUser QuestionnaireUser { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }
    }
}