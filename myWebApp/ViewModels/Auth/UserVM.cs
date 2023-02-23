using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace myWebApp.ViewModels.Auth
{
    public class UserVM
    {
        public int UserId { get; set; }
        //[Display(Name = "First Name")]
        //[Required(ErrorMessage = "{0} is Required!")]
        //[AllowNull]
        public string? FName { get; set; }
        //[Display(Name = "Last Name")]
        //[Required(ErrorMessage = "{0} is Required!")]
        //[AllowNull]
        public string? LName { get; set; }
        //[Display(Name = "Fathers' Name")]
        //[Required(ErrorMessage = "{0} is Required!")]
        //[AllowNull]
        public string? FatherName { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is Required!")]
        [EmailAddress]
        [Remote("UserEmailExist", "Validations", ErrorMessage = "{0} Already Exist")]
        public string Email { get; set; }
        //[Display(Name = "UserName")]
        //[Required(ErrorMessage = "{0} is Required!")]
        //[AllowNull]
        public string? UserName { get; set; }
        //[Display(Name = "Password")]
        //[Required(ErrorMessage = "{0} is Required!")]
        //[AllowNull]
        //[PasswordPropertyText]
        public string Password { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }
        public string? UserImageUrl { get; set; }
        public IFormFile UserImage { get; set; }
    }
}
