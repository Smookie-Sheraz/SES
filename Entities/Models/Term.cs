using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Term
    {
        public int TermId { get; set; }
        public int? YearId { get; set; }
        //[Display(Name = "Term Name")]
        public string? TermName { get; set; }

        //[Display(Name = "Start Date")]
        //[DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        //[Display(Name = "End Date")]
        //[DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        //[Display(Name = "Total Days")]
        public int? TotalDays { get; set; }
        //[Display(Name = "Total School Days")]
        public int? TotalSchoolDays { get; set; }
        public int? AssesmentDays { get; set; }
        public int? AssesmentWiseTermDays { get; set; }

        //[Display(Name = "Holidays")]
        public int? TermHolidays { get; set; }
        //[Display(Name = "Total Sat & Sun")]
        public bool? IsActive { get; set; }
        public int? TotalSatSun { get; set; }
        //[Display(Name = "Modified Date")]
        //[DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }
        //[Display(Name = "Modified by")]
        public string? ModifiedBy { get; set; }
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        //[Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? AreSaturdaysOff { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
        public Year? Year { get; set; }
        //public List<Month> Months { get; set; }
        public List<UnitAllocation> UnitAllocations { get; set; }
        public List<ChapterAllocation> ChapterAllocations { get; set; }
        public List<TopicAllocation> TopicAllocations { get; set; }
        public List<SubTopicAllocation> SubTopicAllocations { get; set; }
        public List<Holiday> Holidays { get; set; }
        public ICollection<STPlanApproval> STPlans { get; set; } = null!;
    }
}