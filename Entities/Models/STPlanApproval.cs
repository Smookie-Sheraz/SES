using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Entities.Models
{
    public class STPlanApproval
    {
        public int STPlanApprovalId { get; set; }
        public Book? Book { get; set; }
        public int? BookId { get; set; }
        public Employee? Employee { get; set; }
        public int? EmployeeId { get; set; }
        public Section? Section { get; set; }
        public int? SectionId { get; set; }
        public Year? Year { get; set; }
        public int? YearId { get; set; }
        public Term? Term { get; set; }
        public int? TermId { get; set; }
        public string? Status { get; set; }
        public string? Comments { get; set; }
        public bool? CTApproval { get; set; }
        public bool? GMApproval { get; set; }
        public bool? ACApproval { get; set; }
        public bool? DCApproval { get; set; }
        public bool? DAApproval { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public ICollection<CTPlanApproval> CTPlans { get; set; } = null!;
        public ICollection<GMPlanApproval> GMPlans { get; set; } = null!;
        public ICollection<ACPlanApproval> ACPlans { get; set; } = null!;
        public ICollection<DCPlanApproval> DCPlans { get; set; } = null!;
        public ICollection<DAPlanApproval> DAPlans { get; set; } = null!;
    }
}