using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.ToTable("Chapter").HasKey(p => p.ChapterId)
                .HasName("PK_Chapter");
            builder.Property(p => p.ChapterId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.ChapterName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SLO)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.StartPage)
                .HasColumnType("int");
            builder.Property(p => p.EndPage)
                .HasColumnType("int");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.IsAllocated)
                .HasColumnType("bit")
                .HasDefaultValue(false);
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
            builder.HasOne(p => p.Unit)
                 .WithMany(p => p.Chapters)
                 .HasForeignKey(FK => FK.UnitId)
                 .HasConstraintName("FK_Chapter_UnitId")
                 .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
