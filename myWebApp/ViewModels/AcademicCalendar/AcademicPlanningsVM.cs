using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class AcademicPlanningsVM
    {
        public int AcademicPlanningsId { get; set; }
        public string PlanName { get; set; }
        public string? PlannedBy { get; set; }
        public string? ApprovalStatus { get; set; }
        //[Display (Name = "Class")]
        //[Required(ErrorMessage = "{0} Is Required!")]
        
        public int? ClassId { get; set; }
        //[Display(Name = "Subject")]
        //[Required(ErrorMessage = "{0} Is Required!")]
        public int? SubjectId { get; set; }
        //[Display(Name = "Book")]
        //[Required(ErrorMessage = "{0} Is Required!")]
        public int? BookId { get; set; }
        public string? ClassName { get; set; }
        public string? BookName { get; set; }
        public string? SubjectName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public int? EmployeeId { get; set; }
        public string? ApprovingPerson { get; set; }
        public List<AcademicPlanningsVM>? plans { get; set; }

        //to disable the submit plan approval button 
        public bool ActiveSubmitPlan { get; set; } = false;
    }
}
