using Infrastructure.Data;
using Infrastructure.Repositories;
using myWebApp.Controllers;
using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class TopicAllocationVM
    {
        public int YearId { get; set; }
        public int TermId { get; set; }
        
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public List<TopicList> Topics { get; set; }
    }
}
