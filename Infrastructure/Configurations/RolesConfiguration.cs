using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class RolesConfiguration : IEntityTypeConfiguration<Entities.Models.Roles>
    {
        public void Configure(EntityTypeBuilder<Entities.Models.Roles> builder)
        {
            builder.ToTable("Roles").HasKey(p => p.RoleId)
                .HasName("PK_Role");
            builder.Property(p => p.RoleId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.RollName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.CreatedDate)
                .HasColumnType("date");
            builder.Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.Property(p => p.DeletedOn)
                .HasColumnType("date");
            builder.Property(p => p.ModifiedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ModifiedDate)
                .HasColumnType("date");
            builder.Property(p => p.ModifiedDate)
                .HasColumnType("date");
        }
    }
}
