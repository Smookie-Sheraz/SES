//using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;

//namespace myWebApp.ViewModels.School
//{
//    public class AddGradesSection
//    {
//        [Display(Name = "Campus Code")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [StringLength(4, ErrorMessage = "{0} should not exceed 9999")]
//        public string Code { get; set; }
//        [Display(Name = "Description")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [StringLength(1000, ErrorMessage = "{0} should not exceed {1} Characters")]
//        public string Description { get; set; }
//        [Display(Name = "Grade Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("GradesIdExist", "Validations", ErrorMessage = "Grade does't exist")]
//        public int GradesId { get; set; }
//        public bool IsActive { get; set; }
//    }
//}
