using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.ToTable("Topic").HasKey(p => p.TopicId)
                .HasName("PK_Topic");
            builder.Property(p => p.TopicId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.TopicName)
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
            builder.HasOne(p => p.Chapter)
                 .WithMany(p => p.Topics)
                 .HasForeignKey(FK => FK.ChapterId)
                 .HasConstraintName("FK_Topic_ChapterId")
                 .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
