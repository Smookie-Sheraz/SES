using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Setups
{
    public class UpdateDesignationVM
    {
        public int DesigationId { get; set; }
        [Display(Name = "Designation Name")]
        [Required(ErrorMessage = "{0} is Required!")]
        public string DesignationName { get; set; }
        [Display(Name ="Status")]
        public bool IsActive { get; set; }
    }
}
