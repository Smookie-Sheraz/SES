namespace myWebApp.ViewModels.AcademicCalendar
{
    public class YearList
    {
        public int? YearId { get; set; }
        public string? YearName { get; set; }
        public DateTime? StartDate { get; set; }
        public List<TermList> terms = new List<TermList>();
        public DateTime? EndDate { get; set; }
        public int? TotalDays { get; set; }
        public int? TotalSatSundays { get; set; }
        public int? Holidays { get; set; }
        public int? TotalSchoolDays { get; set; }
        public int? TotalAssesWiseSchoolDays { get; set; }
        public bool? IsLeapYear { get; set; }
    }
}
