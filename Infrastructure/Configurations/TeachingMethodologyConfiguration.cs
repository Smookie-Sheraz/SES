using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class TeachingMethodologyConfiguration : IEntityTypeConfiguration<TeachingMethodology>
    {
        public void Configure(EntityTypeBuilder<TeachingMethodology> builder)
        {
            builder.ToTable("TeachingMethodology").HasKey(p => p.TeachingMethodologyId)
                .HasName("PK_TeachingMethodology");
            builder.Property(p => p.TeachingMethodologyId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.TMethodologyName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.ModifiedDate)
                .HasColumnType("date");
            builder.Property(p => p.ModifiedDate)
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
        }
    }
}
