using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("Setup_Subject").HasKey(p => p.SubjectId)
                .HasName("PK_Subject");
            builder.Property(p => p.SubjectId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.SubjectName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SubjectCode)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.CreatedDate)
                .HasColumnType("date");
            builder.Property(p => p.CreatedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.DeletedOn)
                .HasColumnType("date");
            builder.Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            //builder.HasOne(p => p.Campus)
            //    .WithMany(a => a.Departments)
            //    .IsRequired()
            //    .HasForeignKey(fk => fk.CampusID)
            //    .HasConstraintName("FK_Deparment_CampusId")
            //    .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
