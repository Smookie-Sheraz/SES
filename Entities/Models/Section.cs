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
        //public Book Book { get; set; }
        //public int? BookId { get; set; }
        public Grade? Grade { get; set; }
        public int? GradeId { get; set; }
    }
}