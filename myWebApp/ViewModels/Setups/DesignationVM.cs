using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Setups
{
    public class DesignationVM
    {
        [Display(Name = "Designation Name")]
        [Required(ErrorMessage = "{0} is Required!")]
        public string DesignationName { get; set; }
        public int? IsActive { get; set; }
    }
}
