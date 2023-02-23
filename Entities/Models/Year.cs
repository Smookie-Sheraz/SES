using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Year
    {
        public int YearId { get; set; }
        //[Display(Name = "Year Name")]
        public string? YearName { get; set; }
        //[Display(Name = "Start Date")]
        //[DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        //[Display(Name = "End Date")]
        //[DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        //[Display(Name = "Total Days")]
        public int? TotalDays { get; set; }
        //[Display(Name = "Total Saturdays & Sundays")]
        public int? TotalSatSundays { get; set; }
        //[Display(Name = "Holidays")]
        public int? Holidays { get; set; }
        //[Display(Name = "Total School Days")]
        public int? TotalSchoolDays { get; set; }
        public int? AssesmentDays { get; set; }
        //[Display(Name = "Total Assesment Wise Days")]
        public int? TotalAssesWiseSchoolDays { get; set; }
        //[Display(Name = "Is The Year Leap")]
        public bool? IsLeapYear { get; set; }
        //[Display(Name = "Modified Date")]
        //[DataType(DataType.Date)]
        public bool? IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //[Display(Name = "Modified by")]
        public string? ModifiedBy { get; set; }
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        //[Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
        public List<Term> Terms { get; set; }

    }
}