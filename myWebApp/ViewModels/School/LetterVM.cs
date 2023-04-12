namespace myWebApp.ViewModels.School
{
    public class LetterVM
    {
        public int SchoolLatterId { get; set; }
        public int? IssuingACId { get; set; }
        public int? StudentId { get; set; }
        public int? ParentId { get; set; }
        public string? SenderName { get; set; }
        public string? SenderDesignation { get; set; }
        public string? SenderContact { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverDesignation { get; set; }
        public string? ReceiverContact { get; set; }
        public string? Salutation { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? SendingDate { get; set; }
        //public DateTime? EndDate { get; set; }
        public string? Body { get; set; }
        public string? Closing { get; set; }
        public bool? IsActive { get; set; }
    }
}
