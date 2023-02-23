using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class UnitVM
    {
        public string? UnitName { get; set; }
        [Display(Name = "Book")]
        [Required(ErrorMessage = "{0} is required")]
        public int BookId { get; set; }
        public int? UnitId { get; set; }
        public string? SLO { get; set; }
        [Display(Name ="Start Page")]
        public int StartPage { get; set; }
        [Display(Name ="End Page")]
        public int EndPage { get; set; }
        [Display(Name ="Status")]
		public bool IsActive { get; set; }
        public List<ChapterVM> Chapters = new List<ChapterVM>();
    }
}
