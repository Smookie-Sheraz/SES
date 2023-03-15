using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        //[Display(Name = "Grade Name")]
        //[StringLength(200)]
        public string? GradeName { get; set; }
        //[Display(Name = "GradeCode")]
        //[StringLength(100)]
        public string? GradeCode { get; set; }
        public bool? IsActive { get; set; }
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        //[Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public Employee? GradeManager { get; set; }
        public int? GradeManagerId { get; set; }
        public SchoolSection? SchoolSection { get; set; }
        public int? SchoolSectionId { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
        public ICollection<Section> Sections { get; set; } = null!;
        public ICollection<Book> Books { get; set; } = null!;


    }
}