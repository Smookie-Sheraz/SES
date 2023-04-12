using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Student").HasKey(p => p.StudentId)
                .HasName("PK_Student");
            builder.Property(p => p.StudentId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.FName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.LName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.DOB)
                .HasColumnType("date");
            builder.Property(p => p.RegistrationDate)
                .HasColumnType("date");
            builder.Property(p => p.RegistraionNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.StudentRegistraionNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.CandidateNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.CandidateName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.PalceOfBirth)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.OnlyRegisteredNoAdmitted)
                .HasColumnType("bit");
            builder.Property(p => p.CNIC)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ITSNumber)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Phone)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Mobile)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ModeOfTransport)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FromSchool)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ToSchool)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Address)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Email)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SecondEmail)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.AdmissionEmail)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Status)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.RollNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.PassportNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.PassportValidity)
                .HasColumnType("date");
            builder.Property(p => p.VisaValidity)
                .HasColumnType("date");
            builder.Property(p => p.ResidentCardNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ElectricityBillNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.WaterBillNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.AdmissionTestResult)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SecondAddress)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.City)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Category)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.EmergencyContactName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.EmergencyContactNumber)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Cast)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.BoardOrEnrollmentNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.BoardOrUniversityName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SeatNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.AdmittedSession)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.AdmittedClassOrSection)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.BoardMarksObtained)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.BloodGroup)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Religion)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.CountryOfBirth)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.IgnoreFeeDefaulterRestrictLogin)
                .HasColumnType("bit");
            builder.Property(p => p.ScholarchipAmount)
                .HasColumnType("int");
            builder.Property(p => p.TaxPercentage)
                .HasColumnType("int");
            builder.Property(p => p.Nationality)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.NationalityType)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Allergies)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.LanguageSpken)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ExtraCurricularActivities)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Picture)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.LoginFeeDefualterRestrictLogin)
                .HasColumnType("bit");
            builder.Property(p => p.RestrictLogin)
                .HasColumnType("bit");
            builder.Property(p => p.VaccinationFirstDose)
                .HasColumnType("date");
            builder.Property(p => p.VaccinationSecondDose)
                .HasColumnType("date");
            builder.Property(p => p.VaccinationThirdDose)
                .HasColumnType("date");
            builder.Property(p => p.VaccinationFourthDose)
                .HasColumnType("date");
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
            builder.HasOne(x => x.Class)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.ClassId)
                .HasConstraintName("FK_Student_ClassId")
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.ParentId)
                .HasConstraintName("FK_Student_ParentId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
