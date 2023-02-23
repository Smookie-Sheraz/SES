using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Permissions
    {
        public int PermissionId { get; set; }
        //[Display(Name = "First Name")]
        //////[Required(ErrorMessage = "{0} is Required!")]
        public string? PermissionName { get; set; }
        public string? PermissionDbName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //[Display(Name = "Modified by")]
        public string? ModifiedBy { get; set; }
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        //[Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
        public List<UserPermissions> UserPermissions { get; set; }
    }
}