using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Entities.Models
{
    public class Chapter
    {
        public int ChapterId { get; set; }
        public int? UnitId { get; set; }
        //[Display(Name = "Chapter Name")]
        //[StringLength(200)]
        public string? ChapterName { get; set; }
        public string? SLO { get; set; }
        //[Display(Name = "Start Page")]
        public int? StartPage { get; set; }
        //[Display(Name = "End Page")]
        public int? EndPage { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllocated { get; set; }
        //[Display(Name = "Modified Date")]
        //[DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }
        //[Display(Name = "Modified by")]
        public string? ModifiedBy { get; set; }
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        //[Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
        public Unit? Unit { get; set; }
        public List<Topic>? Topics { get; set; }
        public ICollection<ChapterAllocation> ChapterAllocations { get; set; } = null!;
        public ICollection<TopicAllocation> TopicAllocations { get; set; } = null!;
        public ICollection<ChapterQuestions> ChapterQuestions { get; set; } = null!;
    }
}