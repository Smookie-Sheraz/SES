using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class PlanApprovalConfiguration : IEntityTypeConfiguration<PlanApproval>
    {
        public void Configure(EntityTypeBuilder<PlanApproval> builder)
        {
            builder.ToTable("PlanApproval").HasKey(p => p.PlanApprovalId)
                .HasName("PK_PlanApproval");
            builder.Property(p => p.PlanApprovalId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.Status)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Remarks)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ApprovingPerson)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ApprovingPersonName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.HasOne(x => x.BookPlan)
                .WithMany(x => x.Plans)
                .HasForeignKey(x => x.BookId)
                .HasConstraintName("FK_PlanApproval_BookId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Plan)
                .WithMany(x => x.PlanApprovals)
                .HasForeignKey(x => x.PlanId)
                .HasConstraintName("FK_PlanApproval_PlanId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
