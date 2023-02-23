using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class UpdateTermVM
    {
        [Display(Name = "Term Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string TermName { get; set; }
        public int TermId { get; set; }
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Year")]
        [Required(ErrorMessage = "{0} is required")]
        public int YearId { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public int? SatSunday { get; set; }
        public int? AssessmentDays { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }
    }
}
