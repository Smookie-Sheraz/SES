using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Entities.Models
{
    public class Book
    {
        public int BookId { get; set; }
        //[Display(Name = "Book Name")]
        //[StringLength(200)]
        public string? BookName { get; set; }
        //[Display(Name = "BookCode")]
        //[StringLength(100)]
        public string? BookCode { get; set; }
        //[Display(Name = "Author Name")]
        //[StringLength(200)]
        public string? Author { get; set; }
        //[Display(Name = "Publisher Name")]
        //[StringLength(200)]
        public string? Publisher { get; set; }
        //[Display(Name = "Publish Date")]
        public DateTime? PublishDate { get; set; }
        public Subject? Subject { get; set; }
        public int? SubjectId { get; set; }
        public Grade? Grade { get; set; }
        public int? GradeId { get; set; }
        public string? ResourceBook { get; set; }
        public string? ResourceBookPath { get; set; }
        public int? ResourceNoteBookId { get; set; }
        public ResourceNoteBook? ResourceNoteBook { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsWorkBook { get; set; }
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
        public ICollection<Unit> Units { get; set; } = null!;
        public ICollection<UnitAllocation> unitAllocations { get; set; } = null!;
        public ICollection<ChapterAllocation> ChapterAllocations { get; set; } = null!;
        public ICollection<TopicAllocation> TopicAllocations { get; set; } = null!;
        public ICollection<SubTopicAllocation> SubTopicAllocations { get; set; } = null!;
        public ICollection<SubjectTeacherAllocation> SubjectTeacherAllocations { get; set; } = null!;
        public ICollection<STPlanApproval> STPlans { get; set; } = null!;
        public ICollection<PlanApproval> Plans { get; set; } = null!;
        public ICollection<Test> Tests { get; set; } = null!;
    }
}