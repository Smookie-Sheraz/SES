using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class SubDepartment
    {
        public int SubDepartmentId { get; set; }
        //[Display(Name = "Sub Department Name")]
        //[StringLength(200)]
        public string? DepartmentName { get; set; }
        public Department? MainDepartment { get; set; }
        //[Display(Name = "Main Department")]
        public int? MainDepartmentId { get; set; }
        //[Display(Name = "Head of the Department")]
        public int? HeadId { get; set; }
        public Head? Head { get; set; }
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        //[Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
        public List<Employee>? Employees { get; set; }

    }
}