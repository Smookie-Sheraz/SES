using Entities.Models;
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
        public int? UnitId { get; set; }
        [Display(Name = "Plan")]
        [Required(ErrorMessage = "{0} is Required!")]
        public int PlanId { get; set; }
        public int? CopiedPlanId { get; set; }
        public bool IsPlanCopiable { get; set; }
        public int? SectionId { get; set; }
        public string? SearchValues { get; set; }
        public bool AreSaturdaysOff { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public List<UnitList> Units { get; set; }
        public List<AcademicPlannings>? CopyablePlans { get; set; }
    }
}
