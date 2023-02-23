using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class ShooraConfiguration : IEntityTypeConfiguration<Shoora>
    {
        public void Configure(EntityTypeBuilder<Shoora> builder)
        {
            builder.ToTable("Shoora").HasKey(p => p.ShooraId)
                .HasName("PK_Shoora");
            builder.Property(p => p.ShooraId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.FName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.LName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FatherName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SpouseName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MaritalStatus)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Mobile)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.DOB)
                .HasColumnType("date");
            builder.Property(p => p.CNICNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.CNICIssueDate)
                .HasColumnType("date");
            builder.Property(p => p.CNICExpiryDate)
                .HasColumnType("date");
            builder.Property(p => p.Email)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Address)
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
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.HasOne(p => p.Gender)
                .WithMany(p => p.Shooras)
                .HasForeignKey(FK => FK.GenderId)
                .HasConstraintName("FK_Shoora_GenderId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.School)
                .WithMany(p => p.Shooras)
                .HasForeignKey(FK => FK.SchoolId)
                .HasConstraintName("FK_Shoora_SchoolId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Campus)
                .WithMany(p => p.Shooras)
                .HasForeignKey(FK => FK.CampdusId)
                .HasConstraintName("FK_Shoora_CampusId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
