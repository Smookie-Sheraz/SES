using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class SchoolSectionConfiguration : IEntityTypeConfiguration<SchoolSection>
    {
        public void Configure(EntityTypeBuilder<SchoolSection> builder)
        {
            builder.ToTable("Setup_SchoolSection").HasKey(p => p.SchoolSectionId)
                .HasName("PK_SchoolSection");
            builder.Property(p => p.SchoolSectionId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.SectionName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SectionHead)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Abbrevation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.PhoneNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.address)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Email)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ModifiedDate)
                .HasColumnType("date");
            builder.Property(p => p.ModifiedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.CreatedDate)
                .HasColumnType("date");
            builder.Property(p => p.CreatedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.DeletedOn)
                .HasColumnType("date");
            builder.Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.HasOne(x => x.Campus)
                .WithMany(x => x.sections)
                .HasForeignKey(x => x.CampusId)
                .HasConstraintName("FK_SchoolSection_CampusId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.School)
                .WithMany(x => x.SchoolSections)
                .HasForeignKey(x => x.SchoolId)
                .HasConstraintName("FK_SchoolSection_SchoolId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.AssistantCoordinator)
                .WithMany(x => x.SchoolSections)
                .HasForeignKey(x => x.AssistantCoordinatorId)
                .HasConstraintName("FK_SchoolSection_AssistantCoordinatorId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
