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
        }
    }
}
