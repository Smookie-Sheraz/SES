using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class SubjectAllocationConfiguration : IEntityTypeConfiguration<SubjectAllocation>
    {
        public void Configure(EntityTypeBuilder<SubjectAllocation> builder)
        {
            builder.ToTable("SubjectAllocation").HasKey(p => p.SubjectAllocationId)
                .HasName("PK_SubjectAllocation");
            builder.Property(p => p.SubjectAllocationId)
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
            //builder.Property(p => p.preExist)
            //    .HasColumnType("bit")
            //    .HasDefaultValue(false);
            builder.HasOne(p => p.Subject)
                 .WithMany(p => p.SubjectAllocations)
                 .HasForeignKey(FK => FK.SubjectId)
                 .HasConstraintName("FK_SubjectAllocation_SubjectId")
                 .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Section)
                 .WithMany(p => p.SubjectAllocations)
                 .HasForeignKey(FK => FK.SectionId)
                 .HasConstraintName("FK_SubjectAllocation_SectionId")
                 .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
