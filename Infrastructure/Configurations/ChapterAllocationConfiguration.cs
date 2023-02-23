using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class ChapterAllocationConfiguration : IEntityTypeConfiguration<ChapterAllocation>
    {
        public void Configure(EntityTypeBuilder<ChapterAllocation> builder)
        {
            builder.ToTable("ChapterAllocation").HasKey(p => p.ChapterAllocationId)
                .HasName("PK_ChapterAllocation");
            builder.Property(p => p.ChapterAllocationId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.StartDate)
                .HasColumnType("date");
            builder.Property(p => p.EndDate)
                .HasColumnType("date");
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
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.WorkBookStartPage)
                .HasColumnType("int");
            builder.Property(p => p.WorkBookEndPage)
                .HasColumnType("int");
            builder.HasOne(p => p.WorkBook)
                .WithMany(p => p.ChapterAllocations)
                .HasForeignKey(p => p.WorkBookId)
                .HasConstraintName("FK_ChapterAllocation_WorkBookId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Chapter)
                 .WithMany(p => p.ChapterAllocations)
                 .HasForeignKey(FK => FK.ChapterId)
                 .HasConstraintName("FK_ChapterAllocation_ChapterId")
                 .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Unit)
                 .WithMany(p => p.ChapterAllocations)
                 .HasForeignKey(FK => FK.UnitId)
                 .HasConstraintName("FK_ChapterAllocation_UnitId")
                 .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Term)
                .WithMany(p => p.ChapterAllocations)
                .HasForeignKey(FK => FK.TermId)
                .HasConstraintName("FK_ChapterAllocation_TermId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
