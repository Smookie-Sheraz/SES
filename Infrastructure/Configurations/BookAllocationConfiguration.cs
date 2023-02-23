using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class BookAllocationConfiguration : IEntityTypeConfiguration<BookAllocation>
    {
        public void Configure(EntityTypeBuilder<BookAllocation> builder)
        {
            builder.ToTable("Setup_BookAllocation").HasKey(p => p.BookAllocationId)
                .HasName("PK_BookAllocation");
            builder.Property(p => p.BookAllocationId)
                .ValueGeneratedOnAdd();
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
            builder.Property(p => p.preExist)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.HasOne(p => p.Book)
                 .WithMany(p => p.BookAllocations)
                 .HasForeignKey(FK => FK.BookId)
                 .HasConstraintName("FK_BookAllocation_BookId")
                 .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Section)
                 .WithMany(p => p.BookAllocations)
                 .HasForeignKey(FK => FK.SectionId)
                 .HasConstraintName("FK_BookAllocation_SectionId")
                 .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
