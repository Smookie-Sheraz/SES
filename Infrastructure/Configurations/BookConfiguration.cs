using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Setup_Book").HasKey(p => p.BookId)
                .HasName("PK_Book");
            builder.Property(p => p.BookId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.BookName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.BookCode)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Author)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Publisher)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.PublishDate)
                .HasColumnType("date");
            builder.Property(p => p.ResourceBook)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ResourceBookPath)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.IsWorkBook)
                .HasColumnType("bit");
            builder.Property(p => p.CreatedDate)
                .HasColumnType("date");
            builder.Property(p => p.CreatedBy)
                .HasColumnType("nvarchar(30)");
            builder.Property(p => p.DeletedOn)
                .HasColumnType("date");
            builder.Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.HasOne(p => p.Subject)
                 .WithMany(p => p.Books)
                 .HasForeignKey(FK => FK.SubjectId)
                 .HasConstraintName("FK_Book_SubjectId")
                 .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Grade)
                 .WithMany(p => p.Books)
                 .HasForeignKey(FK => FK.GradeId)
                 .HasConstraintName("FK_Book_GradeId")
                 .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.ResourceNoteBook)
                 .WithMany(p => p.Books)
                 .HasForeignKey(FK => FK.ResourceNoteBookId)
                 .HasConstraintName("FK_Book_ResourceNoteBookId")
                 .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
