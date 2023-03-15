﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class ACPlanApprovalConfiguration : IEntityTypeConfiguration<ACPlanApproval>
    {
        public void Configure(EntityTypeBuilder<ACPlanApproval> builder)
        {
            builder.ToTable("ACPlanApproval").HasKey(p => p.ACPlanApprovalId)
                .HasName("PK_ACPlanApproval");
            builder.Property(p => p.ACPlanApprovalId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.Status)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Comments)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.CTApproval)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.Property(p => p.GMApproval)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.Property(p => p.ACApproval)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.Property(p => p.DCApproval)
                .HasColumnType("bit")
                .HasDefaultValue(false);
            builder.Property(p => p.DAApproval)
                .HasColumnType("bit")
                .HasDefaultValue(false);
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
            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.HasOne(p => p.Plan)
                .WithMany(p => p.ACPlans)
                .HasForeignKey(p => p.PlanId)
                .HasConstraintName("FK_ACPlanApproval_STPlanId")
                .OnDelete(DeleteBehavior.SetNull);
            //builder.HasOne(p => p.Year)
            //    .WithMany(p => p.STPlans)
            //    .HasForeignKey(p => p.YearId)
            //    .HasConstraintName("FK_STPlanApproval_YearId")
            //    .OnDelete(DeleteBehavior.SetNull);
            //builder.HasOne(p => p.Term)
            //    .WithMany(p => p.STPlans)
            //    .HasForeignKey(p => p.TermId)
            //    .HasConstraintName("FK_STPlanApproval_TermId")
            //    .OnDelete(DeleteBehavior.SetNull);
            //builder.HasOne(p => p.Book)
            //    .WithMany(p => p.STPlans)
            //    .HasForeignKey(p => p.BookId)
            //    .HasConstraintName("FK_STPlanApproval_BookId")
            //    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
