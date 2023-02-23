using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee").HasKey(p => p.EmployeeId)
                .HasName("PK_Employee");
            builder.Property(p => p.EmployeeId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.EmployeeCode)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.LName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FatherName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SpouseName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MaritalStatus)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.UserImageUrl)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Mobile)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.DOB)
                .HasColumnType("date");
            builder.Property(p => p.CNICNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.CNICIssueDate)
                .HasColumnType("date");
            builder.Property(p => p.CNICExpiryDate)
                .HasColumnType("date");
            builder.Property(p => p.Email)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Address)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.JoiningDate)
                .HasColumnType("date");
            builder.Property(p => p.ProbationPeriod)
                .HasColumnType("int");
            builder.Property(p => p.StartofProbationDate)
                .HasColumnType("date");
            builder.Property(p => p.EndofProbationDate)
                .HasColumnType("date");
            builder.Property(p => p.ConfirmationDate)
                .HasColumnType("date");
            builder.Property(p => p.EmployeeType)
                .HasColumnType("int");
            builder.Property(p => p.Period)
                .HasColumnType("int");
            builder.Property(p => p.StartofPeriodDate)
                .HasColumnType("date");
            builder.Property(p => p.EndofPeriodDate)
                .HasColumnType("date");
            builder.HasOne(p => p.Department)
                .WithMany(a => a.Employees)
                .HasForeignKey(fk => fk.DepartmentId)
                .HasConstraintName("FK_Employee_DepartmentId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.SubDepartment)
                .WithMany(a => a.Employees)
                .HasForeignKey(fk => fk.SubDepartmentId)
                .HasConstraintName("FK_Employee_SubDepartmentId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Designation)
                .WithMany(a => a.Employees)
                .HasForeignKey(fk => fk.DesignationId)
                .HasConstraintName("FK_Employee_DesignationId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Gender)
                .WithMany(p => p.Employees)
                .HasForeignKey(FK => FK.GenderId)
                .HasConstraintName("FK_Employee_GenderId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.School)
                .WithMany(p => p.Employees)
                .HasForeignKey(FK => FK.SchoolId)
                .HasConstraintName("FK_Employee_SchoolId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Campus)
                .WithMany(p => p.Employees)
                .HasForeignKey(FK => FK.CampusId)
                .HasConstraintName("FK_Employee_CampusId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.SchoolSection)
                .WithMany(p => p.Employees)
                .HasForeignKey(FK => FK.SchoolSectionId)
                .HasConstraintName("FK_Employee_SchoolSectionId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Role)
                .WithMany(p => p.Employees)
                .HasForeignKey(FK => FK.RoleId)
                .HasConstraintName("FK_Employee_RoleId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
