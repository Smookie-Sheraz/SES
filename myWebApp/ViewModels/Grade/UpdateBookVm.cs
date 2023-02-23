using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class UpdateBookVM
    {
        public int BookId { get; set; }
        [Display(Name = "Grade")]
        [Required(ErrorMessage = "{0} is required")]
        public int GradeId { get; set; }
        public string? BookCode { get; set; }
        public string? BookName { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? SubjectId { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }
        public bool IsWorkBook { get; set; }
        public int? ResourceNoteBookId { get; set; }
        public string? ResourceBook { get; set; }
        public string? ResourceBookURL { get; set; }
        public IFormFile? NewResourceBookURL { get; set; }
    }
}
