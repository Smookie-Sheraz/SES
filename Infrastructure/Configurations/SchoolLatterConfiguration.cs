using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class SchoolLatterConfiguration : IEntityTypeConfiguration<SchoolLatter>
    {
        public void Configure(EntityTypeBuilder<SchoolLatter> builder)
        {
            builder.ToTable("SchoolLatter").HasKey(p => p.SchoolLatterId)
                .HasName("PK_SchoolLatter");
            builder.Property(p => p.SchoolLatterId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.SenderName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SenderDesignation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SenderContact)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ReceiverName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ReceiverDesignation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ReceiverContact)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Salutation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Body)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Closing)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Date)
                .HasColumnType("date");
            builder.Property(p => p.SendingDate)
                .HasColumnType("date");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.ModifiedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ModifiedDate)
                 .HasColumnType("date");
            builder.Property(p => p.CreatedDate)
                .HasColumnType("date");
            builder.Property(p => p.CreatedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.DeletedOn)
                .HasColumnType("date");
            builder.Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.HasOne(x => x.IssuingAC)
                .WithMany(x => x.SchoolLatters)
                .HasForeignKey(x => x.IssuingACId)
                .HasConstraintName("FK_SchoolLetter_IssuingACId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Student)
                .WithMany(x => x.SchoolLatters)
                .HasForeignKey(x => x.StudentId)
                .HasConstraintName("FK_SchoolLetter_StudentId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Parent)
                .WithMany(x => x.SchoolLatters)
                .HasForeignKey(x => x.ParentId)
                .HasConstraintName("FK_SchoolLetter_ParentId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
