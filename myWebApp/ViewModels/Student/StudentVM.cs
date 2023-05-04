using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Student
{
    public class StudentVM
    {
        public int StudentId { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} is Required!")]
        public string FName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "{0} is Required!")]
        public string LName { get; set; }
        public string? RegistraionNo { get; set; }
        public string? StudentRegistraionNo { get; set; }
        public string? CandidateNo { get; set; }
        public string? CandidateName { get; set; }
        [Display(Name = "Gender")]
        [Required(ErrorMessage = "{0} is Required!")]
        public string Gender { get; set; }
        public string? PalceOfBirth { get; set; }
        //[Display(Name = "Date of Birth")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public DateTime? DOB { get; set; }
        public bool OnlyRegisteredNoAdmitted { get; set; }
        public string? ITSNumber { get; set; }
        //[Display(Name = "Phone No ")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? Phone { get; set; }
        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "{0} is Required!")]
        public int ClassId { get; set; }
        public int? ParentId { get; set; }
        public IFormFile? Picture { get; set; }
        public string? PictureURL { get; set; }
        [Display(Name = "WhatsApp No")]
        [Required(ErrorMessage = "{0} is Required!")]
        [StringLength(11,ErrorMessage ="WhatsApp No Should Must be 11 Digits Long!",MinimumLength = 11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Please Enter a Valid WhatsApp No!")]
        public string? WhatsAppNo { get; set; }
        [Display(Name = "Mobile No")]
        [Required(ErrorMessage = "{0} is Required!")]
        [StringLength(11, ErrorMessage = "Mobile No Should Must be 11 Digits Long!", MinimumLength = 11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Please Enter a Valid Mobile No!")]
        public string? Mobile { get; set; }
        //[Display(Name = "Mode of Transport")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? ModeOfTransport { get; set; }
        public string? ToSchool { get; set; }
        public string? FromSchool { get; set; }
        //[Display(Name = "Address")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? Address { get; set; }
        //[Display(Name = "Username")]
        //[Required(ErrorMessage = "{0} is Required!")]
        //public string Username { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} is Required!")]
        [Remote("EmailAlreadyExist", "Validations", ErrorMessage = "{0} Already Exist")]
        public string Email { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} is Required!")]
        [Remote("StudentUpdateEmailAlreadyExist", "Validations", AdditionalFields = "StudentId", ErrorMessage = "{0} Already Exist")]
        public string UpdateEmail { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? SecondEmail { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? AdmissionEmail { get; set; }
        //[Display(Name = "Password")]
        //[Required(ErrorMessage = "{0} is Required!")]
        //public string Password { get; set; }
        //[Display(Name = "Repeat Password")]
        //[Compare("Password",ErrorMessage = "Passowrd Doesn't Match!")]
        //[Required(ErrorMessage = "{0} is Required!")]
        //public string PasswordRepeat { get; set; }
        public bool Status { get; set; }
        //[Display(Name = "Roll No")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? RollNo { get; set; }
        [Display(Name = "CNIC No")]
        [Required(ErrorMessage = "{0} is Required!")]
        [Remote("CNICExist", "Validations", ErrorMessage = "{0} Already Exist")]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "{0} Must be {1} Characters Long!")]
        public string CNIC { get; set; }
        [Display(Name = "CNIC No")]
        [Required(ErrorMessage = "{0} is Required!")]
        [Remote("UpdateStudentCNICExist", "Validations", AdditionalFields = "StudentId", ErrorMessage = "{0} Already Exist")]
        [StringLength(15, ErrorMessage = "{0} Can't Be Less Than {1} Characters!")]
        public string UpdateCNIC { get; set; }
        public string? PassportNo { get; set; }
        public DateTime? PassportValidity { get; set; }
        public DateTime? VisaValidity { get; set; }
        public string? ResidentCardNo { get; set; }
        public string? ElectricityBillNo { get; set; }
        public string? WaterBillNo { get; set; }
        public string? AdmissionTestResult { get; set; }
        public string? SecondAddress { get; set; }
        //[Display(Name = "City Name")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? City { get; set; }
        //[Display(Name = "Category")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? Category { get; set; }
        //[Display(Name = "Emergency Contact Name")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? EmergencyContactName { get; set; }
        [Display(Name = "Emergency Contact No")]
        [Required(ErrorMessage = "{0} is Required!")]
        [StringLength(11, ErrorMessage = "Emergency Contact No Should Must be 11 Digits Long!", MinimumLength = 11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Please Enter a Valid Emergency Contact No!")]
        public string? EmergencyContactNumber { get; set; }
        //[Display(Name = "Category")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? Cast { get; set; }
        public string? BoardOrEnrollmentNo { get; set; }
        public string? BoardOrUniversityName { get; set; }
        public string? SeatNo { get; set; }
        public string? AdmittedSession { get; set; }
        public string? AdmittedClassorSection { get; set; }
        public int? BoardMarksObtained { get; set; }
        //[Display(Name = "Blood Group")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? BloodGroup { get; set; }
        //[Display(Name = "Religion")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? Religion { get; set; }
        //[Display(Name = "Country of Birth")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? CountryOfBirth { get; set; }
        public bool IgnoreFeeDefaulterRestrictLogin { get; set; }
        public int? ScholarchipAmount { get; set; }
        public int? TaxPercentage { get; set; }
        public string? Nationality { get; set; }
        public string? NationalityType { get; set; }
        public string? Allergies { get; set; }
        //[Display(Name = "Language Spoken")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? LanguageSpken { get; set; }
        public string? ExtraCurricularActivities { get; set; }
        public bool LoginFeeDefualterRestrictLogin { get; set; }
        public bool RestrictLogin { get; set; }
        public DateTime? VaccinationFirstDose { get; set; }
        public DateTime? VaccinationSecondDose { get; set; }
        public DateTime? VaccinationThirdDose { get; set; }
        public DateTime? VaccinationFourthDose { get; set; }

        #region Parent

        [Display(Name = "Parent Type")]
        [Required(ErrorMessage = "{0} is Required!")]
        public string ParentType { get; set; }
        [Display(Name = "Parent First Name")]
        [Required(ErrorMessage = "{0} is Required!")]
        public string ParentFName { get; set; }
        [Display(Name = "Parent Last Name")]
        [Required(ErrorMessage = "{0} is Required!")]
        public string ParentLName { get; set; }
        public DateTime? ParentRegistraionDate { get; set; }
        [Display(Name = "Parent CNIC")]
        [Required(ErrorMessage = "{0} is Required!")]
        [StringLength(maximumLength: 15, ErrorMessage = "{0} Can't Be Less Than {1} Characters!", MinimumLength = 15)]
        public string? ParentCNIC { get; set; }
        //[Display(Name = "Parent Mobile No")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? ParentMobile { get; set; }
        //[Display(Name = "Parent Occupation")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? ParentOccupation { get; set; }
        //[Display(Name = "Parent Designation")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? ParentDesignation { get; set; }
        //[Display(Name = "Parent Employer")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? ParentEmployer { get; set; }
        //[Display(Name = "Parent Department")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? ParentDepartment { get; set; }
        //[Display(Name = "Parent Office Address")]
        //[Required(ErrorMessage = "{0} is Required!")]
        public string? ParentOfficeAddress { get; set; }
        [Display(Name = "Parent Email")]
        [Required(ErrorMessage = "{0} is Required!")]
        public string ParentEmail { get; set; }
        //[Display(Name = "Parent Password")]
        //[Required(ErrorMessage = "{0} is Required!")]
        //public string ParentPassword { get; set; }
        //[Display(Name = "Parent Username")]
        //[Required(ErrorMessage = "{0} is Required!")]
        //public string ParentUsername { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} is Required!")]
        [Remote("ParentUpdateEmailAlreadyExist", "Validations", AdditionalFields = "ParentId", ErrorMessage = "{0} Already Exist")]
        public string ParentUpdateEmail { get; set; }

        #endregion
    }
}
