using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Setups
{
    public class UpdateSubDepartmentVM
    {
        public int SubDepartmentId { get; set; }
        //[Display(Name = "Department Name")]
        //[Required(ErrorMessage = "{0} is Required!")]
        // //[DataType(((int)DataType.Custom),ErrorMessage ="{0} Must be a Number")]
        //[StringLength(200, ErrorMessage = "{0} Should be less than {1}")]
        public string? DepartmentName { get; set; }
        [Display(Name = "Department")]
        [Required(ErrorMessage = "{0} is Required")]
        public int MainDepartmentId { get; set; }
        [Display(Name = "Department Head")]
        [Required(ErrorMessage = "{0} is Required")]
        public int HeadId { get; set; }
        [Display(Name ="Status")]
        public bool IsActive { get; set; }
    }
}
