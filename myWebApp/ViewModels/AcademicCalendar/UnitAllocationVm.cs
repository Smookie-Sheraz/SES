using Infrastructure.Data;
using Infrastructure.Repositories;
using myWebApp.Controllers;
using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class UnitAllocationVM
    {
        public int? TermId { get; set; }
        public int? YearId { get; set; }
        public int? BookId { get; set; }
        public int? SectionId { get; set; }
        public string? SearchValues { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public List<UnitList> Units { get; set; }
    }
}
