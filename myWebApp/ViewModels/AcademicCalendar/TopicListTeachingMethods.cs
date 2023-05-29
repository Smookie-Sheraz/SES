﻿namespace myWebApp.ViewModels.AcademicCalendar
{
    public class TopicListTeachingMethods
    {
        public int? TopicId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TeachingMethodology { get; set; }
        public int? TeachingMethodologyId { get; set; }
        public string? TeachingMethodologyDesc { get; set; }
        //public bool IsSelected { get; set; } = false;
        public string? Check { get; set; } = "No";
        //public bool? preAllocation { get; set; } = false;
        public int? WorkBookId { get; set; }
        public int? WorkBookStartPage { get; set; }
        public int? WorkBookEndPage { get; set; }
        public int? WBMinPage { get; set; }
        public int? WBMaxPage { get; set; }
    }
}
