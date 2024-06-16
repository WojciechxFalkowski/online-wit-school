using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TSQLV6.Models;

public partial class UniversityDbContext : DbContext
{
    public UniversityDbContext()
    {
    }

    public UniversityDbContext(DbContextOptions<UniversityDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Lecturer> Lecturers { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Syllabus> Syllabi { get; set; }

    public virtual DbSet<Thesis> Theses { get; set; }

    public virtual DbSet<User> Users { get; set; }

   // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=UniversityDB;Trusted_Connection=True;Trust Server Certificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.AcademicYear).HasMaxLength(4);
            entity.Property(e => e.CourseCode).HasMaxLength(10);
            entity.Property(e => e.CourseName).HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.LecturerId).HasColumnName("LecturerID");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.Courses)
                .HasForeignKey(d => d.LecturerId)
                .HasConstraintName("FK_Courses_Users");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName).HasMaxLength(50);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Grade).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .HasColumnName("StudentID");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollments_Courses");
        });

        modelBuilder.Entity<Lecturer>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AcademicDegree).HasMaxLength(50);
            entity.Property(e => e.LecturerId)
                .ValueGeneratedOnAdd()
                .HasColumnName("LecturerID");

            entity.HasOne(d => d.LecturerNavigation).WithMany()
                .HasForeignKey(d => d.LecturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lecturers_Users");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.Property(e => e.SpecializationId).HasColumnName("SpecializationID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.SpecializationName).HasMaxLength(100);

            entity.HasOne(d => d.Department).WithMany(p => p.Specializations)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Specializations_Departments");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId);
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.SpecializationId).HasColumnName("SpecializationID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.StudyMode).HasMaxLength(50);

            entity.HasOne(d => d.Department).WithMany()
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Students_Departments");

            entity.HasOne(d => d.Specialization).WithMany()
                .HasForeignKey(d => d.SpecializationId)
                .HasConstraintName("FK_Students_Specializations");

            entity.HasOne(d => d.StudentNavigation).WithMany()
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Users");
        });

        modelBuilder.Entity<Syllabus>(entity =>
        {
            entity.Property(e => e.SyllabusId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SyllabusID");
            entity.Property(e => e.AssessmentMethods).HasColumnType("text");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.LearningObjectives).HasColumnType("text");
            entity.Property(e => e.ReadingMaterials).HasColumnType("text");

            entity.HasOne(d => d.Course).WithMany(p => p.Syllabi)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Syllabi_Courses");
        });

        modelBuilder.Entity<Thesis>(entity =>
        {
            entity.Property(e => e.ThesisId).HasColumnName("ThesisID");
            entity.Property(e => e.DocumentPath).HasMaxLength(255);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.ThesisType).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UploadDate).HasColumnType("datetime");

            entity.HasOne(d => d.Student).WithMany(p => p.Theses)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Theses_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.UserType).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
