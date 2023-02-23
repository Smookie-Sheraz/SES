using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class SchoolConfiguration : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.ToTable("Setup_School").HasKey(p => p.SchoolId)
                .HasName("PK_School");
            builder.Property(p => p.SchoolId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.SchoolName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.RegistrationNo)
                .HasColumnType("int");
            builder.Property(p => p.CEOName)
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
        }
    }
}
