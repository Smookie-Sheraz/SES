using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class SubjectTeacherAllocationConfiguration : IEntityTypeConfiguration<SubjectTeacherAllocation>
    {
        public void Configure(EntityTypeBuilder<SubjectTeacherAllocation> builder)
        {
            builder.ToTable("SubjectTeacherAllocation").HasKey(p => p.SubjectTeacherAllocationId)
                .HasName("PK_SubjectTeacherAllocation");
            builder.Property(p => p.SubjectTeacherAllocationId)
                .ValueGeneratedOnAdd();
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
            builder.HasOne(p => p.Book)
                 .WithMany(p => p.SubjectTeacherAllocations)
                 .HasForeignKey(FK => FK.BookId)
                 .HasConstraintName("FK_SubjectTeacherAllocation_BookId")
                 .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Section)
                 .WithMany(p => p.SubjectTeacherAllocations)
                 .HasForeignKey(FK => FK.SectionId)
                 .HasConstraintName("FK_SubjectTeacherAllocation_SectionId")
                 .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Employee)
                 .WithMany(p => p.SubjectTeacherAllocations)
                 .HasForeignKey(FK => FK.EmployeeId)
                 .HasConstraintName("FK_SubjectTeacherAllocation_EmployeeId")
                 .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
