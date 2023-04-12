using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Test
    {
        public int TestId { get; set; }
        public string? TestTitle { get; set; }
        public int? TotalMarks { get; set; }
        public int? ObtainedMarks { get; set; }
        public bool? IsActive { get; set; }
        public Section? Class { get; set; }
        public int? ClassId { get; set; }
        public Book? Book { get; set; }
        public int? BookId { get; set; }
        public Subject? Subject { get; set; }
        public int? SubjectId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public ICollection<Diary> Diaries { get; set; } = null!;

    }
}
