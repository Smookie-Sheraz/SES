using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class SectionVM
    {
        [Display(Name = "Class Name")]
        [Required]
        public string SectionName { get; set; }
        [Display(Name = "Grade")]
        [Required(ErrorMessage = "{0} is Required!")]
        public int GradeId { get; set; }
        [Display(Name = "Class Teacher")]
        [Required(ErrorMessage = "{0} is required")]
        public int ClassTeacherId { get; set; }
        public int? IsActive { get; set; }
    }
}
