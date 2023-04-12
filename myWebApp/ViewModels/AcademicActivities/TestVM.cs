using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using myWebApp.Controllers;
using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.AcademicActivities
{
    public class TestVM
    {
        public int TestId { get; set; }
        public string? TestTitle { get; set; }
        public int? TotalMarks { get; set; }
        public int? ObtainedMarks { get; set; }
        public bool? IsActive { get; set; }
        public int? ClassId { get; set; }
        public int? BookId { get; set; }
        public int? SubjectId { get; set; }
    }
}
