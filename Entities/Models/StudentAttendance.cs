using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class StudentAttendance
    {
        public int StudentAttendanceId { get; set; }
        public string? AttendanceStatus { get; set; }
        public string? LateOrOnTime { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Section? Class { get; set; }
        public int? ClassId { get; set; }
        public Student? Student { get; set; }
        public int? StudentId { get; set; }
    }
}