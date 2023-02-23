using Entities.ViewModels;

namespace myWebApp.ViewModels.BookAllocation
{
    public class BookViewList
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? SubjectName { get; set; }
        public string? Author { get; set; }
        public string? GardeName { get; set; }
        public string? Publisher { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? ResourceBookURL { get; set; }
        public string? ResourceNoteBook { get; set; }
        public string? ResourceBook { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsWorkBook { get; set; }
        public List<UnitVM> units = new List<UnitVM>();
    }
}
