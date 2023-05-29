using Infrastructure.Data;
using Infrastructure.Repositories;
using myWebApp.Controllers;
using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class TopicListTest
    {
        public int? ChapterId { get; set; }
        public int? TopicId { get; set; }
        public string? TopicName { get; set; }
        public DateTime? ChapterStartDate { get; set; }
        public DateTime? ChapterEndDate { get; set; }
        //public List<TopicListTeachingMethodsTest> TeachingMethods { get; set; }
        //public int? TopicId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TeachingMethodology { get; set; }
        public string? TeachingMethodologyDesc { get; set; }
        //public bool IsSelected { get; set; } = false;
        public bool Check { get; set; } = false;
        //public bool? preAllocation { get; set; } = false;
        public int? WorkBookId { get; set; }
        public int? WorkBookStartPage { get; set; }
        public int? WorkBookEndPage { get; set; }
        public int? WBMinPage { get; set; }
        public int? WBMaxPage { get; set; }
    }
}
