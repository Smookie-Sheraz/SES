using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using myWebApp.Controllers;
using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class HolidaysVM
    {
        public int? TermId { get; set; }
        public int? YearId { get; set; }
        public string? HolidayName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public int? NoOfHolidays { get; set; }
        public bool IsSchoolOff { get; set; }
        public List<Holiday>? Holidays { get; set; }
    }
}
