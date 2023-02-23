using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Setups
{
    public class GenderVM
    {
        [Display(Name = "Gender")]
        [Required(ErrorMessage ="{0} is Required!")]
        public string Gender { get; set; }
    }
}
