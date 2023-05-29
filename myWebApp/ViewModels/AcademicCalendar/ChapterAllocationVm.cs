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
        public int? SectionId { get; set; }
        public int? BookId { get; set; }
        [Display(Name = "Unit")]
        [Required(ErrorMessage = "{0} is Required!")]
        public int UnitId { get; set; }
        [Display(Name = "Plan")]
        [Required(ErrorMessage = "{0} is Required!")]
        public int PlanId { get; set; }
        public bool AreSaturdaysOff { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public List<ChapterList> Chapters { get; set; }
    }
}
