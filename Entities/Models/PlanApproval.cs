using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Entities.Models
{
    public class PlanApproval
    {
        public int PlanApprovalId { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public string? ApprovingPerson { get; set; }
        public string? ApprovingPersonName { get; set; }
        public bool? IsActive { get; set; }
        public Book? BookPlan { get; set; }
        public int? BookId { get; set; }
        public AcademicPlannings? Plan { get; set; }
        public int? PlanId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}