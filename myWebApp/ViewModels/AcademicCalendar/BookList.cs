namespace myWebApp.ViewModels.AcademicCalendar
{
    public class BookListVM
    {
        public int BookId { get; set; }
        public List<UnitList> units = new List<UnitList>();
        public string? BookName { get; set; }
        public bool? IsSelected { get; set; }
        public string? SubjectName { get; set; }
        public int? GradeId { get; set; }
        public int? EmployeeId { get; set; }
        public bool? preAllocation { get; set; } = false;

    }
}
