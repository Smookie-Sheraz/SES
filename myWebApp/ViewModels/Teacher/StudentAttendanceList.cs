using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Teacher
{
    public class StudentAttendanceList
    {
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public string? AttendanceStatus { get; set; }
        public string? OnTimeOrLate { get; set; }
        public string? StudentName { get; set; }
        public string? ParentName { get; set; }
        public string? RollNo { get; set; }
        public string? LeaveReason { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? LeaveResponse { get; set; }
        public string? LeaveStatus { get; set; }
    }
}
