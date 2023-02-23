using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Setups.Campus
{
    public class CampusVM
    {
        public int CampusId { get; set; }
        public string? CampusName { get; set; }
        public string? SchoolName { get; set; }
        public string? PrincipalName { get; set; }
        public string? Abbrevation { get; set; }
        public string? address { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }
        [Display(Name = "School")]
        [Required(ErrorMessage = "{0} is required")]
        public int SchoolId { get; set; }
        public List<CampusVM> campuses = new List<CampusVM>();
    }
}
