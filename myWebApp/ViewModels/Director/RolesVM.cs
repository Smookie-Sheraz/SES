using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Director
{
    public class RolesVM
    {
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }
    }
}
