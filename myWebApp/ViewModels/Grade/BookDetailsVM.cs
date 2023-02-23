using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Grade
{
    public class BookDetailsVM
    {
        //public int? GradeId { get; set; }
        //public string BookCode { get; set; }
        //public string BookName { get; set; }
        //public string Author { get; set; }
        //public string Publisher { get; set; }
        //public DateTime PublishDate { get; set; }
        //public int? SubjectId { get; set; }
        //public bool IsActive { get; set; }
        //public int? ResourceNoteBookId { get; set; }
        //public string ResourceBook { get; set; }
        //public IFormFile ResourceBookURL { get; set; }
        public List<BookViewList> books = new List<BookViewList>();
    }
}
