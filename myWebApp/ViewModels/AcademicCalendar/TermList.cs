using myWebApp.ViewModels.BookAllocation;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class TermList
    {
        public int? YearId { get; set; }
		public List<UnitList> units = new List<UnitList>();

        //conditional porperty
		public List<BookListVM> Books = new List<BookListVM>();
		public int? TermId { get; set; }
        public string? YearName { get; set; }
        public string? TermName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TotalDays { get; set; }
        public int? TotalSatSundays { get; set; }
        public int? Holidays { get; set; }
        public int? TotalSchoolDays { get; set; }
        public int? AssessmentWiseSchoolDays { get; set; }
        public int? AssesmentDays { get; set; }
        public bool? IsActive { get; set; }
    }
}
