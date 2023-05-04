using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? RegistraionNo { get; set; }
        public string? StudentRegistraionNo { get; set; }
        public string? CandidateNo { get; set; }
        public string? CandidateName { get; set; }
        public string? Gender { get; set; }
        public string? PalceOfBirth { get; set; }
        public bool? OnlyRegisteredNoAdmitted { get; set; }
        public string? ITSNumber { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? WhastAppNo { get; set; }
        public string? ModeOfTransport { get; set; }
        public string? ToSchool { get; set; }
        public string? FromSchool { get; set; }
        public string? Address { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? SecondEmail { get; set; }
        public string? AdmissionEmail { get; set; }
        //public string? Password { get; set; }
        //public string? PasswordRepeat { get; set; }
        public bool? Status { get; set; }
        public string? RollNo { get; set; }
        public string? CNIC { get; set; }
        public string? PassportNo { get; set; }
        public DateTime? PassportValidity { get; set; }
        public DateTime? VisaValidity { get; set; }
        public string? ResidentCardNo { get; set; }
        public string? ElectricityBillNo { get; set; }
        public string? WaterBillNo { get; set; }
        public string? AdmissionTestResult { get; set; }
        public string? SecondAddress { get; set; }
        public string? City { get; set; }
        public string? Category { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public string? Cast { get; set; }
        public string? BoardOrEnrollmentNo { get; set; }
        public string? BoardOrUniversityName { get; set; }
        public string? SeatNo { get; set; }
        public string? AdmittedSession { get; set; }
        public string? AdmittedClassOrSection { get; set; }
        public int? BoardMarksObtained { get; set; }
        public string? BloodGroup { get; set; }
        public string? Religion { get; set; }
        public string? CountryOfBirth { get; set; }
        public bool? IgnoreFeeDefaulterRestrictLogin { get; set; }
        public int? ScholarchipAmount { get; set; }
        public int? TaxPercentage { get; set; }
        public string? Nationality { get; set; }
        public string? NationalityType { get; set; }
        public string? Allergies { get; set; }
        public string? LanguageSpken { get; set; }
        public string? ExtraCurricularActivities { get; set; }
        public bool? LoginFeeDefualterRestrictLogin { get; set; }
        public bool? RestrictLogin { get; set; }
        public string? Picture { get; set; }
        public DateTime? VaccinationFirstDose { get; set; }
        public DateTime? VaccinationSecondDose { get; set; }
        public DateTime? VaccinationThirdDose { get; set; }
        public DateTime? VaccinationFourthDose { get; set; }
        public Section? Class { get; set; }
        public int? ClassId { get; set; }
        public Parent? Parent { get; set; }
        public int? ParentId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public ICollection<StudentAttendance> StudentAttendances { get; set; } = null!;
        public ICollection<LeaveApplication> LeaveApplications { get; set; } = null!;
        public ICollection<SchoolLatter> SchoolLatters { get; set; } = null!;

    }
}
