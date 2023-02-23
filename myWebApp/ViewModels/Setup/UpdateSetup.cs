//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;

//namespace myWebApp.ViewModels.Setup
//{
//    public class UpdateSetup
//    {
//        [Display(Name = "Id")]
//        [Required(ErrorMessage = "{0} is Required!")]
//        public int Id { get; set; }
//        [Display(Name = "Code")]
//        [Required(ErrorMessage = "{0} is Required!")]
//        // [DataType(((int)DataType.Custom),ErrorMessage ="{0} Must be a Number")]
//        [StringLength(4, ErrorMessage = "{0} Should be less than {1}")]
//        public string Code { get; set; }

//        [Display(Name = "Description")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [StringLength(1000, ErrorMessage = "{0} should not exceed {1} Characters")]
//        public string Description { get; set; }
//        [Display(Name = "short Description")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [StringLength(100, ErrorMessage = "{0} should not exceed {1} Characters")]
//        public string ShortDescription { get; set; }
//        public bool IsActive { get; set; }
//        [Display(Name = "School Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        // [DataType(((int)DataType.Custom), ErrorMessage = "The School Id Must be an Number")]
//        public int SchoolId { get; set; }
//        [Display(Name = "Campus Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        // [DataType(((int)DataType.Custom), ErrorMessage = "The Campus Id Must be an Number")]
//        public int CampusId { get; set; }
//    }
//}
