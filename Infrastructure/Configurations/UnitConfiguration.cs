using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("Unit").HasKey(p => p.UnitId)
                .HasName("PK_Unit");
            builder.Property(p => p.UnitId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.UnitName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SLO)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.StartPage)
                .HasColumnType("int");
            builder.Property(p => p.EndPage)
                .HasColumnType("int");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.IsAllocated)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.Property(p => p.ModifiedDate)
                .HasColumnType("date");
            builder.Property(p => p.ModifiedBy)
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
            builder.HasOne(p => p.Book)
                 .WithMany(p => p.Units)
                 .HasForeignKey(FK => FK.BookId)
                 .HasConstraintName("FK_Unit_BookId")
                 .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
