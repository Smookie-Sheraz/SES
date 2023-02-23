using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Campus
    {
        public int CampusId { get; set; }
        public string? CampusName { get; set; }
        public string? PrincipalName { get; set; }
        public string? Abbrevation { get; set; }
        public string? address { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public School? School { get; set; }
        public int? SchoolId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public ICollection<SchoolSection> sections { get; set; } = null!;
        public ICollection<Employee> Employees { get; set; } = null!;
        public ICollection<Shoora> Shooras { get; set; } = null!;

    }
}
