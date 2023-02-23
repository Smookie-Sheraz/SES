using myWebApp.ViewModels.BookAllocation;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.ViewModels.Grade
{
    public class ResourceNoteBookVm
    {
        public int ResourceNoteBookId { get; set; }
        [Display(Name = "Notebook Name")]
        public string ResourceNoteBookName { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }
        public List<ResourceNoteBookVm> NoteBooks = new List<ResourceNoteBookVm>();

    }
}
