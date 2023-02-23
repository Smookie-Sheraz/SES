using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Entities.Models
{
    public class SubjectTeacherAllocation
    {
        public int SubjectTeacherAllocationId { get; set; }
        public Section? Section { get; set; }
        public int? SectionId { get; set; }
        public Employee? Employee { get; set; }
        public int? EmployeeId { get; set; }
        public Book? Book { get; set; }
        public int? BookId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}