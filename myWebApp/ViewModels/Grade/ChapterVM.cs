using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class ChapterVM
    {
        public string? ChapterName { get; set; }
        public string? SLO { get; set; }
        public int? BookId { get; set; }
        public int? UnitId { get; set; }
        [Display(Name = "Start Page")]
        public int StartPage { get; set; }
        public int? MinPage { get; set; }
        public int? MaxPage { get; set; }
		[Display(Name = "End Page")]
		public int EndPage { get; set; }
        public List<TopicVM> topics = new List<TopicVM>();
    }
}
