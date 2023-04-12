using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class LeaveApplicationConfiguration : IEntityTypeConfiguration<LeaveApplication>
    {
        public void Configure(EntityTypeBuilder<LeaveApplication> builder)
        {
            builder.ToTable("LeaveApplication").HasKey(p => p.LeaveApplicationId)
                .HasName("PK_LeaveApplication");
            builder.Property(p => p.LeaveApplicationId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.Reason)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ApplicationStatus)
                .HasColumnType("nvarchar(max)")
                .HasDefaultValue("Pending");
            builder.Property(p => p.StartDate)
                .HasColumnType("date");
            builder.Property(p => p.EndDate)
                .HasColumnType("date");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.ModifiedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ModifiedDate)
                 .HasColumnType("date");
            builder.Property(p => p.CreatedDate)
                .HasColumnType("date");
            builder.Property(p => p.CreatedBy)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.DeletedOn)
                .HasColumnType("date");
            builder.Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.HasOne(x => x.Student)
                .WithMany(x => x.LeaveApplications)
                .HasForeignKey(x => x.StudentId)
                .HasConstraintName("FK_LeaveApplication_StudentId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
