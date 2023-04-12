using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("Test").HasKey(p => p.TestId)
                .HasName("PK_Test");
            builder.Property(p => p.TestId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.TestTitle)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ObtainedMarks)
                .HasColumnType("int");
            builder.Property(p => p.TotalMarks)
                .HasColumnType("int");
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
            builder.HasOne(x => x.Class)
                .WithMany(x => x.Tests)
                .HasForeignKey(x => x.ClassId)
                .HasConstraintName("FK_Test_ClassId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Subject)
                .WithMany(x => x.Tests)
                .HasForeignKey(x => x.SubjectId)
                .HasConstraintName("FK_Test_SubjectId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Book)
                .WithMany(x => x.Tests)
                .HasForeignKey(x => x.BookId)
                .HasConstraintName("FK_Test_BookId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
