using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class UpdateSubjectVM
    {
        public int SubjectId { get; set; }
        public string? SubjectCode { get; set; }
        public string? SubjectName { get; set; }
        [Display(Name = "Status")]
        public int IsActive { get; set; }
    }
}
