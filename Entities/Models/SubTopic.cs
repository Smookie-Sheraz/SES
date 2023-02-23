using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Entities.Models
{
    public class SubTopic
    {
        public int SubTopicId { get; set; }
        public Topic? Topic { get; set; }
        public int? TopicId { get; set; }
        //[Display(Name = "Chapter Name")]
        //[StringLength(200)]
        public string? SubTopicName { get; set; }
        //[Display(Name = "Start Page")]
        public int? StartPage { get; set; }
        //[Display(Name = "End Page")]
        public int? EndPage { get; set; }
        public bool IsActive { get; set; }
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
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
        public ICollection<SubTopicAllocation> SubTopicAllocations { get; set; } = null!;
    }
}