using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class SchoolNoticeConfiguration : IEntityTypeConfiguration<SchoolNotice>
    {
        public void Configure(EntityTypeBuilder<SchoolNotice> builder)
        {
            builder.ToTable("SchoolNotice").HasKey(p => p.SchoolNoticeId)
                .HasName("PK_SchoolNotice");
            builder.Property(p => p.SchoolNoticeId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.Title)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Salutation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Recipient)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Body)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Date)
                .HasColumnType("date");
            builder.Property(p => p.BroadcastDate)
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
                .WithMany(x => x.SchoolNotices)
                .HasForeignKey(x => x.IssuingACId)
                .HasConstraintName("FK_SchoolNotice_IssuingACId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
