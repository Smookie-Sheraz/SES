using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class UpdateGradeVM
    {
        public int GradeId { get; set; }
        [Display(Name = "Grade Name")]
        public string? GradeName { get; set; }
        [Display(Name = "School Section")]
        [Required(ErrorMessage = "{0} is required")]
        public int SchoolSectionId { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }

    }
}
