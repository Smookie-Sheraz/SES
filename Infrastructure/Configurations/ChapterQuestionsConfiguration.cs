using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class ChapterQuestionsConfiguration : IEntityTypeConfiguration<ChapterQuestions>
    {
        public void Configure(EntityTypeBuilder<ChapterQuestions> builder)
        {
            builder.ToTable("ChapterQuestions").HasKey(p => p.QuestionId)
                .HasName("PK_ChapterQuestions");
            builder.Property(p => p.QuestionId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.Question)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.QuestionType)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Topic)
                .HasColumnType("nvarchar(max)");
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
            builder.HasOne(p => p.Chapter)
                .WithMany(p => p.ChapterQuestions)
                .HasForeignKey(p => p.ChapterId)
                .HasConstraintName("FK_ChapterQuestions_ChapterId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
