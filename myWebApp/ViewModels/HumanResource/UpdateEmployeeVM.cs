using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.HumanResource
{
    public class UpdateEmployeeVM
    {
        public int EmployeeId { get; set; }
        //[Display(Name = "Employee Code")]
        //[Required(ErrorMessage = "{0} is required")]
        //[StringLength(10, ErrorMessage = "{0} can't exceed {1} characters")]
        public string? EmployeeCode { get; set; }
        //[Display(Name = "First Name")]
        //[Required(ErrorMessage = "{0} is required")]
        //[StringLength(100, ErrorMessage = "{0} can't exceed {1} characters")]
        public string? FName { get; set; }
        //[Display(Name = "Last Name")]
        //[Required(ErrorMessage = "{0} is required")]
        //[StringLength(100, ErrorMessage = "{0} can't exceed {1} characters")]
        public string? LName { get; set; }
        //[Display(Name = "Father's Name")]
        //[Required(ErrorMessage = "{0} is required")]
        //[StringLength(100, ErrorMessage = "{0} can't exceed {1} characters")]
        public string? FatherName { get; set; }
        public string? Role { get; set; }
        public string? UserImageUrl { get; set; }
        public IFormFile? UserImage { get; set; }
        //[Display(Name = "Spouse Name")]
        //[StringLength(100, ErrorMessage = "{0} can't exceed {1} characters")]
        public string? SpouseName { get; set; }
        //[Display(Name = "Gender")]
        //[Required(ErrorMessage = "{0} is required")]
        public int? GenderId { get; set; }
        [Display(Name ="Status")]
        public bool IsActive { get; set; }
        //[Display(Name = "Marital Status")]
        //[Required(ErrorMessage = "{0} is required")]
        //[StringLength(11, ErrorMessage = "{0} can't exceed {1} characters")]
        public string? MaritalStatus { get; set; }
        //[Display(Name = "Mobile No")]
        //[Required(ErrorMessage = "{0} is required")]
        //[StringLength(11, ErrorMessage = "{0} should not exceed {1) characters")]
        public string? Mobile { get; set; }
        //[Display(Name = "Date of Birth")]
        //[Required(ErrorMessage = "{0} is required")]
        //[DataType(DataType.Date)]
        public DateTime? DOB { get; set; }
        //[Display(Name = "CNIC No")]
        //[Required(ErrorMessage = "{0} is required")]
        //[StringLength(15, ErrorMessage = "{0} should not exceed {1} characters")]
        [Display(Name = "CNIC No")]
        [Required(ErrorMessage = "{0} is Required!")]
        [StringLength(15, ErrorMessage = "{0} can't exceed {1} characters")]
        [Remote("UpdateEmployeeCNICExist", "Validations",AdditionalFields ="EmployeeId", ErrorMessage = "{0} Already Exist")]
        public string CNIC { get; set; }
        //[Display(Name = "CNIC Issue Date")]
        //[Required(ErrorMessage = "{0} is required")]
        //[DataType(DataType.Date)]
        public DateTime? CNICIssueDate { get; set; }
        //[Display(Name = "CNIC Expiry Date")]
        //[Required(ErrorMessage = "{0} is required")]
        //[DataType(DataType.Date)]
        public DateTime? CNICExpiryDate { get; set; }
        //[Display(Name = "Email Address")]
        //[Required(ErrorMessage = "{0} is required")]
        //[EmailAddress]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is Required!")]
        [Remote("EmployeeUpdateEmailAlreadyExist", "Validations", AdditionalFields = "EmployeeId", ErrorMessage = "{0} Already Exist")]
        public string Email { get; set; }
        //[Display(Name = "Address")]
        //[Required(ErrorMessage = "{0} is required")]
        public string? Address { get; set; }
        //[Display(Name = "Joining Date")]
        //[Required(ErrorMessage = "{0} is required")]
        //[DataType(DataType.Date)]
        public DateTime? JoiningDate { get; set; }
        //[Display(Name = "Probation Period")]
        //[Required(ErrorMessage = "{0} is required")]
        public string? FieldOfSpecialization { get; set; }
        public int? ProbationPeriod { get; set; }
        //[Display(Name = "End of Probation")]
        //[Required(ErrorMessage = "{0} is required")]
        public DateTime? EndofProbationDate { get; set; }
        public DateTime? StartofPeriodDate { get; set; }
        public int? Period { get; set; }
        public DateTime? EndofPeriodDate { get; set; }
        public DateTime? StartofProbationDate { get; set; }
        //[Display(Name = "Confirmation Date")]
        //[Required(ErrorMessage = "{0} is required")]
        public DateTime? ConfirmationDate { get; set; }
        //[Display(Name = "Confirmation Delay")]
        //[Required(ErrorMessage = "{0} is required")]
        public DateTime? ConfirmationDelay { get; set; }
        public string? ConfirDelayReason { get; set; }
        public int? DesignationId { get; set; }
        public int? RoleId { get; set; }
        public string? DesignationName { get; set; }
        public int? SubDepartmentId { get; set; }
        public string? SubDepartmentName { get; set; }
        [Display(Name = "Department")]
        [Required(ErrorMessage = "{0} is required")]
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        [Display(Name = "School")]
        [Required(ErrorMessage = "{0} is required")]
        public int SchoolId { get; set; }
        [Display(Name = "Campus")]
        [Required(ErrorMessage = "{0} is required")]
        public int CampusId { get; set; }
        [Display(Name = "School Section")]
        [Required(ErrorMessage = "{0} is required")]
        public int SchoolSectionId { get; set; }
        public int? EmployeeType { get; set; }
        public string? CampusName { get; set; }
        public string? SchoolName { get; set; }
        public string? GenderName { get; set; }
        public string? SchoolSectionName { get; set; }
    }
}