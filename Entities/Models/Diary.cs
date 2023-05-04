using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Diary
    {
        public int DiaryId { get; set; }
        public string? ClassWork { get; set; }
        public string? HomeWork { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? Date { get; set; }
        public Test? Test { get; set; }
        public int? TestId { get; set; }
        public Subject? Subject { get; set; }
        public int? SubjectId { get; set; }
        public Section? Class { get; set; }
        public int? ClassId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }

    }
}
