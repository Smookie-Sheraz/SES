using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class ChapterAnswersConfiguration : IEntityTypeConfiguration<ChapterAnswers>
    {
        public void Configure(EntityTypeBuilder<ChapterAnswers> builder)
        {
            builder.ToTable("ChapterAnswers").HasKey(p => p.AnswerId)
                .HasName("PK_ChapterAnswers");
            builder.Property(p => p.QuestionId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.Choice)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsTrue)
                .HasColumnType("bit")
                .HasDefaultValue(false);
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
            builder.HasOne(p => p.Question)
                .WithMany(p => p.Answers)
                .HasForeignKey(p => p.QuestionId)
                .HasConstraintName("FK_ChapterAnswers_QuestionId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
