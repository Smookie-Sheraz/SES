using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class CampusConfiguration : IEntityTypeConfiguration<Campus>
    {
        public void Configure(EntityTypeBuilder<Campus> builder)
        {
            builder.ToTable("Setup_Campus").HasKey(p => p.CampusId)
                .HasName("PK_Campus");
            builder.Property(p => p.CampusId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.CampusName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.PrincipalName)
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
            builder.HasOne(x => x.School)
                .WithMany(x => x.campuses)
                .HasForeignKey(x => x.SchoolId)
                .HasConstraintName("FK_Campus_ShoolId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
