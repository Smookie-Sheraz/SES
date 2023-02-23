using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Grade
{
    public class ChapterQuestionsVM
    {
        public int QuestionId { get; set; }
        public int UnitId { get; set; }
        public int ChapterId { get; set; }
        [Display(Name = "Quiz Status")]
        [Required(ErrorMessage = "{0} is Required!")]
        public bool IsActive { get; set; }
        [Display(Name = "Question")]
        [Required(ErrorMessage ="{0} is Required!")]
        public string Question { get; set; }
        [Display(Name = "Topic")]
        [Required(ErrorMessage = "{0} is Required!")]
        public string Topic { get; set; }
        [Display(Name = "Question Type")]
        [Required(ErrorMessage = "{0} is Required!")]
        public string QuestionType { get; set; }
        public bool? TrueOrFalse { get; set; }
        public List<ChapterAnswersList> Answers { get; set; }
    }
}
