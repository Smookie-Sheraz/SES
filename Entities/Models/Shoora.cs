namespace Entities.Models
{
    public class Shoora
    {
        public int ShooraId { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? FatherName { get; set; }
        public string? SpouseName { get; set; }
        public Gender? Gender { get; set; }
        public int? GenderId { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Mobile { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DOB { get; set; }
        public string? CNICNo { get; set; }
        public DateTime? CNICIssueDate { get; set; }
        public DateTime? CNICExpiryDate { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public School? School { get; set; }
        public int? SchoolId { get; set; }
        public Campus? Campus { get; set; }
        public int? CampdusId { get; set; }
        public DateTime? DeletedOn { get; set; }
        public List<Head>? Heads { get; set; }
        public List<Department>? Departments { get; set; }

    }
}