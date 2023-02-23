using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class SubTopicVM
    {
        [Display(Name ="Sub-Topic Name")]
        public string SubTopicName { get; set; }
        public int TopicId { get; set; }
        [Display(Name ="Start Page")]
        public int StartPage { get; set; }
		[Display(Name = "End Page")]
		public int EndPage { get; set; }
        public int? MinPage { get; set; }
        public int? MaxPage { get; set; }
    }
}
