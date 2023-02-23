using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class UpdateSectionVM
    {
        public int SectionId { get; set; }
        [Display(Name = "Class Name")]
        public string SectionName { get; set; }
        [Display(Name = "Grade")]
        [Required(ErrorMessage = "{0} is required")]
        public int GradeId { get; set; }
        [Display(Name ="Status")]
        public bool IsActive { get; set; }
    }
}
