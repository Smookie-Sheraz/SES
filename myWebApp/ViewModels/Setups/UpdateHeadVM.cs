using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Setups
{
    public class UpdateHeadVM
    {
        public int Headid { get; set; }
        //[Display(Name = "Employee Name")]
        public int? EmployeeId { get; set; }
        //[Display(Name = "Shoora Name")]
        public int? ShooraId { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }
    }
}
