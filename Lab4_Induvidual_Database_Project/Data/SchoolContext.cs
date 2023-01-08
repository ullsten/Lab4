using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Lab4_Induvidual_Database_Project.Models;

namespace Lab4_Induvidual_Database_Project.Data
{
    public partial class SchoolContext : DbContext
    {
        public SchoolContext()
        {
        }

        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<GetAllStaff> GetAllStaffs { get; set; } = null!;
        public virtual DbSet<Grade> Grades { get; set; } = null!;
        public virtual DbSet<GradesLastMonth> GradesLastMonths { get; set; } = null!;
        public virtual DbSet<PayrollOffice> PayrollOffices { get; set; } = null!;
        public virtual DbSet<Position> Positions { get; set; } = null!;
        public virtual DbSet<Salary> Salaries { get; set; } = null!;
        public virtual DbSet<ShowTeacherWithCourse> ShowTeacherWithCourses { get; set; } = null!;
        public virtual DbSet<StaffAdmin> StaffAdmins { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<TestTable> TestTables { get; set; } = null!;
        public virtual DbSet<staff> staff { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=ULLSTENLENOVO; Initial Catalog=School; Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.City).HasMaxLength(25);

                entity.Property(e => e.Homeland).HasMaxLength(25);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StreetAddress).HasMaxLength(50);
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");

                entity.Property(e => e.ClassName).HasMaxLength(50);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.CourseName).HasMaxLength(50);

                entity.Property(e => e.CourseStatus).HasMaxLength(10);
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("Exam");

                entity.Property(e => e.DateOfGrade).HasColumnType("date");

                entity.Property(e => e.FkCourseId).HasColumnName("FK_CourseId");

                entity.Property(e => e.FkGradeId).HasColumnName("FK_GradeId");

                entity.Property(e => e.FkStaffAdminId).HasColumnName("FK_StaffAdminId");

                entity.Property(e => e.FkStudentId).HasColumnName("FK_StudentId");

                entity.Property(e => e.StartDateCourse).HasColumnType("date");

                entity.HasOne(d => d.FkCourse)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.FkCourseId)
                    .HasConstraintName("FK_Exam_Course");

                entity.HasOne(d => d.FkGrade)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.FkGradeId)
                    .HasConstraintName("FK__Exam__FK_GradeId__04E4BC85");

                entity.HasOne(d => d.FkStaffAdmin)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.FkStaffAdminId)
                    .HasConstraintName("FK_Exam_StaffAdmin");

                entity.HasOne(d => d.FkStudent)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.FkStudentId)
                    .HasConstraintName("FK_Exam_Student");
            });

            modelBuilder.Entity<GetAllStaff>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GetAllStaff");

                entity.Property(e => e.DateOfEmployment)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Date of employment");

                entity.Property(e => e.EmployedYear)
                    .HasMaxLength(4)
                    .HasColumnName("Employed year");

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(101);

                entity.Property(e => e.Position).HasMaxLength(20);

                entity.Property(e => e.Ssn)
                    .HasMaxLength(15)
                    .HasColumnName("SSN");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("Grade");
            });

            modelBuilder.Entity<GradesLastMonth>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Grades last month");

                entity.Property(e => e.Course).HasMaxLength(50);

                entity.Property(e => e.DateOfGrade)
                    .HasColumnType("date")
                    .HasColumnName("Date of grade");

                entity.Property(e => e.Student).HasMaxLength(101);
            });

            modelBuilder.Entity<PayrollOffice>(entity =>
            {
                entity.ToTable("PayrollOffice");

                entity.Property(e => e.PaymentDate).HasColumnType("date");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("Position");

                entity.Property(e => e.PositionName).HasMaxLength(20);
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.ToTable("Salary");

                entity.Property(e => e.SalaryType).HasMaxLength(15);
            });

            modelBuilder.Entity<ShowTeacherWithCourse>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ShowTeacherWithCourse");

                entity.Property(e => e.AssignedCourse)
                    .HasMaxLength(50)
                    .HasColumnName("Assigned Course");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(101);

                entity.Property(e => e.Position).HasMaxLength(20);
            });

            modelBuilder.Entity<StaffAdmin>(entity =>
            {
                entity.ToTable("StaffAdmin");

                entity.Property(e => e.FkAddressId).HasColumnName("FK_AddressId");

                entity.Property(e => e.FkPositionId).HasColumnName("FK_PositionId");

                entity.Property(e => e.FkSalaryId).HasColumnName("FK_SalaryId");

                entity.Property(e => e.FkStaffId).HasColumnName("FK_StaffId");

                entity.Property(e => e.Salary).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.FkAddress)
                    .WithMany(p => p.StaffAdmins)
                    .HasForeignKey(d => d.FkAddressId)
                    .HasConstraintName("FK_StaffAdmin_Address");

                entity.HasOne(d => d.FkPosition)
                    .WithMany(p => p.StaffAdmins)
                    .HasForeignKey(d => d.FkPositionId)
                    .HasConstraintName("FK__StaffAdmi__FK_Po__3D2915A8");

                entity.HasOne(d => d.FkSalary)
                    .WithMany(p => p.StaffAdmins)
                    .HasForeignKey(d => d.FkSalaryId)
                    .HasConstraintName("FK_StaffAdmin_Salary");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.DayOfBirth)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FkAddressId).HasColumnName("FK_AddressId");

                entity.Property(e => e.FkClassId).HasColumnName("FK_ClassId");

                entity.Property(e => e.Gender)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.SecurityNumber).HasMaxLength(15);

                entity.HasOne(d => d.FkAddress)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.FkAddressId)
                    .HasConstraintName("FK_Student_Address");

                entity.HasOne(d => d.FkClass)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.FkClassId)
                    .HasConstraintName("FK_Student_Class");
            });

            modelBuilder.Entity<TestTable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TestTable");

                entity.Property(e => e.Testing)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.ToTable("Staff");

                entity.Property(e => e.DayOfBirth)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.SecurityNumber).HasMaxLength(15);

                entity.Property(e => e.YearOnSchool).HasMaxLength(4);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
