using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class SchoolNotice
    {
        public int SchoolNoticeId { get; set; }
        public Employee? IssuingAC { get; set; }
        public int? IssuingACId { get; set; }
        public string? Title { get; set; }
        public string? Salutation { get; set; }
        public string? Recipient { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? BroadcastDate { get; set; }
        //public DateTime? EndDate { get; set; }
        public string? Body { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
