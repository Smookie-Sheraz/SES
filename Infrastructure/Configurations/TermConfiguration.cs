using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class TermConfiguration : IEntityTypeConfiguration<Term>
    {
        public void Configure(EntityTypeBuilder<Term> builder)
        {
            builder.ToTable("Setup_Term").HasKey(p => p.TermId)
                .HasName("PK_Term");
            builder.Property(p => p.TermId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.TermName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.StartDate)
                .HasColumnType("date");
            builder.Property(p => p.EndDate)
                .HasColumnType("date");
            builder.Property(p => p.TotalDays)
                .HasColumnType("int");
            builder.Property(p => p.TotalSchoolDays)
                .HasColumnType("int");
            builder.Property(p => p.AssesmentDays)
                .HasColumnType("int");
            builder.Property(p => p.TotalSatSun)
                .HasColumnType("int");
            builder.Property(p => p.TermHolidays)
                .HasColumnType("int");
            builder.Property(p => p.AssesmentWiseTermDays)
                .HasColumnType("int");
            builder.Property(p => p.ModifiedDate)
                .HasColumnType("date");
            builder.Property(p => p.ModifiedBy)
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
            builder.Property(p => p.AreSaturdaysOff)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            //builder.HasMany(p => p.Months)
            //    .WithOne(p => p.Term)
            //    .IsRequired()
            //    .HasForeignKey(FK => FK.TermId)
            //    .HasConstraintName("FK_Month_TermId")
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
