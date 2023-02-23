using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class ResourceNoteBookConfiguration : IEntityTypeConfiguration<ResourceNoteBook>
    {
        public void Configure(EntityTypeBuilder<ResourceNoteBook> builder)
        {
            builder.ToTable("ResourceNoteBook").HasKey(p => p.ResourceNoteBookId)
                .HasName("PK_ResourceNoteBook");
            builder.Property(p => p.ResourceNoteBookId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.NoteBookName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.StartDate)
                .HasColumnType("date");
            builder.Property(p => p.EndDate)
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
        }
    }
}
