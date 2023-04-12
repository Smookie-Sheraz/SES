using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Parent
{
    public class LeaveApplicationVM
    {
        public int LeaveApplicationId { get; set; }
        public int StudentId { get; set; }
        [Display(Name = "Reason")]
        [Required(ErrorMessage = "{0} is Required!")]
        public string Reason { get; set; }
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "{0} is Required!")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [Required(ErrorMessage = "{0} is Required!")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        public string? MinDate{ get; set; }
        public string? Status { get; set; }
    }
}
