﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicCalendar
{
    public class TopicListTeachingMethods
    {
        public int? TopicId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Display(Name = "Chapter Start Date")]
        [Remote("StartDateNotValid", "Validations", ErrorMessage = "{0} Can't be set to this")]
        //public DateTime? ChapterStartDate { get; set; }
        //public DateTime? ChapterEndDate { get; set; }
        public string? TeachingMethodology { get; set; }
        public int? TeachingMethodologyId { get; set; }
        public string? TeachingMethodologyDesc { get; set; }
        public bool IsSelected { get; set; }
        public string? ChapterName { get; set; }
        public bool? preAllocation { get; set; } = false;
        public int? WorkBookId { get; set; }
        public int? WorkBookStartPage { get; set; }
        public int? WorkBookEndPage { get; set; }
        public int? WBMinPage { get; set; }
        public int? WBMaxPage { get; set; }
    }
}
