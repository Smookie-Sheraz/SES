using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class SubDepartmentConfiguration : IEntityTypeConfiguration<SubDepartment>
    {
        public void Configure(EntityTypeBuilder<SubDepartment> builder)
        {
            builder.ToTable("Setup_SubDepartment").HasKey(p => p.SubDepartmentId)
                .HasName("PK_SubDepartment");
            builder.Property(p => p.SubDepartmentId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.DepartmentName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.CreatedDate)
                .HasColumnType("date");
            builder.Property(p => p.CreatedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.DeletedOn)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.HasOne(p => p.MainDepartment)
                .WithMany(p => p.SubDepartments)
                .HasForeignKey(FK => FK.MainDepartmentId)
                .HasConstraintName("FK_Subdepartment_DepartmentId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Head)
                .WithMany(x => x.subDepartments)
                .HasForeignKey(FK => FK.HeadId)
                .HasConstraintName("FK_SubDepartment_HeadId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
