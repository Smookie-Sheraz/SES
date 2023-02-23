using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace myWebApp.ViewModels.Auth
{
    public class LoginVM
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is Required!")]
        [Remote("UserNameExist", "Validations", ErrorMessage ="{0} Doesn't Exist")]
        public string Email { get; set; } = null!;
        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is Required!")]
        //[DataType(DataType.Password)]
        //[PasswordPropertyText]
        public string Password { get; set; } = null!;
        public bool? KeepLoggedIn { get; set; } = false!;
    }
}
