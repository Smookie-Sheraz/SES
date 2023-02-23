using Microsoft.AspNetCore.Mvc;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class MonthVM
    {
        [Remote("MonthsLimit", "Validations", ErrorMessage = "Term Can't have more than 3 months")]
        public int TermId { get; set; }
        public string? Event { get; set; }
        public int? Holidays { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? SatSunday { get; set; }
        public int? AssessmentDays { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }
}
