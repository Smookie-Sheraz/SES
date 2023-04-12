using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Teacher
{
    public class StudentAttendanceVM
    {
        public string? TeacherName { get; set; }
        public string? ClassName { get; set; }
        public string? Date { get; set; }
        public List<StudentAttendanceList> AttendanceList { get; set; }
    }
}
