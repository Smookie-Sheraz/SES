using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class TopicVM
    {
        public int TopicId { get; set; }
		[Display(Name = "Topic Name")]
		public string TopicName { get; set; }
        public int? ChapterId { get; set; }
        [Display(Name ="Start Page")]
        public int StartPage { get; set; }
		public int? MinPage { get; set; }
        public int? MaxPage { get; set; }
        [Display(Name ="End Page")]
        public int EndPage { get; set; }
		public List<SubTopicVM> SubTopics = new List<SubTopicVM>();
    }
}
