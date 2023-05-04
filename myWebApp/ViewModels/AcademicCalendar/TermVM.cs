using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class TermVM
    {
        [Display(Name = "Year")]
        [Required(ErrorMessage = "{0} is required")]
        public int YearId { get; set; }
        [Display(Name = "Term Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string TermName { get; set; }
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime TermStartDate { get; set; }
        [Display(Name = "End Date")]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime TermEndDate { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public string? Event { get; set; }
        public int? Holidays { get; set; }
        public DateTime? EventDate { get; set; }
        public int? SatSunday { get; set; }
        public int? AssessmentDays { get; set; }
        public List<TermList> Terms { get; set; }
        //public int TotalDays { get; set; }
        //public int TotalSatSunday { get; set; }
        //public int Holidays { get; set; }
        //public string Event { get; set; }
        //public DateTime EventDate { get; set; }
        //public int NoOfMonths { get; set; }
        public bool AreSaturdaysOff { get; set; }
    }
}
