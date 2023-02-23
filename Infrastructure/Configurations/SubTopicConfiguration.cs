using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class SubTopicConfiguration : IEntityTypeConfiguration<SubTopic>
    {
        public void Configure(EntityTypeBuilder<SubTopic> builder)
        {
            builder.ToTable("SubTopic").HasKey(p => p.SubTopicId)
                .HasName("PK_SubTopic");
            builder.Property(p => p.SubTopicId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.SubTopicName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.StartPage)
                .HasColumnType("int");
            builder.Property(p => p.EndPage)
                .HasColumnType("int");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
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
            builder.HasOne(p => p.Topic)
                 .WithMany(p => p.SubTopics)
                 .HasForeignKey(FK => FK.TopicId)
                 .HasConstraintName("FK_SubTopic_TopicId")
                 .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
