using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Setups.SchoolSection
{
    public class SchoolSectionVM
    {
        public int SchoolSectionId { get; set; }
        [Display(Name = "School")]
        [Required(ErrorMessage = "{0} is required")]
        public int? SchoolId { get; set; }
        [Display(Name = "Assistant Coordinator")]
        public int ACId { get; set; }
        public string? SectionName { get; set; }
        public string? CampusName { get; set; }
        public string? Abbrevation { get; set; }
        public string? address { get; set; }
        public string? PhoneNo { get; set; }
        [Display(Name ="Status")]
        public bool IsActive { get; set; }
        public string? Email { get; set; }
        public string? SectionHead { get; set; }
        [Display(Name = "Campus")]
        [Required(ErrorMessage = "{0} is required")]
        public int? CampusId { get; set; }
        public List<SchoolSectionVM> SchoolSections = new List<SchoolSectionVM>();
    }
}
