using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.School
{
    public class NoticeVM
    {
        public int SchoolNoticeId { get; set; }
        public int? IssuingACId { get; set; }
        public string? Title { get; set; }
        public string? Salutation { get; set; }
        public string? Recipient { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? BroadcastDate { get; set; }
        //public DateTime? EndDate { get; set; }
        public string? Body { get; set; }
        public bool? IsActive { get; set; }
    }
}
