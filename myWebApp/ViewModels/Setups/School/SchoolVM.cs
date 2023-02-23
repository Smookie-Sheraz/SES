using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Setups.School
{
    public class SchoolVM
    {
        public int SchoolId { get; set; }
        [Display(Name ="School Name")]
        public string SchoolName { get; set; }
        public string? CEOName { get; set; }
        public int? RegistrationNo { get; set; }
        public string? Abbrevation { get; set; }
        public string? address { get; set; }
        [Display(Name = "Phone No")]
        [StringLength(11,ErrorMessage ="{0} Can't Exceed {1} Characters!")]
        public string PhoneNo { get; set; }
        public string? Email { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }
    }
}
