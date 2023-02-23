using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.ToTable("Setup_Section").HasKey(p => p.SectionId)
                .HasName("PK_Section");
            builder.Property(p => p.SectionId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.SectionName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SectionCode)
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
            //builder.HasOne(p => p.Book)
            //    .WithMany(p => p.Sections)
            //    .IsRequired()
            //    .HasForeignKey(FK => FK.BookId)
            //    .HasConstraintName("FK_Section_BookId")
            //    .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Grade)
                .WithMany(p => p.Sections)
                .HasForeignKey(FK => FK.GradeId)
                .HasConstraintName("FK_Section_GradeId")
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
