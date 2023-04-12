using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class SubTopicAllocationConfiguration : IEntityTypeConfiguration<SubTopicAllocation>
    {
        public void Configure(EntityTypeBuilder<SubTopicAllocation> builder)
        {
            builder.ToTable("SubTopicAllocation").HasKey(p => p.SubTopicAllocationId)
                .HasName("PK_SubTopicAllocation");
            builder.Property(p => p.SubTopicAllocationId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.StartDate)
                .HasColumnType("date");
            builder.Property(p => p.EndDate)
                .HasColumnType("date");
            builder.Property(p => p.ModifiedDate)
                .HasColumnType("date");
            builder.Property(p => p.IsAllocated)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.IsActive)
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
            builder.Property(p => p.WorkBookStartPage)
                .HasColumnType("int");
            builder.Property(p => p.WorkBookEndPage)
                .HasColumnType("int");
            builder.HasOne(p => p.WorkBook)
                .WithMany(p => p.SubTopicAllocations)
                .HasForeignKey(p => p.WorkBookId)
                .HasConstraintName("FK_SubTopicAllocation_WorkBookId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Topic)
                 .WithMany(p => p.SubTopicAllocations)
                 .HasForeignKey(FK => FK.TopicId)
                 .HasConstraintName("FK_SubTopicAllocation_TopicId")
                 .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Term)
                .WithMany(p => p.SubTopicAllocations)
                .HasForeignKey(FK => FK.TermId)
                .HasConstraintName("FK_SubTopicAllocation_TermId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.SubTopic)
                .WithMany(p => p.SubTopicAllocations)
                .HasForeignKey(FK => FK.SubTopicId)
                .HasConstraintName("FK_SubTopicAllocation_SubTopicId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Section)
                .WithMany(p => p.SubTopicAllocations)
                .HasForeignKey(FK => FK.SectionId)
                .HasConstraintName("FK_SubTopicAllocation_SectionId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Plan)
                .WithMany(p => p.SubTopicAllocations)
                .HasForeignKey(FK => FK.PlanId)
                .HasConstraintName("FK_SubTopicAllocation_PlanId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
