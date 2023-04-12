using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class SchoolLatter
    {
        public int SchoolLatterId { get; set; }
        public Employee? IssuingAC { get; set; }
        public int? IssuingACId { get; set; }
        public Student? Student { get; set; }
        public int? StudentId { get; set; }
        public Parent? Parent { get; set; }
        public int? ParentId { get; set; }

        public string? SenderName { get; set; }
        public string? SenderDesignation { get; set; }
        public string? SenderContact { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverDesignation { get; set; }
        public string? ReceiverContact { get; set; }
        public string? Salutation { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? SendingDate { get; set; }
        //public DateTime? EndDate { get; set; }
        public string? Body { get; set; }
        public string? Closing { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
