using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class UserPermissions
    {
        public int UserPermissionId { get; set; }
        //[Display(Name = "First Name")]
        //////[Required(ErrorMessage = "{0} is Required!")]
        public Roles? Role { get; set; }
        public int? RoleId { get; set; }
        public Permissions? Permission { get; set; }
        public int? PermissionId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //[Display(Name = "Modified by")]
        public string? ModifiedBy { get; set; }
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        //[Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
    }
}