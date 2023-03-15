using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable("Setup_Grade").HasKey(p => p.GradeId)
                .HasName("PK_Grade");
            builder.Property(p => p.GradeId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.GradeName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.GradeCode)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.CreatedDate)
                .HasColumnType("date");
            builder.Property(p => p.CreatedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.DeletedOn)
                .HasColumnType("date");
            builder.Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.HasOne(p => p.SchoolSection)
                .WithMany(a => a.Grades)
                .HasForeignKey(fk => fk.SchoolSectionId)
                .HasConstraintName("FK_Grade_SchoolSectionId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.GradeManager)
                .WithMany(a => a.Grades)
                .HasForeignKey(fk => fk.GradeManagerId)
                .HasConstraintName("FK_Grade_GradeManagerId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
