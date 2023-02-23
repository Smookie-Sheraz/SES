﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(SchoolDbContext))]
    [Migration("20221123080913_GradeModules")]
    partial class GradeModules
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Entities.Models.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"), 1L, 1);

                    b.Property<string>("Author")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("BookCode")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("BookName")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("date");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("PublishDate")
                        .HasColumnType("date");

                    b.Property<string>("Publisher")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int?>("SubjectId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("BookId")
                        .HasName("PK_Book");

                    b.HasIndex("SubjectId");

                    b.ToTable("Setup_Book", (string)null);
                });

            modelBuilder.Entity("Entities.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentId"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("date");

                    b.Property<string>("DeletedOn")
                        .HasColumnType("varchar(30)");

                    b.Property<int?>("DepartmentHeadId")
                        .HasColumnType("int");

                    b.Property<string>("DepartmentName")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("ShortDescripiton")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.HasKey("DepartmentId")
                        .HasName("PK_Department");

                    b.HasIndex("DepartmentHeadId");

                    b.ToTable("Setup_Department", (string)null);
                });

            modelBuilder.Entity("Entities.Models.Designation", b =>
                {
                    b.Property<int>("DesignationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DesignationId"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("date");

                    b.Property<string>("DeletedOn")
                        .HasColumnType("varchar(30)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("DesignationId")
                        .HasName("PK_Designation");

                    b.ToTable("Setup_Designation", (string)null);
                });

            modelBuilder.Entity("Entities.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("varchar(1000)");

                    b.Property<DateTime?>("CNICExpiryDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("CNICIssueDate")
                        .HasColumnType("date");

                    b.Property<string>("CNICNo")
                        .HasColumnType("varchar(15)");

                    b.Property<DateTime?>("ConfirmationDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DOB")
                        .HasColumnType("date");

                    b.Property<int?>("DepartmentId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("DesignationId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("EmployeeCode")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<int?>("EmployeeType")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndofPeriodDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("EndofProbationDate")
                        .HasColumnType("date");

                    b.Property<string>("FName")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FatherName")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FieldOfSpecialization")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GenderId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("JoiningDate")
                        .HasColumnType("date");

                    b.Property<string>("LName")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("MaritalStatus")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Mobile")
                        .HasColumnType("varchar(11)");

                    b.Property<int?>("Period")
                        .HasColumnType("int");

                    b.Property<int?>("ProbationPeriod")
                        .HasColumnType("int");

                    b.Property<string>("SpouseName")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("StartofPeriodDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("StartofProbationDate")
                        .HasColumnType("date");

                    b.Property<int?>("SubDepartmentId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("EmployeeId")
                        .HasName("PK_Employee");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("DesignationId");

                    b.HasIndex("GenderId");

                    b.HasIndex("SubDepartmentId");

                    b.ToTable("Employee", (string)null);
                });

            modelBuilder.Entity("Entities.Models.Gender", b =>
                {
                    b.Property<int>("GenderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GenderId"), 1L, 1);

                    b.Property<string>("WGender")
                        .HasColumnType("varchar(15)");

                    b.HasKey("GenderId")
                        .HasName("PK_Gender");

                    b.ToTable("Setup_Gender", (string)null);
                });

            modelBuilder.Entity("Entities.Models.Grade", b =>
                {
                    b.Property<int>("GradeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GradeId"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("date");

                    b.Property<string>("GradeCode")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("GradeName")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.HasKey("GradeId")
                        .HasName("PK_Grade");

                    b.ToTable("Setup_Grade", (string)null);
                });

            modelBuilder.Entity("Entities.Models.Head", b =>
                {
                    b.Property<int>("HeadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HeadId"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("date");

                    b.Property<string>("DeletedOn")
                        .HasColumnType("varchar(30)");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int?>("ShooraId")
                        .HasColumnType("int");

                    b.HasKey("HeadId")
                        .HasName("PK_Head");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ShooraId");

                    b.ToTable("Setup_Head", (string)null);
                });

            modelBuilder.Entity("Entities.Models.Section", b =>
                {
                    b.Property<int>("SectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SectionId"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("date");

                    b.Property<int?>("GradeId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("SectionCode")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("SectionName")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("SectionId")
                        .HasName("PK_Section");

                    b.HasIndex("GradeId");

                    b.ToTable("Setup_Section", (string)null);
                });

            modelBuilder.Entity("Entities.Models.SectionBook", b =>
                {
                    b.Property<int>("SectionBookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SectionBookId"), 1L, 1);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.HasKey("SectionBookId");

                    b.HasIndex("BookId");

                    b.HasIndex("SectionId")
                        .IsUnique();

                    b.ToTable("SectionBook");
                });

            modelBuilder.Entity("Entities.Models.Shoora", b =>
                {
                    b.Property<int>("ShooraId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShooraId"), 1L, 1);

                    b.Property<string>("Address")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<DateTime?>("CNICExpiryDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("CNICIssueDate")
                        .HasColumnType("date");

                    b.Property<string>("CNICNo")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DOB")
                        .HasColumnType("date");

                    b.Property<string>("DeletedOn")
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("FName")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("FatherName")
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.Property<int?>("GenderId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LName")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("MaritalStatus")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Mobile")
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)");

                    b.Property<string>("SpouseName")
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.HasKey("ShooraId")
                        .HasName("PK_Shoora");

                    b.HasIndex("GenderId");

                    b.ToTable("Shoora", (string)null);
                });

            modelBuilder.Entity("Entities.Models.SubDepartment", b =>
                {
                    b.Property<int>("SubDepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubDepartmentId"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("date");

                    b.Property<string>("DeletedOn")
                        .HasColumnType("varchar(30)");

                    b.Property<string>("DepartmentName")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int?>("HeadId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("MainDepartmentId")
                        .HasColumnType("int");

                    b.HasKey("SubDepartmentId")
                        .HasName("PK_SubDepartment");

                    b.HasIndex("HeadId")
                        .IsUnique()
                        .HasFilter("[HeadId] IS NOT NULL");

                    b.HasIndex("MainDepartmentId");

                    b.ToTable("Setup_SubDepartment", (string)null);
                });

            modelBuilder.Entity("Entities.Models.Subject", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubjectId"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("date");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("SubjectCode")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("SubjectName")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("SubjectId")
                        .HasName("PK_Subject");

                    b.ToTable("Setup_Subject", (string)null);
                });

            modelBuilder.Entity("Entities.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("FName")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FatherName")
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LName")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Password")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(100)");

                    b.HasKey("UserId")
                        .HasName("PK_User");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Entities.Models.Book", b =>
                {
                    b.HasOne("Entities.Models.Subject", "Subject")
                        .WithMany("Books")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Book_SubjectId");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Entities.Models.Department", b =>
                {
                    b.HasOne("Entities.Models.Shoora", "Shoora")
                        .WithMany("Departments")
                        .HasForeignKey("DepartmentHeadId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_Department_ShooraId");

                    b.Navigation("Shoora");
                });

            modelBuilder.Entity("Entities.Models.Employee", b =>
                {
                    b.HasOne("Entities.Models.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Employee_DepartmentId");

                    b.HasOne("Entities.Models.Designation", "Designation")
                        .WithMany("Employees")
                        .HasForeignKey("DesignationId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("FK_Employee_DesignationId");

                    b.HasOne("Entities.Models.Gender", "Gender")
                        .WithMany("Employees")
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_Employee_GenderId");

                    b.HasOne("Entities.Models.SubDepartment", "SubDepartment")
                        .WithMany("Employees")
                        .HasForeignKey("SubDepartmentId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("FK_Employee_SubDepartmentId");

                    b.Navigation("Department");

                    b.Navigation("Designation");

                    b.Navigation("Gender");

                    b.Navigation("SubDepartment");
                });

            modelBuilder.Entity("Entities.Models.Head", b =>
                {
                    b.HasOne("Entities.Models.Employee", "Employee")
                        .WithMany("Heads")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_Head_EmployeeId");

                    b.HasOne("Entities.Models.Shoora", "Shoora")
                        .WithMany("Heads")
                        .HasForeignKey("ShooraId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_Head_ShooraId");

                    b.Navigation("Employee");

                    b.Navigation("Shoora");
                });

            modelBuilder.Entity("Entities.Models.Section", b =>
                {
                    b.HasOne("Entities.Models.Grade", "Grade")
                        .WithMany("Sections")
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Section_GradeId");

                    b.Navigation("Grade");
                });

            modelBuilder.Entity("Entities.Models.SectionBook", b =>
                {
                    b.HasOne("Entities.Models.Book", "Book")
                        .WithMany("SectionBooks")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.Section", "Section")
                        .WithOne("SectionBook")
                        .HasForeignKey("Entities.Models.SectionBook", "SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("Entities.Models.Shoora", b =>
                {
                    b.HasOne("Entities.Models.Gender", "Gender")
                        .WithMany("Shooras")
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_Shoora_GenderId");

                    b.Navigation("Gender");
                });

            modelBuilder.Entity("Entities.Models.SubDepartment", b =>
                {
                    b.HasOne("Entities.Models.Employee", "Head")
                        .WithOne("SubDepartmentHead")
                        .HasForeignKey("Entities.Models.SubDepartment", "HeadId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_SubDepartment_HeadId");

                    b.HasOne("Entities.Models.Department", "MainDepartment")
                        .WithMany("SubDepartments")
                        .HasForeignKey("MainDepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Subdepartment_DepartmentId");

                    b.Navigation("Head");

                    b.Navigation("MainDepartment");
                });

            modelBuilder.Entity("Entities.Models.Book", b =>
                {
                    b.Navigation("SectionBooks");
                });

            modelBuilder.Entity("Entities.Models.Department", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("SubDepartments");
                });

            modelBuilder.Entity("Entities.Models.Designation", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Entities.Models.Employee", b =>
                {
                    b.Navigation("Heads");

                    b.Navigation("SubDepartmentHead");
                });

            modelBuilder.Entity("Entities.Models.Gender", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Shooras");
                });

            modelBuilder.Entity("Entities.Models.Grade", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("Entities.Models.Section", b =>
                {
                    b.Navigation("SectionBook")
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.Models.Shoora", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("Heads");
                });

            modelBuilder.Entity("Entities.Models.SubDepartment", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Entities.Models.Subject", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
