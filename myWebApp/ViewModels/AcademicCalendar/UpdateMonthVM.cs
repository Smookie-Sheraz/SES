using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class UpdateMonthVM
    {
        public int MonthId { get; set; }
        [Display(Name = "Term")]
        [Required(ErrorMessage = "{0} is required")]
        public int TermId { get; set; }
        public string Event { get; set; }
        public int Holidays { get; set; }
        public int? AssesmentDays { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
