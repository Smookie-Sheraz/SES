//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Diagnostics.CodeAnalysis;

//namespace Entities.Models
//{
//    public class Month
//    {
//        public int MonthId { get; set; }
//        public int TermId { get; set; }
//        //[Display(Name = "Start Date")]
//        //[DataType(DataType.Date)]
//        public DateTime? StartDate { get; set; }
//        //[Display(Name = "End Date")]
//        //[DataType(DataType.Date)]
//        public DateTime? EndDate { get; set; }
//        //[Display(Name = "Total Days")]
//        public int? TotalDays { get; set; }
//        //[Display(Name = "Total Saturdays & Sundays")]
//        public int? TotalSatSundays { get; set; }
//        public int? AssesmentDays { get; set; }
//        public string? Event { get; set; }

//        //[Display(Name = "Event Date")]
//        //[DataType(DataType.Date)]
//        public DateTime? EventDate { get; set; }
//        //[Display(Name = "Holidays")]
//        public int? Holidays { get; set; }
//        //[Display(Name = "Total School Days")]
//        public int? TotalSchoolDays { get; set; }
//        //[Display(Name = "Modified Date")]
//        //[DataType(DataType.Date)]
//        public DateTime? ModifiedDate { get; set; }
//        //[Display(Name = "Modified by")]
//        public string? ModifiedBy { get; set; }
//        //[Display(Name = "Created Date")]
//        //[DataType(DataType.Date)]
//        public DateTime? CreatedDate { get; set; }
//        //[Display(Name = "Created by")]
//        public string? CreatedBy { get; set; }
//        public bool? IsDeleted { get; set; }
//        //[Display(Name = "Created On")]
//        //[DataType(DataType.Date)]
//        public DateTime? DeletedOn { get; set; }
//        public Term Term { get; set; }
//        //public ICollection<UnitAllocation> UnitAllocations { get; set; } = null!;
//        //public ICollection<ChapterAllocation> ChapterAllocations { get; set; } = null!;
//        //public ICollection<TopicAllocation> TopicAllocations { get; set; } = null!;
//        //public ICollection<SubTopicAllocation> SubTopicAllocations { get; set; } = null!;
//    }
//}