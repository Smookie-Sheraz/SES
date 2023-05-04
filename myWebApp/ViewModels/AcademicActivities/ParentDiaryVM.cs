using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using myWebApp.Controllers;
using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicActivities
{
    public class ParentDiaryVM
    {
        public int DiaryId { get; set; }
        public string? ClassWork { get; set; }
        public string? SHomeWork { get; set; }
        public string? SubjectName { get; set; }
        public string? Test { get; set; }
        public int? HomeWork { get; set; }
        public bool? IsActive { get; set; }
        public int? TestId { get; set; }
        public int? SubjectId { get; set; }
        public int? ClassId { get; set; }
        public int? StudentId { get; set; }
        public DateTime? DiaryDate { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public List<ParentDiaryVM>? Diaries { get; set; }
    }
}
