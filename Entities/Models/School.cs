using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class School
    {
        public int SchoolId { get; set; }
        public string? SchoolName { get; set; }
        public string? CEOName { get; set; }
        public int? RegistrationNo { get; set; }
        public string? Abbrevation { get; set; }
        public string? address { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public ICollection<Campus> campuses { get; set; } = null!;
        public ICollection<SchoolSection> SchoolSections { get; set; } = null!;
        public ICollection<Employee> Employees { get; set; } = null!;
        public ICollection<Shoora> Shooras { get; set; } = null!;
    }
}
