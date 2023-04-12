using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Parent
    {
        public int ParentId { get; set; }
        public string? ParentType { get; set; }

        #region Father
        public string? FName { get; set; }
        public string? LName { get; set; }
        public DateTime? RegistraionDate { get; set; }
        public string? Gender { get; set; }
        public string? ITSNumber { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? SecondMobile { get; set; }
        public string? Address { get; set; }
        public string? SecondAddress { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? SecondEmail { get; set; }
        public string? AdmissionEmail { get; set; }
        public bool? Status { get; set; }
        public string? CNIC { get; set; }
        public string? PassportNo { get; set; }
        public string? VisaNo { get; set; }
        public DateTime? PassportValidity { get; set; }
        public DateTime? VisaValidity { get; set; }
        public string? ResidentCardNo { get; set; }
        public string? FamilyId { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Occupation { get; set; }
        public string? Designation { get; set; }
        public string? Employer { get; set; }
        public string? OfficeAddress { get; set; }
        public string? OfficeNumber { get; set; }
        public string? DegreeQualification { get; set; }
        public string? EducationInstituion { get; set; }
        public DateTime? InstituionStartDate { get; set; }
        public DateTime? InstituionEndDate { get; set; }
        public DateTime? VaccinationFirstDose { get; set; }
        public DateTime? VaccinationSecondDose { get; set; }
        public DateTime? VaccinationThirdDose { get; set; }
        public DateTime? VaccinationFourthDose { get; set; }

        #endregion

        #region Mother
        public string? MotherFName { get; set; }
        public string? MotherLName { get; set; }
        public DateTime? MotherRegistraionDate { get; set; }
        public string? MotherGender { get; set; }
        public string? MotherITSNumber { get; set; }
        public string? MotherPhone { get; set; }
        public string? MotherMobile { get; set; }
        public string? MotherSecondMobile { get; set; }
        public string? MotherAddress { get; set; }
        public string? MotherSecondAddress { get; set; }
        public string? MotherUsername { get; set; }
        public string? MotherEmail { get; set; }
        public string? MotherSecondEmail { get; set; }
        public string? MotherAdmissionEmail { get; set; }
        public string? MotherPassword { get; set; }
        public string? MotherPasswordRepeat { get; set; }
        public bool? MotherStatus { get; set; }
        public string? MotherCNIC { get; set; }
        public string? MotherPassportNo { get; set; }
        public string? MotherVisaNo { get; set; }
        public DateTime? MotherPassportValidity { get; set; }
        public DateTime? MotherVisaValidity { get; set; }
        public string? MotherResidentCardNo { get; set; }
        public string? MotherFamilyId { get; set; }
        public string? MotherMaritalStatus { get; set; }
        public string? MotherOccupation { get; set; }
        public string? MotherDesignation { get; set; }
        public string? MotherEmployer { get; set; }
        public string? MotherOfficeAddress { get; set; }
        public string? MotherOfficeNumber { get; set; }
        public string? MotherDegreeQualification { get; set; }
        public string? MotherEducationInstituion { get; set; }
        public DateTime? MotherInstituionStartDate { get; set; }
        public DateTime? MotherInstituionEndDate { get; set; }
        public DateTime? MotherVaccinationFirstDose { get; set; }
        public DateTime? MotherVaccinationSecondDose { get; set; }
        public DateTime? MotherVaccinationThirdDose { get; set; }
        public DateTime? MotherVaccinationFourthDose { get; set; }

        #endregion

        #region EmergencyContact
        public string? FirstContactName { get; set; }
        public string? FirstContactEmail { get; set; }
        public string? FirstContactRelation { get; set; }
        public string? FirstContactOfficeNo { get; set; }
        public string? FirstContactAddress { get; set; }
        public string? SecondContactName { get; set; }
        public string? SecondContactEmail { get; set; }
        public string? SecondContactRelation { get; set; }
        public string? SecondContactOfficeNo { get; set; }
        public string? SecondContactAddress { get; set; }
        public string? ThirdContactName { get; set; }
        public string? ThirdContactEmail { get; set; }
        public string? ThirdContactRelation { get; set; }
        public string? ThirdContactOfficeNo { get; set; }
        public string? ThirdContactAddress { get; set; }
        public string? FourthContactName { get; set; }
        public string? FourthContactEmail { get; set; }
        public string? FourthContactRelation { get; set; }
        public string? FourthContactOfficeNo { get; set; }
        public string? FourthContactAddress { get; set; }

        #endregion

        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public ICollection<Student> Students { get; set; } = null!;
        public ICollection<SchoolLatter> SchoolLatters { get; set; } = null!;
    }
}
