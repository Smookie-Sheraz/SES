using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User").HasKey(p => p.UserId)
                .HasName("PK_User");
            builder.Property(p => p.UserId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.FName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.LName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FatherName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Email)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.UserName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Password)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.UserImageURL)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Role)
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
        }
    }
}
