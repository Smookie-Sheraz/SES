using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Teacher
{
    public class TeachingMethodologyVM
    {
        public int? TeachingMethodologyId { get; set; }
        public string? TMethodName { get; set; }
        [Display(Name ="Status")]
        public bool IsActive { get; set; }
    }
}
