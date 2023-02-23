using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class UpdateTopicVM
    {
        public string TopicName { get; set; }
        public int TopicId { get; set; }
        [Display(Name = "Chapter")]
        [Required(ErrorMessage = "{0} is required")]
        public int ChapterId { get; set; }
        [Display(Name = "Start Page")]
        public int StartPage { get; set; }
        [Display(Name = "End Page")]
        public int EndPage { get; set; }
        public int? MinPage { get; set; }
        public int? MaxPage { get; set; }
        [Display(Name ="Status")]
        public bool IsActive { get; set; }
    }
}
