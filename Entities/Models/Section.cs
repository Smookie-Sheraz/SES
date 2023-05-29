using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Section
    {
        public int SectionId { get; set; }
        //[Display(Name = "Section Name")]
        //[StringLength(200)]
        public string? SectionName { get; set; }
        //[Display(Name = "GradeCode")]
        //[StringLength(100)]
        public string? SectionCode { get; set; }
        public bool? IsActive { get; set; }
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        //[Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
        public ICollection<BookAllocation> BookAllocations { get; set; } = null!;
        public ICollection<SubjectAllocation> SubjectAllocations { get; set; } = null!;
        public ICollection<SubjectTeacherAllocation> SubjectTeacherAllocations { get; set; } = null!;
        public ICollection<UnitAllocation> UnitAllocations { get; set; } = null!;
        public ICollection<ChapterAllocation> ChapterAllocations { get; set; } = null!;
        public ICollection<TopicAllocation> TopicAllocations { get; set; } = null!;
        public ICollection<SubTopicAllocation> SubTopicAllocations { get; set; } = null!;
        public ICollection<Student> Students { get; set; } = null!;
        public ICollection<STPlanApproval> STPlans { get; set; } = null!;
        public ICollection<StudentAttendance> StudentAttendances { get; set; } = null!;
        public ICollection<AcademicPlannings> AcademicPlannings { get; set; } = null!;
        public ICollection<Test> Tests { get; set; } = null!;
        public ICollection<Diary> Diaries { get; set; } = null!;
        //public Book Book { get; set; }
        //public int? BookId { get; set; }
        public Grade? Grade { get; set; }
        public int? GradeId { get; set; }
        public Employee? ClassTeacher { get; set; }
        public int? ClassTeacherId { get; set; }
    }
}