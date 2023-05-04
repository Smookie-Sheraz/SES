using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Entities.Models
{
    public class ChapterAllocation
    {
        public int ChapterAllocationId { get; set; }
        public Term? Term { get; set; }
        public int? TermId { get; set; }
        public Chapter? Chapter { get; set; }
        public int? ChapterId { get; set; }
        public Unit? Unit { get; set; }
        public int? UnitId { get; set; }
        public Book? WorkBook { get; set; }
        public int? WorkBookId { get; set; }
        public Section? Section { get; set; }
        public int? SectionId { get; set; }
        public AcademicPlannings? Plan { get; set; }
        public int? PlanId { get; set; }
        public int? WorkBookStartPage { get; set; }
        public int? WorkBookEndPage { get; set; }
        public bool? IsActive { get; set; }
        public bool? AreSaturdaysOff { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
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

    }
}