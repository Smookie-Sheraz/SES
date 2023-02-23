using Infrastructure.Data;
using Infrastructure.Repositories;
using myWebApp.Controllers;
using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class CalendarVM
    {
        public string? CalendarName { get; set; }
        public int? totalItems { get; set; }
        public int? GradeId { get; set; }
        public int? SubjectId { get; set; }
        public int? BookId { get; set; }
        public int? YearId { get; set; }
        public int? ClassId { get; set; }
        public int? SchoolSectionId { get; set; }
        public List<YearList> years = new List<YearList>();
    }
}
