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
        [Display(Name = "Topic")]
        [Required(ErrorMessage = "{0} is Required!")]
        public int TopicId { get; set; }
        public int TermId { get; set; }
        [Display(Name = "Plan")]
        [Required(ErrorMessage = "{0} is Required!")]
        public int PlanId { get; set; }
        public int BookId { get; set; }
        public int SectionId { get; set; }
        [Display(Name = "Unit")]
        [Required(ErrorMessage = "{0} is Required!")]
        public int UnitId { get; set; }
        [Display(Name = "Chapter")]
        [Required(ErrorMessage = "{0} is Required!")]
        public int ChapterId { get; set; }
        public bool AreSaturdaysOff { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public List<SubTopicList> SubTopics { get; set; }
    }
}
