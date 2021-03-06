using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplzQuestionnaire.Model
{
    //Add-Migration M2 -v
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

            modelBuilder.Entity<SessionGroupUser>()
                .HasKey(c => new { c.UserId, c.SessionGroupId });
            modelBuilder.Entity<SessionGroupUser>()
                .HasIndex(c => new { c.UserId, c.SessionGroupId });
        }

        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<UserAnswer> UserAnswers { get; set; }
        public virtual DbSet<QuestionnaireUser> QuestionnaireUsers { get; set; }
        public virtual DbSet<QuestionnaireUserClaim> QuestionnaireUserClaims { get; set; }
        public virtual DbSet<SessionGroup> SessionGroups { get; set; }
        public virtual DbSet<SessionGroupUser> SessionGroupUsers { get; set; }
    }

    public enum Progression
    {
        Admin = 0,
        Auto = 1,
        OnSubmit = 2
    }

    public enum CustomAnswer
    {
        NO = 0,
        TEXT = 1,
        NUMBER = 2
    }

    public class SessionGroup
    {
        public virtual int SessionGroupId { get; set; }
        public virtual string Name { get; set; } = "";
        public virtual DateTime Date { get; set; } = DateTime.UtcNow.Date;
        [ForeignKey("Questionnaire")]
        public virtual int QuestionnaireId { get; set; }
        public virtual Questionnaire Questionnaire { get; set; }
        public virtual ICollection<SessionGroupUser> SessionGroupUsers { get; set; }
    }

    public class SessionGroupUser
    {
        [ForeignKey("SessionGroup")]
        public virtual int SessionGroupId { get; set; }
        [Column(TypeName = "nvarchar")]
        [System.ComponentModel.DataAnnotations.StringLength(36)]
        [ForeignKey("QuestionnaireUser")]
        public virtual string UserId { get; set; }
    }

    public class Question
    {
        public virtual int QuestionId { get; set; }
        public virtual int Rank { get; set; } = 1;
        public virtual string Description { get; set; }
        public virtual int Timeout { get; set; } = 0;
        public virtual int MaxAnswers { get; set; } = 0;
        public virtual int MaxPoints { get; set; } = 0;
        public virtual CustomAnswer CustomAnswer { get; set; }
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

        [Column(TypeName = "nvarchar")]
        [System.ComponentModel.DataAnnotations.StringLength(36)]
        [ForeignKey("QuestionnaireUser")]
        public virtual string UserId { get; set; }
        public virtual QuestionnaireUser QuestionnaireUser { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<SessionGroup> SessionGroups { get; set; }
    }

    public class QuestionnaireUser : IdentityUser<string>
    {

        [Column(TypeName = "nvarchar")]
        [System.ComponentModel.DataAnnotations.StringLength(36)]
        public override string Id { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey("UserId")]
        public virtual ICollection<Questionnaire> Questionnaires { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
    }

    public class QuestionnaireUserClaim : IdentityUserClaim<string>
    {    }

    public class UserAnswer
    {
        public virtual int AnswerId { get; set; }
        public virtual Answer Answer { get; set; }
        public virtual string TextAnswer { get; set; }

        [Column(TypeName = "nvarchar")]
        [System.ComponentModel.DataAnnotations.StringLength(36)]
        [ForeignKey("QuestionnaireUser")]
        public virtual string UserId { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }
    }
}