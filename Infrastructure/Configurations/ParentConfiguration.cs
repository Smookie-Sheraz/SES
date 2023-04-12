using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Infrastructure.Configurations
{
    public class ParentConfiguration : IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            builder.ToTable("Parent").HasKey(p => p.ParentId)
                .HasName("PK_Parent");
            builder.Property(p => p.ParentId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.ParentType)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.LName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Gender)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ITSNumber)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Phone)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Mobile)
               .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SecondMobile)
               .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Address)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SecondAddress)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Username)
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
            builder.Property(p => p.CNIC)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.PassportNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.VisaNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.PassportValidity)
                .HasColumnType("date");
            builder.Property(p => p.VisaValidity)
                .HasColumnType("date");
            builder.Property(p => p.ResidentCardNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FamilyId)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MaritalStatus)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Occupation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Designation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Employer)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.OfficeAddress)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.OfficeNumber)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.DegreeQualification)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.EducationInstituion)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.InstituionStartDate)
                .HasColumnType("date");
            builder.Property(p => p.InstituionEndDate)
                .HasColumnType("date");
            builder.Property(p => p.VaccinationFirstDose)
                .HasColumnType("date");
            builder.Property(p => p.VaccinationSecondDose)
                .HasColumnType("date");
            builder.Property(p => p.VaccinationThirdDose)
                .HasColumnType("date");
            builder.Property(p => p.VaccinationFourthDose)
                .HasColumnType("date");
            builder.Property(p => p.MotherFName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherLName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherGender)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherITSNumber)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherPhone)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherMobile)
               .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherSecondMobile)
               .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherAddress)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherSecondAddress)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherUsername)
               .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherEmail)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherSecondEmail)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherAdmissionEmail)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherStatus)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            builder.Property(p => p.MotherCNIC)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherPassportNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherVisaNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherPassportValidity)
                .HasColumnType("date");
            builder.Property(p => p.MotherVisaValidity)
                .HasColumnType("date");
            builder.Property(p => p.MotherResidentCardNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherFamilyId)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherMaritalStatus)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherOccupation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherDesignation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherEmployer)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherOfficeAddress)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherOfficeNumber)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherDegreeQualification)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherEducationInstituion)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.MotherInstituionStartDate)
                .HasColumnType("date");
            builder.Property(p => p.MotherInstituionEndDate)
                .HasColumnType("date");
            builder.Property(p => p.MotherVaccinationFirstDose)
                .HasColumnType("date");
            builder.Property(p => p.MotherVaccinationSecondDose)
                .HasColumnType("date");
            builder.Property(p => p.MotherVaccinationThirdDose)
                .HasColumnType("date");
            builder.Property(p => p.MotherVaccinationFourthDose)
                .HasColumnType("date");
            builder.Property(p => p.FirstContactName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FirstContactEmail)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FirstContactRelation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FirstContactOfficeNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FirstContactAddress)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SecondContactName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SecondContactEmail)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SecondContactRelation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SecondContactOfficeNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.SecondContactAddress)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ThirdContactName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ThirdContactEmail)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ThirdContactRelation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ThirdContactOfficeNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.ThirdContactAddress)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FourthContactName)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FourthContactEmail)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FourthContactRelation)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FourthContactOfficeNo)
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.FourthContactAddress)
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
        }
    }
}
