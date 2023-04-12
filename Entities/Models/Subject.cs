using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        //[Display(Name = "Subject Name")]
        //[StringLength(200)]
        public string? SubjectName { get; set; }
        //[Display(Name = "SubjectCode")]
        //[StringLength(100)]
        public string? SubjectCode { get; set; }
        public bool? IsActive { get; set; }
        public ICollection<Book> Books { get; set; } = null!;
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        //[Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
        public ICollection<SubjectAllocation> SubjectAllocations { get; set; } = null!;
        public ICollection<Diary> Diaries { get; set; } = null!;
        public ICollection<Test> Tests { get; set; } = null!;
    }
}