using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class User
    {
        public int UserId { get; set; }
        //[Display(Name = "First Name")]
        //////[Required(ErrorMessage = "{0} is Required!")]
        public string? FName { get; set; }
        //[Display(Name = "Last Name")]
        ////[Required(ErrorMessage = "{0} is Required!")]
        ////[AllowNull]
        public string? LName { get; set; }
        //[Display(Name = "Fathers' Name")]
        ////[Required(ErrorMessage = "{0} is Required!")]
        ////[AllowNull]
        public string? FatherName { get; set; }
        //[Display(Name = "Email Address")]
        ////[Required(ErrorMessage = "{0} is Required!")]
        //[EmailAddress]
        ////[AllowNull]
        public string? Email { get; set; }
        //[Display(Name = "UserName")]
        ////[Required(ErrorMessage = "{0} is Required!")]
        ////[AllowNull]
        public string? UserName { get; set; }
        //[Display(Name = "Password")]
        ////[Required(ErrorMessage = "{0} is Required!")]
        ////[AllowNull]
        //[PasswordPropertyText]
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? UserImageURL { get; set; }
        public bool IsActive { get; set; }
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        //[Display(Name = "Created by")]
        public bool? IsDeleted { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
    }
}