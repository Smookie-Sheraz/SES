namespace myWebApp.ViewModels.AcademicCalendar
{
    public class SubTopicList
    {
        public int TopicId { get; set; }
        public int SubTopicId { get; set; }
        public string? SubTopicName { get; set; }
        public bool IsSelected { get; set; }
        public string? TopicName { get; set; }
        public bool? preAllocation { get; set; } = false;
        public DateTime? TopicDeliveryDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? TopicStartDate { get; set; }
        public DateTime? TopicEndDate { get; set; }
        public int? WorkBookId { get; set; }
        public int? WorkBookStartPage { get; set; }
        public int? WorkBookEndPage { get; set; }
        public int? WBMinPage { get; set; }
        public int? WBMaxPage { get; set; }
    }
}
