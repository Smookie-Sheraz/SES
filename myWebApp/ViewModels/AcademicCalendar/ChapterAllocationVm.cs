using Infrastructure.Data;
using Infrastructure.Repositories;
using myWebApp.Controllers;
using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class ChapterAllocationVM
    {
        //public int MonthId { get; set; }
        public int? TermId { get; set; }
        public int? YearId { get; set; }
        public int? UnitId { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public List<ChapterList> Chapters { get; set; }
    }
}
