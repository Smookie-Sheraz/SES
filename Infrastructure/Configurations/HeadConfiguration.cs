using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class HeadConfiguration : IEntityTypeConfiguration<Head>
    {
        public void Configure(EntityTypeBuilder<Head> builder)
        {
            builder.ToTable("Setup_Head").HasKey(p => p.HeadId)
                .HasName("PK_Head");
            builder.Property(p => p.HeadId)
                .ValueGeneratedOnAdd();
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
            builder.HasOne(p => p.Shoora)
                .WithMany(p => p.Heads)
                .HasForeignKey(FK => FK.ShooraId)
                .HasConstraintName("FK_Head_ShooraId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Employee)
               .WithMany(p => p.Heads)
               .HasForeignKey(FK => FK.EmployeeId)
               .HasConstraintName("FK_Head_EmployeeId")
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
