using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Setups
{
    public class SubDepartmentVM
    {
        ////[Display(Name = "Department Name")]
        //[Required(ErrorMessage = "{0} is Required!")]
        // //[DataType(((int)DataType.Custom),ErrorMessage ="{0} Must be a Number")]
        //[StringLength(200, ErrorMessage = "{0} Should be less than {1}")]
        public string? DepartmentName { get; set; }
        [Display(Name = "Main Department")]
        [Required(ErrorMessage = "{0} is Required")]
        public int MainDepartmentId { get; set; }
        [Display(Name = "Department Head")]
        [Required(ErrorMessage = "{0} is Required")]
        public int HeadId { get; set; }
        public List<HeadViewList> heads = new List<HeadViewList>();
        public List<SubDepartmentList> subdepartments = new List<SubDepartmentList>();
    }
}
