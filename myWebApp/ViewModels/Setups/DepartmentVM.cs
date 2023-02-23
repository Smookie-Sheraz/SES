using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Setups
{
    public class DepartmentVM
    {
        ////[Display(Name = "Department Name")]
       // //[Required(ErrorMessage = "{0} is Required!")]
        // //[DataType(((int)DataType.Custom),ErrorMessage ="{0} Must be a Number")]
       // //[StringLength(200, ErrorMessage = "{0} Should be less than {1}")]
        public string? DepartmentName { get; set; }
        [Display(Name = "Department Head")]
        [Required(ErrorMessage = "{0} is Required")]
        public int DepartmentHeadId { get; set; }
        //[Display(Name = "Description")]
        //[Required(ErrorMessage = "{0} is Required")]
        //[StringLength(1000, ErrorMessage = "{0} should not exceed {1} Characters")]
        public string? Description { get; set; }
        //[Display(Name = "short Description")]
        //[Required(ErrorMessage = "{0} is Required")]
        //[StringLength(500, ErrorMessage = "{0} should not exceed {1} Characters")]
        public string? ShortDescription { get; set; }
        public int? IsActive { get; set; }

        public List<DepartmentList> departmentLists = new List<DepartmentList>();
    }
}
