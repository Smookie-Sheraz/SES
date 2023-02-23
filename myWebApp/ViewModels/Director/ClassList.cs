namespace myWebApp.ViewModels.BookAllocation
{
    public class ClassList
    {
        public string? ClassName { get; set; }
        public int? SectionId { get; set; }
        public List<BookList> books { get; set; } 
    }
}
