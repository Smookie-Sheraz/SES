namespace myWebApp.ViewModels.AcademicCalendar
{
    public class ChapterList
    {
        public int TermId { get; set; }
        public List<TopicList> topics = new List<TopicList>();
        public int ChapterId { get; set; }
        public int UnitId { get; set; }
        public int UnitAllocationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ChapterName { get; set; }
        public string? UnitName { get; set; }
        public bool IsSelected { get; set; }
        public bool? preAllocation { get; set; } = false;
        public int? WorkBookId { get; set; }
        public int? WorkBookStartPage { get; set; }
        public int? WorkBookEndPage { get; set; }
        public int? WBMinPage { get; set; }
        public int? WBMaxPage { get; set; }
    }
}
