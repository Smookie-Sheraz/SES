using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.ToTable("Setup_Gender").HasKey(p => p.GenderId)
                .HasName("PK_Gender");
            builder.Property(p => p.GenderId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.WGender)
                .HasColumnType("nvarchar(max)");
            //builder.HasOne(p => p.Campus)
            //    .WithMany(a => a.Departments)
            //    .IsRequired()
            //    .HasForeignKey(fk => fk.CampusID)
            //    .HasConstraintName("FK_Deparment_CampusId")
            //    .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
