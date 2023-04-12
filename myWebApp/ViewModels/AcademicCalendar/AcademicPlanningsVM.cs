namespace myWebApp.ViewModels.AcademicCalendar
{
    public class AcademicPlanningsVM
    {
        public int AcademicPlanningsId { get; set; }
        public string? PlanName { get; set; }
        public string? PlannedBy { get; set; }
        public string? ApprovalStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public int? EmployeeId { get; set; }
        public string? ApprovingPerson { get; set; }
        public List<AcademicPlanningsVM>? plans { get; set; }
        public bool ActiveSubmitPlan { get; set; } = false;
    }
}
