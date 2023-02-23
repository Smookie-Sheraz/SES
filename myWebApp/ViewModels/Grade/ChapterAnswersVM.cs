using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Grade
{
    public class ChapterAnswersList
    {
        [Display(Name = "Answer Status")]
        [Required(ErrorMessage = "{0} is Required!")]
        public bool IsTrue { get; set; }
        [Display(Name = "Quiz Status")]
        [Required(ErrorMessage = "{0} is Required!")]
        public bool IsActive { get; set; }
        [Display(Name = "Choice")]
        [Required(ErrorMessage ="{0} is Required!")]
        public string Choice { get; set; }
    }
}
