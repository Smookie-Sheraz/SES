using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class UpdateChapterVM
    {
        [Display(Name = "Chapter Name")]
        public string? ChapterName { get; set; }
        public string? SLO { get; set; }
        public int ChapterId { get; set; }
        [Display(Name = "Unit")]
        [Required(ErrorMessage = "{0} is required")]
        public int UnitId { get; set; }
        [Display(Name = "Start Page")]
        public int StartPage { get; set; }
        [Display(Name = "End Page")]
        public int EndPage { get; set; }
        public int? MinPage { get; set; }
        public int? MaxPage { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }

    }
}
