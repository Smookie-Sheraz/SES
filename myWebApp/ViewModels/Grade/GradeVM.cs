using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class GradeVM
    {
        public string? GradeCode { get; set; }
        public string? GradeName { get; set; }

        [Display(Name = "School Section")]
        [Required(ErrorMessage = "{0} is Required!")]
        public int SchoolSectionId { get; set; }
        [Display(Name = "Grade Manager")]
        [Required(ErrorMessage = "{0} is Required!")]
        public int GradeManagerId { get; set; }
        public bool? IsActive { get; set; }

    }
}
