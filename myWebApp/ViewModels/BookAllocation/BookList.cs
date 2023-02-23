using myWebApp.ViewModels.AcademicCalendar;

namespace myWebApp.ViewModels.BookAllocation
{
    public class BookList
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public bool? IsSelected { get; set; }
        public string? SubjectName { get; set; }
        public int? GradeId { get; set; }
        public int? EmployeeId { get; set; }
        public bool? preAllocation { get; set; } = false;

    }
}
