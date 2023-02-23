using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class UserPermissionsConfiguration : IEntityTypeConfiguration<UserPermissions>
    {
        public void Configure(EntityTypeBuilder<UserPermissions> builder)
        {
            builder.ToTable("UserPermissions").HasKey(p => p.UserPermissionId)
                .HasName("PK_UserPermissions");
            builder.Property(p => p.UserPermissionId)
                .ValueGeneratedOnAdd();
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
            builder.HasOne(p => p.Role)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(p => p.RoleId)
                .HasConstraintName("FK_UserPermissions_RoleId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(p => p.PermissionId)
                .HasConstraintName("FK_UserPermissions_PermissionId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
