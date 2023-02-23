using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class YearConfiguration : IEntityTypeConfiguration<Year>
    {
        public void Configure(EntityTypeBuilder<Year> builder)
        {
            builder.ToTable("Setup_Year").HasKey(p => p.YearId)
                .HasName("PK_Year");
            builder.Property(p => p.YearId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.YearName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.StartDate)
                .HasColumnType("date");
            builder.Property(p => p.EndDate)
                .HasColumnType("date");
            builder.Property(p => p.TotalDays)
                .HasColumnType("int");
            builder.Property(p => p.TotalSatSundays)
                .HasColumnType("int");
            builder.Property(p => p.AssesmentDays)
                .HasColumnType("int");
            builder.Property(p => p.Holidays)
                .HasColumnType("int");
            builder.Property(p => p.TotalSchoolDays)
                .HasColumnType("int");
            builder.Property(p => p.TotalAssesWiseSchoolDays)
                .HasColumnType("int");
            builder.Property(p => p.IsLeapYear)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.ModifiedDate)
                .HasColumnType("date");
            builder.Property(p => p.ModifiedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.CreatedDate)
                .HasColumnType("date");
            builder.Property(p => p.CreatedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.DeletedOn)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.HasMany(p => p.Terms)
                .WithOne(p => p.Year)
                .HasForeignKey(FK => FK.YearId)
                .HasConstraintName("FK_Term_YearId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
