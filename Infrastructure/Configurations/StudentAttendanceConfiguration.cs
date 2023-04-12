using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class StudentAttendanceConfiguration : IEntityTypeConfiguration<StudentAttendance>
    {
        public void Configure(EntityTypeBuilder<StudentAttendance> builder)
        {
            builder.ToTable("StudentAttendance").HasKey(p => p.StudentAttendanceId)
                .HasName("PK_StudentAttendance");
            builder.Property(p => p.StudentAttendanceId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.AttendanceStatus)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.LateOrOnTime)
                .HasColumnType("nvarchar(max)")
                .HasDefaultValue("On Time");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.Date)
                .HasColumnType("date");
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
            builder.HasOne(x => x.Class)
                .WithMany(x => x.StudentAttendances)
                .HasForeignKey(x => x.ClassId)
                .HasConstraintName("FK_StudentAttendance_ClassId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Student)
                .WithMany(x => x.StudentAttendances)
                .HasForeignKey(x => x.StudentId)
                .HasConstraintName("FK_StudentAttendance_StudentId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
