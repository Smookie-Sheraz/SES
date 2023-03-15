using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        //[Display(Name = "Employee Code")]
        //[StringLength(10, ErrorMessage = "{0} can't exceed {1} characters")]
        public string? EmployeeCode { get; set; }
        //[Display(Name = "First Name")]
        public string? FName { get; set; }
        //[Display(Name = "Last Name")]
        public string? LName { get; set; }
        //[Display(Name = "Father Name")]
        public string? FatherName { get; set; }
        //[Display(Name = "Spouse Name")]
        public string? SpouseName { get; set; }
        public Gender? Gender { get; set; }
        //[Display(Name = "Gender")]
        public int? GenderId { get; set; }
        public School? School { get; set; }
        public int? SchoolId { get; set; }
        public Campus? Campus { get; set; }
        public int? CampusId { get; set; }
        public SchoolSection? SchoolSection { get; set; }
        public int? SchoolSectionId { get; set; }
        //[Display(Name = "Marital Status")]
        public string? MaritalStatus { get; set; }
        public string? UserImageUrl { get; set; }
        //[Display(Name = "Mobile No")]
        public string? Mobile { get; set; }
        //[Display(Name = "Date of Birth")]
        public DateTime? DOB { get; set; }
        public bool? IsActive { get; set; }
        //[Display(Name = "CNIC No")]
        public string? CNICNo { get; set; }
        //[Display(Name = "CNIC Issue Date")]
        public DateTime? CNICIssueDate { get; set; }
        //[Display(Name = "CNIC Expiry Date")]
        public DateTime? CNICExpiryDate { get; set; }
        //[Display(Name = "Email Address")]
        //[EmailAddress]
        public string? Email { get; set; }
        //[Display(Name = "Address")]
        public string? Address { get; set; }
        //[Display(Name = "Field of Specialization")]
        public string? FieldOfSpecialization { get; set; }
        //[Display(Name = "Joining Date")]
        public DateTime? JoiningDate { get; set; }
        //[Display(Name = "Probation Period")]
        public int? ProbationPeriod { get; set; }
        //[Display(Name = "Period")
        public int? Period { get; set; }
        //[Display(Name = "Start Of Period Date")]
        public DateTime? StartofPeriodDate { get; set; }
        //[Display(Name = "End Of Period Date")]
        public DateTime? EndofPeriodDate { get; set; }
        //[Display(Name = "Start Of Probation Date")]
        public DateTime? StartofProbationDate { get; set; }
        //[Display(Name = "End Of Probation Date")]
        public DateTime? EndofProbationDate { get; set; }
        //[Display(Name = "Confirmation Date")]
        public DateTime? ConfirmationDate { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        //[Display(Name = "Department")]
        //[Display(Name = "Employeement Type")]
        public int? EmployeeType { get; set; }
        public int? SubDepartmentId { get; set; }
        public SubDepartment? SubDepartment { get; set; }
        public int? RoleId { get; set; }
        public Roles? Role { get; set; }
        //[Display(Name = "SubDepartment")]
        public int? DesignationId { get; set; }
        public Designation? Designation { get; set; }
        public List<Head>? Heads { get; set; }
        public SubDepartment? SubDepartmentHead { get; set; }
        public ICollection<SubjectTeacherAllocation> SubjectTeacherAllocations { get; set; } = null!;
        public ICollection<STPlanApproval> STPlans { get; set; } = null!;
        public ICollection<Section> Sections { get; set; } = null!;
        public ICollection<Grade> Grades { get; set; } = null!;
    }
}
