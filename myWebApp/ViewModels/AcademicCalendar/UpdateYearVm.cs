using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class UpdateYearVM
    {
        public int YearId { get; set; }
        [Display(Name = "Year Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string YearName { get; set; }
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime EndDate { get; set; }
        public bool? IsLeapYear { get; set; } = false;
        [Display(Name = "Status")]
        public bool IsActive { get; set; }

    }
}
