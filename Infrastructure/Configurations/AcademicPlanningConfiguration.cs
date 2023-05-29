using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class AcademicPlanningsConfiguration : IEntityTypeConfiguration<AcademicPlannings>
    {
        public void Configure(EntityTypeBuilder<AcademicPlannings> builder)
        {
            builder.ToTable("AcademicPlanning").HasKey(p => p.AcademicPlanningsId)
                .HasName("PK_AcademicPlanning");
            builder.Property(p => p.AcademicPlanningsId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.PlanName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.PlannedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.StartDate)
                .HasColumnType("date");
            builder.Property(p => p.EndDate)
                .HasColumnType("date");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.CreatedDate)
                .HasColumnType("date");
            builder.Property(p => p.CreatedBy)
                .HasColumnType("nvarchar(30)");
            builder.Property(p => p.DeletedOn)
                .HasColumnType("date");
            builder.Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.HasOne(p => p.Employee)
                .WithMany(p => p.AcademicPlannings)
                .HasForeignKey(FK => FK.EmployeeId)
                .HasConstraintName("FK_AcademicPlannings_EmployeeId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Class)
                .WithMany(p => p.AcademicPlannings)
                .HasForeignKey(FK => FK.ClassId)
                .HasConstraintName("FK_AcademicPlannings_ClassId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Subject)
                .WithMany(p => p.Plans)
                .HasForeignKey(FK => FK.SubjectId)
                .HasConstraintName("FK_AcademicPlannings_SubjectId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Book)
                .WithMany(p => p.AcademicPlannings)
                .HasForeignKey(FK => FK.BookId)
                .HasConstraintName("FK_AcademicPlannings_BookId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
