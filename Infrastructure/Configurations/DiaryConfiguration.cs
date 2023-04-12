using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class DiaryConfiguration : IEntityTypeConfiguration<Diary>
    {
        public void Configure(EntityTypeBuilder<Diary> builder)
        {
            builder.ToTable("Diary").HasKey(p => p.DiaryId)
                .HasName("PK_Diary");
            builder.Property(p => p.DiaryId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.ClassWork)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.HomeWork)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Date)
                .HasColumnType("date");
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
            builder.HasOne(x => x.Test)
                .WithMany(x => x.Diaries)
                .HasForeignKey(x => x.TestId)
                .HasConstraintName("FK_Diary_TestId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Subject)
                .WithMany(x => x.Diaries)
                .HasForeignKey(x => x.SubjectId)
                .HasConstraintName("FK_Diary_SubjectId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Class)
                .WithMany(x => x.Diaries)
                .HasForeignKey(x => x.ClassId)
                .HasConstraintName("FK_Diary_ClassId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
