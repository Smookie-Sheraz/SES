namespace myWebApp.ViewModels.AcademicCalendar
{
    public class MonthList
    {
        public int? MonthId { get; set; }
        public int? TermId { get; set; }
        public string? TermName { get; set; }
        public string? MonthName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TotalDays { get; set; }
        public int? TotalSatSundays { get; set; }
        public string? Event { get; set; }
        public DateTime? EventDate { get; set; }
        public int? Holidays { get; set; }
        public int? TotalSchoolDays { get; set; }
        public List<UnitList> units = new List<UnitList>();
        //public List<TopicList> topicLists = new List<TopicList>();
        //public List<SubTopicList> subTopicLists = new List<SubTopicList>();
    }
}
