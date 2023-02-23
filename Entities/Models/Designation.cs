using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Designation
    {
        public int DesignationId { get; set; }
        //[Display(Name = "Designation Name")]
        //[StringLength(50)]
        public string? Name { get; set; }
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
        public ICollection<Employee> Employees { get; set; } = null!;

    }
}