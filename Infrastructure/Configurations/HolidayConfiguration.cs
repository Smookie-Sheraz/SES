using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
    {
        public void Configure(EntityTypeBuilder<Holiday> builder)
        {
            builder.ToTable("Holidays").HasKey(p => p.HolidayId)
                .HasName("PK_Holiday");
            builder.Property(p => p.HolidayId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.HolidayName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.IsSchoolOff)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.Property(p => p.StartDate)
                .HasColumnType("date");
            builder.Property(p => p.NoOfHolidays)
                .HasColumnType("int");
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
            builder.HasOne(p => p.Term)
                .WithMany(p => p.Holidays)
                .HasForeignKey(FK => FK.TermId)
                .HasConstraintName("FK_Holidays_TermId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
