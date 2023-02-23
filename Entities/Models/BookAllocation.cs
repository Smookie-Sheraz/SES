using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Entities.Models
{
    public class BookAllocation
    {
        public int BookAllocationId { get; set; }
        public Section? Section { get; set; }
        public int? SectionId { get; set; }
        public Book? Book { get; set; }
        public int? BookId { get; set; }
        //[Display(Name = "Created Date")]
        //[DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        //[Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        //[Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }

        public bool preExist { get; set; }
    }
}