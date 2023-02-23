namespace myWebApp.ViewModels.AcademicCalendar
{
    public class UnitList
    {
        public int? TermId { get; set; }
        public List<ChapterList> chapters = new List<ChapterList>();
        public int? UnitId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? WorkBookId { get; set; }
        public int? WorkBookStartPage { get; set; }
        public int? WorkBookEndPage { get; set; }
        public string UnitName { get; set; }
        public bool IsSelected { get; set; }
        public string? BookName { get; set; }
        public bool? preAllocation { get; set; } = false;
    }
}
