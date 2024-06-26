﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestingApi.Data;

#nullable disable

namespace TestingApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TestingApi.Models.Test.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("TemplateId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("TestingApi.Models.Test.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxScore")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("QuestionsPoolId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionsPoolId");

                    b.HasIndex("TemplateId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("TestingApi.Models.Test.QuestionsPool", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("GenerationStrategy")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumOfQuestionsToBeGenerated")
                        .HasColumnType("int");

                    b.Property<Guid?>("TemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TestId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.HasIndex("TestId");

                    b.ToTable("QuestionsPools");
                });

            modelBuilder.Entity("TestingApi.Models.Test.Test", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedTimestamp");

                    b.HasIndex("Duration");

                    b.HasIndex("ModifiedTimestamp");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("TemplateId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("TestingApi.Models.TestTemplate.AnswerTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("DefaultText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCorrectRestriction")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("QuestionTemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuestionTemplateId");

                    b.ToTable("AnswerTemplates");
                });

            modelBuilder.Entity("TestingApi.Models.TestTemplate.QuestionTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("DefaultText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MaxScoreRestriction")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("QuestionsPoolTemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuestionsPoolTemplateId");

                    b.ToTable("QuestionTemplates");
                });

            modelBuilder.Entity("TestingApi.Models.TestTemplate.QuestionsPoolTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("DefaultName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GenerationStrategyRestriction")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int?>("NumOfQuestionsToBeGeneratedRestriction")
                        .HasColumnType("int");

                    b.Property<Guid>("TestTemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TestTemplateId");

                    b.ToTable("QuestionsPoolTemplates");
                });

            modelBuilder.Entity("TestingApi.Models.TestTemplate.TestTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DefaultDuration")
                        .HasColumnType("int");

                    b.Property<string>("DefaultSubject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DefaultTestDifficulty")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("TemplateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedTimestamp");

                    b.HasIndex("DefaultDuration");

                    b.HasIndex("ModifiedTimestamp");

                    b.HasIndex("TemplateName")
                        .IsUnique();

                    b.ToTable("TestTemplates");
                });

            modelBuilder.Entity("TestingApi.Models.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProfilePictureFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedTimestamp");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("FirstName");

                    b.HasIndex("LastName");

                    b.HasIndex("ModifiedTimestamp");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0a5bfb58-d88a-4c47-9253-3e65a6a96fa6"),
                            CreatedBy = new Guid("00000000-0000-0000-0000-000000000000"),
                            CreatedTimestamp = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "nikitinroma2605@gmail.com",
                            EmailConfirmed = true,
                            FirstName = "Roman",
                            LastName = "Nikitin",
                            UserRole = 2
                        });
                });

            modelBuilder.Entity("TestingApi.Models.UserAnswer", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnswerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AnsweringTime")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "QuestionId", "AnswerId");

                    b.HasIndex("AnswerId");

                    b.HasIndex("QuestionId");

                    b.ToTable("UserAnswers");
                });

            modelBuilder.Entity("TestingApi.Models.UserQuestion", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "QuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("UserQuestions");
                });

            modelBuilder.Entity("TestingApi.Models.UserTest", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TestId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndingTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartingTime")
                        .HasColumnType("datetime2");

                    b.Property<float>("TotalScore")
                        .HasColumnType("real");

                    b.Property<float>("UserScore")
                        .HasColumnType("real");

                    b.Property<int>("UserTestStatus")
                        .HasColumnType("int");

                    b.HasKey("UserId", "TestId");

                    b.HasIndex("StartingTime");

                    b.HasIndex("TestId");

                    b.HasIndex("UserScore");

                    b.ToTable("UserTests");
                });

            modelBuilder.Entity("TestingApi.Models.Test.Answer", b =>
                {
                    b.HasOne("TestingApi.Models.Test.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestingApi.Models.TestTemplate.AnswerTemplate", "AnswerTemplate")
                        .WithMany("AnswersFromTemplate")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("AnswerTemplate");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("TestingApi.Models.Test.Question", b =>
                {
                    b.HasOne("TestingApi.Models.Test.QuestionsPool", "QuestionsPool")
                        .WithMany("Questions")
                        .HasForeignKey("QuestionsPoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestingApi.Models.TestTemplate.QuestionTemplate", "QuestionTemplate")
                        .WithMany("QuestionsFromTemplate")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("QuestionTemplate");

                    b.Navigation("QuestionsPool");
                });

            modelBuilder.Entity("TestingApi.Models.Test.QuestionsPool", b =>
                {
                    b.HasOne("TestingApi.Models.TestTemplate.QuestionsPoolTemplate", "QuestionsPoolTemplate")
                        .WithMany("QuestionsPoolsFromTmpl")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("TestingApi.Models.Test.Test", "Test")
                        .WithMany("QuestionsPools")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuestionsPoolTemplate");

                    b.Navigation("Test");
                });

            modelBuilder.Entity("TestingApi.Models.Test.Test", b =>
                {
                    b.HasOne("TestingApi.Models.TestTemplate.TestTemplate", "TestTemplate")
                        .WithMany("TestsFromTemplate")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("TestTemplate");
                });

            modelBuilder.Entity("TestingApi.Models.TestTemplate.AnswerTemplate", b =>
                {
                    b.HasOne("TestingApi.Models.TestTemplate.QuestionTemplate", "QuestionTemplate")
                        .WithMany("AnswerTemplates")
                        .HasForeignKey("QuestionTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuestionTemplate");
                });

            modelBuilder.Entity("TestingApi.Models.TestTemplate.QuestionTemplate", b =>
                {
                    b.HasOne("TestingApi.Models.TestTemplate.QuestionsPoolTemplate", "QuestionsPoolTemplate")
                        .WithMany("QuestionsTemplates")
                        .HasForeignKey("QuestionsPoolTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuestionsPoolTemplate");
                });

            modelBuilder.Entity("TestingApi.Models.TestTemplate.QuestionsPoolTemplate", b =>
                {
                    b.HasOne("TestingApi.Models.TestTemplate.TestTemplate", "TestTemplate")
                        .WithMany("QuestionsPoolTemplates")
                        .HasForeignKey("TestTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TestTemplate");
                });

            modelBuilder.Entity("TestingApi.Models.UserAnswer", b =>
                {
                    b.HasOne("TestingApi.Models.Test.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestingApi.Models.Test.Question", "Question")
                        .WithMany("UserAnswers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TestingApi.Models.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("Question");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TestingApi.Models.UserQuestion", b =>
                {
                    b.HasOne("TestingApi.Models.Test.Question", "Question")
                        .WithMany("UserQuestions")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestingApi.Models.User.User", "User")
                        .WithMany("UserQuestions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TestingApi.Models.UserTest", b =>
                {
                    b.HasOne("TestingApi.Models.Test.Test", "Test")
                        .WithMany("UserTests")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestingApi.Models.User.User", "User")
                        .WithMany("UserTests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Test");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TestingApi.Models.Test.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("UserAnswers");

                    b.Navigation("UserQuestions");
                });

            modelBuilder.Entity("TestingApi.Models.Test.QuestionsPool", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("TestingApi.Models.Test.Test", b =>
                {
                    b.Navigation("QuestionsPools");

                    b.Navigation("UserTests");
                });

            modelBuilder.Entity("TestingApi.Models.TestTemplate.AnswerTemplate", b =>
                {
                    b.Navigation("AnswersFromTemplate");
                });

            modelBuilder.Entity("TestingApi.Models.TestTemplate.QuestionTemplate", b =>
                {
                    b.Navigation("AnswerTemplates");

                    b.Navigation("QuestionsFromTemplate");
                });

            modelBuilder.Entity("TestingApi.Models.TestTemplate.QuestionsPoolTemplate", b =>
                {
                    b.Navigation("QuestionsPoolsFromTmpl");

                    b.Navigation("QuestionsTemplates");
                });

            modelBuilder.Entity("TestingApi.Models.TestTemplate.TestTemplate", b =>
                {
                    b.Navigation("QuestionsPoolTemplates");

                    b.Navigation("TestsFromTemplate");
                });

            modelBuilder.Entity("TestingApi.Models.User.User", b =>
                {
                    b.Navigation("UserQuestions");

                    b.Navigation("UserTests");
                });
#pragma warning restore 612, 618
        }
    }
}
