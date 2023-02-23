using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class SubjectVM
    {
        public string? SubjectCode { get; set; }
		[Display(Name = "Subject Name")]
		public string? SubjectName { get; set; }
		public bool? IsActive { get; set; }
        
    }
}
