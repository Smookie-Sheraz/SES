using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.HumanResource
{
    public class ShooraVM
    {
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
        //[Display(Name = "Spouse Name")]
        //[StringLength(100, ErrorMessage = "{0} can't exceed {1} characters")]
        public string? SpouseName { get; set; }
        //[Display(Name = "Gender")]
        //[Required(ErrorMessage = "{0} is required")]
        public int? GenderId { get; set; }
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
        [Display(Name = "CNIC No")]
        [Required(ErrorMessage = "{0} is Required!")]
        [StringLength(15, ErrorMessage = "{0} can't exceed {1} characters")]
        [Remote("ShooraCNICAlreadyExist", "Validations", ErrorMessage = "{0} Already Exist")]
        public string CNICNo { get; set; }
        //[Display(Name = "CNIC Issue Date")]
        //[Required(ErrorMessage = "{0} is required")]
        //[DataType(DataType.Date)]
        public DateTime? CNICIssueDate { get; set; }
        //[Display(Name = "CNIC Expiry Date")]
        //[Required(ErrorMessage = "{0} is required")]
        //[DataType(DataType.Date)]
        public DateTime? CNICExpiryDate { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is Required!")]
        [EmailAddress]
        [Remote("ShooraEmailAlreadyExist", "Validations", ErrorMessage = "{0} Already Exist")]
        public string Email { get; set; }
        //[Display(Name = "Address")]
        //[Required(ErrorMessage = "{0} is required")]
        public string? Address { get; set; }
        //[Display(Name = "Joining Date")]
        //[Required(ErrorMessage = "{0} is required")]
        //[DataType(DataType.Date)]
        public DateTime? JoiningDate { get; set; }
        //[Display(Name = "Address")]
        //[Required(ErrorMessage = "{0} is required")]
        public string? City { get; set; }
        public int? SchoolId { get; set; }
        public int? CampusId { get; set; }
        
    }
}
