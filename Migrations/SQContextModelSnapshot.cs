﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Migrations
{
    [DbContext(typeof(SQContext))]
    partial class SQContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("QuestionnaireUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("QuestionnaireUserId");

                    b.ToTable("IdentityUserClaim<string>");
                });

            modelBuilder.Entity("SimplzQuestionnaire.Model.Answer", b =>
                {
                    b.Property<int>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("Points")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuestionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AnswerId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("SimplzQuestionnaire.Model.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxAnswers")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxPoints")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuestionnaireId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Timeout")
                        .HasColumnType("INTEGER");

                    b.HasKey("QuestionId");

                    b.HasIndex("QuestionnaireId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("SimplzQuestionnaire.Model.Questionnaire", b =>
                {
                    b.Property<int>("QuestionnaireId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Progression")
                        .HasColumnType("INTEGER");

                    b.Property<string>("QuestionnaireUserId")
                        .HasColumnType("TEXT");

                    b.HasKey("QuestionnaireId");

                    b.HasIndex("QuestionnaireUserId");

                    b.ToTable("Questionnaires");
                });

            modelBuilder.Entity("SimplzQuestionnaire.Model.QuestionnaireUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("QuestionnaireUsers");
                });

            modelBuilder.Entity("SimplzQuestionnaire.Model.UserAnswer", b =>
                {
                    b.Property<string>("QuestionnaireUserId")
                        .HasColumnType("TEXT");

                    b.Property<int>("AnswerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimeTaken")
                        .HasColumnType("INTEGER");

                    b.HasKey("QuestionnaireUserId", "AnswerId");

                    b.HasIndex("AnswerId");

                    b.HasIndex("QuestionnaireUserId", "AnswerId");

                    b.ToTable("UserAnswers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SimplzQuestionnaire.Model.QuestionnaireUser", null)
                        .WithMany("Claims")
                        .HasForeignKey("QuestionnaireUserId");
                });

            modelBuilder.Entity("SimplzQuestionnaire.Model.Answer", b =>
                {
                    b.HasOne("SimplzQuestionnaire.Model.Question", "Question")
                        .WithMany("Answer")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("SimplzQuestionnaire.Model.Question", b =>
                {
                    b.HasOne("SimplzQuestionnaire.Model.Questionnaire", "Questionnaire")
                        .WithMany("Question")
                        .HasForeignKey("QuestionnaireId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Questionnaire");
                });

            modelBuilder.Entity("SimplzQuestionnaire.Model.Questionnaire", b =>
                {
                    b.HasOne("SimplzQuestionnaire.Model.QuestionnaireUser", "QuestionnaireUser")
                        .WithMany("Questionnaire")
                        .HasForeignKey("QuestionnaireUserId");

                    b.Navigation("QuestionnaireUser");
                });

            modelBuilder.Entity("SimplzQuestionnaire.Model.UserAnswer", b =>
                {
                    b.HasOne("SimplzQuestionnaire.Model.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimplzQuestionnaire.Model.QuestionnaireUser", "QuestionnaireUser")
                        .WithMany()
                        .HasForeignKey("QuestionnaireUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("QuestionnaireUser");
                });

            modelBuilder.Entity("SimplzQuestionnaire.Model.Question", b =>
                {
                    b.Navigation("Answer");
                });

            modelBuilder.Entity("SimplzQuestionnaire.Model.Questionnaire", b =>
                {
                    b.Navigation("Question");
                });

            modelBuilder.Entity("SimplzQuestionnaire.Model.QuestionnaireUser", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("Questionnaire");
                });
#pragma warning restore 612, 618
        }
    }
}
