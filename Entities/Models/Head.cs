using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Head
    {
        public int HeadId { get; set; }
        public Shoora? Shoora { get; set; }
        //[AllowNull]
        public int? ShooraId { get; set; }
        public Employee? Employee { get; set; }
        //[AllowNull]
        public int? EmployeeId { get; set; }
        public bool IsActive { get; set; }
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        //[Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
        public List<SubDepartment>? subDepartments { get; set; }
    }
}