using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class UpdateBookAllocationVM
    {
        public int BookAllocationId { get; set; }
        public int? GradeId { get; set; }
        public int? SectionId { get; set; }
        public int? BookId { get; set; }
    }
}
