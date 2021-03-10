﻿using Microsoft.AspNetCore.Identity;
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
        public virtual string Description { get; set; }
        public virtual int Timeout { get; set; }
        public virtual int QuestionnaireId { get; set; }
        public virtual int MaxAnswers { get; set; }
        public virtual int MaxPoints { get; set; }
        public virtual Questionnaire Questionnaire { get; set; }
        public virtual ICollection<Answer> Answer { get; set; }
    }

    public class Answer
    {
        public virtual int AnswerId { get; set; }
        public virtual string Description { get; set; }
        public virtual int Points { get; set; }
        public virtual int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }

    public class Questionnaire
    {
        public virtual int QuestionnaireId { get; set; }
        public virtual string Name { get; set; }
        public virtual Guid Code { get; set; }
        public virtual Progression Progression { get; set; }

        [ForeignKey("QuestionnaireUser")]
        public virtual string UserId { get; set; }
        public virtual QuestionnaireUser QuestionnaireUser { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }

    public class QuestionnaireUser : IdentityUser
    {
        [ForeignKey("UserId")]
        public virtual ICollection<Questionnaire> Questionnaire { get; set; }
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

        [ForeignKey("QuestionnaireUser")]
        public virtual string UserId { get; set; }
        public virtual QuestionnaireUser QuestionnaireUser { get; set; }
        public virtual int TimeTaken { get; set; }
    }
}