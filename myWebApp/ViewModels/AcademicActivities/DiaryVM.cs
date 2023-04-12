using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using myWebApp.Controllers;
using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicActivities
{
    public class DiaryVM
    {
        public int DiaryId { get; set; }
        public string? ClassWork { get; set; }
        public int? HomeWork { get; set; }
        public bool? IsActive { get; set; }
        public int? TestId { get; set; }
        public int? SubjectId { get; set; }
        public int? ClassId { get; set; }
    }
}
