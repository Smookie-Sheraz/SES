namespace Entities.Models
{
    public class Gender
    {
        public int GenderId { get; set; }
        public string? WGender { get; set; }
        public bool? IsActive { get; set; }
        public List<Shoora> Shooras { get; set; }
        public List<Employee> Employees { get; set; }
    }
}