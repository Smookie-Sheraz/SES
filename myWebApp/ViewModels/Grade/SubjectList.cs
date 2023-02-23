namespace myWebApp.ViewModels.Grade
{
    public class SubjectList
    {
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public bool? IsSelected { get; set; }
        public bool? preAllocation { get; set; } = false;

    }
}
