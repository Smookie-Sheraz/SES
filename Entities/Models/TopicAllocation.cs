using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Entities.Models
{
    public class TopicAllocation
    {
        public int TopicAllocationId { get; set; }
        public Term? Term { get; set; }
        public int? TermId { get; set; }
        public Topic? Topic { get; set; }
        public int? TopicId { get; set; }
        public Book? WorkBook { get; set; }
        public int? WorkBookId { get; set; }
        public int? WorkBookStartPage { get; set; }
        public int? WorkBookEndPage { get; set; }
        public Chapter? Chapter { get; set; }
        public int? ChapterId { get; set; }
        public TeachingMethodology? TeachingMethodology { get; set; }
        public int? TeachingMethodologyId { get; set; }
        public Section? Section { get; set; }
        public int? SectionId { get; set; }
        public string? TMethodDesc { get; set; }
        public bool IsAllocated { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}