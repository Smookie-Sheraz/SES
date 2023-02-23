using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Setup_Department").HasKey(p => p.DepartmentId)
                .HasName("PK_Department");
            builder.Property(p => p.DepartmentId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.DepartmentName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Description)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ShortDescripiton)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.CreatedDate)
                .HasColumnType("date");
            builder.Property(p => p.CreatedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.DeletedOn)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.HasOne(p => p.Shoora)
                .WithMany(p => p.Departments)
                .HasForeignKey(FK => FK.DepartmentHeadId)
                .HasConstraintName("FK_Department_ShooraId")
                .OnDelete(DeleteBehavior.SetNull);
            //builder.HasOne(p => p.Campus)
            //    .WithMany(a => a.Departments)
            //    .IsRequired()
            //    .HasForeignKey(fk => fk.CampusID)
            //    .HasConstraintName("FK_Deparment_CampusId")
            //    .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
