using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Grade
{
    public class BookVM
    {
        [Display(Name = "Grade")]
        [Required(ErrorMessage = "{0} is Required!")]
        public int GradeId { get; set; }
        public string? BookCode { get; set; }
        public string? BookName { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public DateTime? PublishDate { get; set; }
        [Display(Name = "Subject")]
        [Required(ErrorMessage ="{0} is Required!")]
        public int SubjectId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsWorkBook { get; set; }
        public int? ResourceNoteBookId { get; set; }
        public string? ResourceBook { get; set; }
        public IFormFile? ResourceBookURL { get; set; }
        public List<BookViewList> books = new List<BookViewList>();
    }
}
