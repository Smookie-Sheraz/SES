using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Setups
{
    public class HeadVM
    {
        public int? EmployeeId { get; set; }
        public int? ShooraId { get; set; }
        public int? SubDepartmentId { get; set; }
        public int? IsActive { get; set; }
    }
}
