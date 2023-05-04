﻿using Infrastructure.Data;
using Infrastructure.Repositories;
using myWebApp.Controllers;
using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class SubTopicAllocationVM
    {
        public int YearId { get; set; }
        public int? TopicId { get; set; }
        public int TermId { get; set; }
        public int PlanId { get; set; }
        public int BookId { get; set; }
        public int SectionId { get; set; }
        public int UnitId { get; set; }
        public int ChapterId { get; set; }
        public bool AreSaturdaysOff { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public List<SubTopicList> SubTopics { get; set; }
    }
}
