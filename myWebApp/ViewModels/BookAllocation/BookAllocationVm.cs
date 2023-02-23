using Infrastructure.Data;
using Infrastructure.Repositories;
using myWebApp.Controllers;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.BookAllocation
{
    public class BookAllocationVm
    {
        //public BookAllocationVM(IEFRepository repository)
        //{
        //        _repository = repository;
        //}
        public int SectionId { get; set; }
        public List<BookList> Books { get; set; }

        //public int GradeId { get; set; }
        //public int SectionId { get; set; }
        //public int BookId { get; set; }
    }
}
