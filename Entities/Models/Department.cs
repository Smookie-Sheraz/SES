using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        //[Display(Name = "Department Name")]
        //[StringLength(200)]
        public string? DepartmentName { get; set; }
        public Shoora? Shoora { get; set; }
        //[Display(Name = "Shoora Member")]
        //[AllowNull]
        public int? DepartmentHeadId { get; set; }
        //[Display(Name = "Discription")]
        //[StringLength(1000)]
        public string? Description { get; set; }
        //[Display(Name = "Short Description")]
        //[StringLength(500)]
        public string? ShortDescripiton { get; set; }
        public bool? IsActive { get; set; }
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        //[Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
        public ICollection<Employee> Employees { get; set; } = null!;
        public ICollection<SubDepartment> SubDepartments { get; set; } = null!;

    }
}