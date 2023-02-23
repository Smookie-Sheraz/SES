//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Entities.Models;

//namespace Infrastructure.Configurations
//{
//    public class MonthConfiguration : IEntityTypeConfiguration<Month>
//    {
//        public void Configure(EntityTypeBuilder<Month> builder)
//        {
//            builder.ToTable("Setup_Month").HasKey(p => p.MonthId)
//                .HasName("PK_Month");
//            builder.Property(p => p.MonthId)
//                .ValueGeneratedOnAdd();
//            builder.Property(p => p.StartDate)
//                .HasColumnType("date");
//            builder.Property(p => p.EndDate)
//                .HasColumnType("date");
//            builder.Property(p => p.TotalDays)
//                .HasColumnType("int");
//            builder.Property(p => p.TotalSatSundays)
//                .HasColumnType("int");
//            builder.Property(p => p.AssesmentDays)
//                .HasColumnType("int");
//            builder.Property(p => p.Event)
//                .HasColumnType("nvarchar(max)");
//            builder.Property(p => p.EventDate)
//                .HasColumnType("date");
//            builder.Property(p => p.ModifiedDate)
//                .HasColumnType("date");
//            builder.Property(p => p.ModifiedBy)
//                .HasColumnType("nvarchar(max)");
//            builder.Property(p => p.CreatedDate)
//                .HasColumnType("date");
//            builder.Property(p => p.CreatedBy)
//                .HasColumnType("nvarchar(max)");
//            builder.Property(p => p.DeletedOn)
//                .HasColumnType("nvarchar(max)");
//            builder.Property(p => p.IsDeleted)
//                .HasColumnType("bit")
//                .HasDefaultValue(false);
//        }
//    }
//}
