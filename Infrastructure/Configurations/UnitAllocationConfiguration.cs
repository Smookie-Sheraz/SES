using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class UnitAllocationConfiguration : IEntityTypeConfiguration<UnitAllocation>
    {
        public void Configure(EntityTypeBuilder<UnitAllocation> builder)
        {
            builder.ToTable("UnitAllocation").HasKey(p => p.UnitAllocationId)
                .HasName("PK_UnitAllocation");
            builder.Property(p => p.UnitAllocationId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.StartDate)
                .HasColumnType("date");
            builder.Property(p => p.EndDate)
                .HasColumnType("date");
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
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.AreSaturdaysOff)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.WorkBookStartPage)
                .HasColumnType("int");
            builder.Property(p => p.WorkBookEndPage)
                .HasColumnType("int");
            builder.HasOne(p => p.WorkBook)
                .WithMany(p => p.unitAllocations)
                .HasForeignKey(p => p.WorkBookId)
                .HasConstraintName("FK_UnitAllocation_WorkBookId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Unit)
                 .WithMany(p => p.UnitAllocations)
                 .HasForeignKey(FK => FK.UnitId)
                 .HasConstraintName("FK_UnitAllocation_UnitId")
                 .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Term)
                .WithMany(p => p.UnitAllocations)
                .HasForeignKey(FK => FK.TermId)
                .HasConstraintName("FK_UnitAllocation_TermId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Section)
                .WithMany(p => p.UnitAllocations)
                .HasForeignKey(FK => FK.SectionId)
                .HasConstraintName("FK_UnitAllocation_SectionId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Plan)
                .WithMany(p => p.UnitAllocations)
                .HasForeignKey(FK => FK.PlanId)
                .HasConstraintName("FK_UnitAllocation_PlanId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
