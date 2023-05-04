using Infrastructure.Data;
using Infrastructure.Repositories;
using myWebApp.Controllers;
using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class PlanDetailsVM
    {
        public string? CalendarName { get; set; }
        public int? totalItems { get; set; }
        public int? GradeId { get; set; }
        public int? SubjectId { get; set; }
        public int? BookId { get; set; }
        public int? YearId { get; set; }
        public int? DurationDays { get; set; }
        public int? SchoolSectionId { get; set; }
        public string? GradeName { get; set; }
        public string? SubjectName { get; set; }
        public string? Textbook { get; set; }
        public string? Workbook { get; set; }
        public string? TeacherName { get; set; }
        public string? ClassName { get; set; }
        public string? Duration { get; set; }

        public List<YearList> years = new List<YearList>();
        public List<PLanList> Plan = new List<PLanList>();
        // Below attributes are used when the subject teacher is logged in it needs these including the YearId above
        public int? TermId { get; set; }
        public int? SectionId { get; set; }

        //Below attribtues are used in PlanApproval 
        //This property is used to set the remarks of the plan
        public string? PlanRemarks { get; set; }
        //This property is used to forward the PlanId
        public string? PlanStatus { get; set; }
        public int? PlanId { get; set; }

        // to check weather a approving person has already approved the plan or not? And according show the approval form
        public bool IsApprovedByMe { get; set; }
    }
}
