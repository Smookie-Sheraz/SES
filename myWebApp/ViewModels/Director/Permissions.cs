using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Director
{
    public class Permissions
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool isSelected { get; set; }
    }
}
