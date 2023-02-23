using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Setups
{
    public class HeadViewList
    {
        public int? HeadId { get; set; }
        public string? EmployeeName { get; set; }
        public string? ShooraName { get; set; }
        public bool? IsActive { get; set; }
    }
}
