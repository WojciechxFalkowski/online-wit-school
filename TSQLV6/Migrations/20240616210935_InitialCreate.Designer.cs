﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TSQLV6.Models;

#nullable disable

namespace TSQLV6.Migrations
{
    [DbContext(typeof(UniversityDbContext))]
    [Migration("20240616210935_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TSQLV6.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CourseID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<string>("AcademicYear")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("LecturerId")
                        .HasColumnType("int")
                        .HasColumnName("LecturerID");

                    b.HasKey("CourseId");

                    b.HasIndex("LecturerId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("TSQLV6.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DepartmentID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentId"));

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("TSQLV6.Models.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("EnrollmentID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnrollmentId"));

                    b.Property<DateOnly?>("CompletionDate")
                        .HasColumnType("date");

                    b.Property<int>("CourseId")
                        .HasColumnType("int")
                        .HasColumnName("CourseID");

                    b.Property<decimal?>("Grade")
                        .HasColumnType("decimal(3, 2)");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("StudentID");

                    b.HasKey("EnrollmentId");

                    b.HasIndex("CourseId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("TSQLV6.Models.Lecturer", b =>
                {
                    b.Property<string>("AcademicDegree")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateOnly>("JoinDate")
                        .HasColumnType("date");

                    b.Property<int>("LecturerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("LecturerID");

                    b.HasIndex("LecturerId");

                    b.ToTable("Lecturers");
                });

            modelBuilder.Entity("TSQLV6.Models.Specialization", b =>
                {
                    b.Property<int>("SpecializationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("SpecializationID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SpecializationId"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int")
                        .HasColumnName("DepartmentID");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SpecializationName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("SpecializationId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Specializations");
                });

            modelBuilder.Entity("TSQLV6.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("StudentID");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int")
                        .HasColumnName("DepartmentID");

                    b.Property<int?>("SpecializationId")
                        .HasColumnType("int")
                        .HasColumnName("SpecializationID");

                    b.Property<string>("StudyMode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("StudentId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("SpecializationId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("TSQLV6.Models.Syllabus", b =>
                {
                    b.Property<int>("SyllabusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("SyllabusID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SyllabusId"));

                    b.Property<string>("AssessmentMethods")
                        .HasColumnType("text");

                    b.Property<int>("CourseId")
                        .HasColumnType("int")
                        .HasColumnName("CourseID");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("LearningObjectives")
                        .HasColumnType("text");

                    b.Property<string>("ReadingMaterials")
                        .HasColumnType("text");

                    b.HasKey("SyllabusId");

                    b.HasIndex("CourseId");

                    b.ToTable("Syllabi");
                });

            modelBuilder.Entity("TSQLV6.Models.Thesis", b =>
                {
                    b.Property<int>("ThesisId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ThesisID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ThesisId"));

                    b.Property<string>("DocumentPath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("StudentID");

                    b.Property<string>("ThesisType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime");

                    b.HasKey("ThesisId");

                    b.HasIndex("StudentId");

                    b.ToTable("Theses");
                });

            modelBuilder.Entity("TSQLV6.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("UserType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TSQLV6.Models.Course", b =>
                {
                    b.HasOne("TSQLV6.Models.User", "Lecturer")
                        .WithMany("Courses")
                        .HasForeignKey("LecturerId")
                        .HasConstraintName("FK_Courses_Users");

                    b.Navigation("Lecturer");
                });

            modelBuilder.Entity("TSQLV6.Models.Enrollment", b =>
                {
                    b.HasOne("TSQLV6.Models.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseId")
                        .IsRequired()
                        .HasConstraintName("FK_Enrollments_Courses");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("TSQLV6.Models.Lecturer", b =>
                {
                    b.HasOne("TSQLV6.Models.User", "LecturerNavigation")
                        .WithMany()
                        .HasForeignKey("LecturerId")
                        .IsRequired()
                        .HasConstraintName("FK_Lecturers_Users");

                    b.Navigation("LecturerNavigation");
                });

            modelBuilder.Entity("TSQLV6.Models.Specialization", b =>
                {
                    b.HasOne("TSQLV6.Models.Department", "Department")
                        .WithMany("Specializations")
                        .HasForeignKey("DepartmentId")
                        .IsRequired()
                        .HasConstraintName("FK_Specializations_Departments");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("TSQLV6.Models.Student", b =>
                {
                    b.HasOne("TSQLV6.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .HasConstraintName("FK_Students_Departments");

                    b.HasOne("TSQLV6.Models.Specialization", "Specialization")
                        .WithMany()
                        .HasForeignKey("SpecializationId")
                        .HasConstraintName("FK_Students_Specializations");

                    b.HasOne("TSQLV6.Models.User", "StudentNavigation")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK_Students_Users");

                    b.Navigation("Department");

                    b.Navigation("Specialization");

                    b.Navigation("StudentNavigation");
                });

            modelBuilder.Entity("TSQLV6.Models.Syllabus", b =>
                {
                    b.HasOne("TSQLV6.Models.Course", "Course")
                        .WithMany("Syllabi")
                        .HasForeignKey("CourseId")
                        .IsRequired()
                        .HasConstraintName("FK_Syllabi_Courses");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("TSQLV6.Models.Thesis", b =>
                {
                    b.HasOne("TSQLV6.Models.User", "Student")
                        .WithMany("Theses")
                        .HasForeignKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK_Theses_Users");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("TSQLV6.Models.Course", b =>
                {
                    b.Navigation("Enrollments");

                    b.Navigation("Syllabi");
                });

            modelBuilder.Entity("TSQLV6.Models.Department", b =>
                {
                    b.Navigation("Specializations");
                });

            modelBuilder.Entity("TSQLV6.Models.User", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Theses");
                });
#pragma warning restore 612, 618
        }
    }
}