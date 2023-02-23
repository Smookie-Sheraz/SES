using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class TopicAllocationConfiguration : IEntityTypeConfiguration<TopicAllocation>
    {
        public void Configure(EntityTypeBuilder<TopicAllocation> builder)
        {
            builder.ToTable("TopicAllocation").HasKey(p => p.TopicAllocationId)
                .HasName("PK_TopicAllocation");
            builder.Property(p => p.TopicAllocationId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.TMethodDesc)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.StartDate)
                .HasColumnType("date");
            builder.Property(p => p.EndDate)
                .HasColumnType("date");
            builder.Property(p => p.ModifiedDate)
                .HasColumnType("date");
            builder.Property(p => p.IsAllocated)
                .HasColumnType("bit")
                .HasDefaultValue(true);
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
                .WithMany(p => p.TopicAllocations)
                .HasForeignKey(p => p.WorkBookId)
                .HasConstraintName("FK_TopicAllocation_WorkBookId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Topic)
                 .WithMany(p => p.TopicAllocations)
                 .HasForeignKey(FK => FK.TopicId)
                 .HasConstraintName("FK_TopicAllocation_TopicId")
                 .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Term)
                .WithMany(p => p.TopicAllocations)
                .HasForeignKey(FK => FK.TermId)
                .HasConstraintName("FK_TopicAllocation_TermId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Chapter)
                .WithMany(p => p.TopicAllocations)
                .HasForeignKey(FK => FK.ChapterId)
                .HasConstraintName("FK_TopicAllocation_ChapterId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.TeachingMethodology)
                .WithMany(p => p.TopicAllocations)
                .HasConstraintName("FK_TopicAllocation_TeachingMethodologyId")
                .HasForeignKey(FK => FK.TeachingMethodologyId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
