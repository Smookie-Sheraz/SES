//using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;

//namespace myWebApp.ViewModels.School
//{
//    public class AddCampus
//    {

//        [Display(Name = "Campus Code")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [StringLength(4, ErrorMessage = "{0} should not exceed 9999")]
//        public string Code { get; set; }
//        [Display(Name = "School Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("SchoolIdExist", "Validations", ErrorMessage = "School does't exist")]

//        public int SchoolId { get; set; }
//        [Display(Name = "Description")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [StringLength(1000, ErrorMessage = "{0} should not exceed {1} Characters")]
//        public string Description { get; set; }
//        [Display(Name = "Location Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("LocationIdExist", "Validations", ErrorMessage = "Location does't exist")]
//        public int LocationId { get; set; }
//        public bool IsActive { get; set; }

//    }
//}
