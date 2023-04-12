using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Entities.Models
{
    public class AcademicPlannings
    {
        public int AcademicPlanningsId { get; set; }
        public string? PlanName { get; set; }
        public string? PlannedBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public Employee? Employee { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public ICollection<UnitAllocation> UnitAllocations { get; set; } = null!;
        public ICollection<ChapterAllocation> ChapterAllocations { get; set; } = null!;
        public ICollection<TopicAllocation> TopicAllocations { get; set; } = null!;
        public ICollection<SubTopicAllocation> SubTopicAllocations { get; set; } = null!;
        public ICollection<PlanApproval> PlanApprovals { get; set; } = null!;
    }
}