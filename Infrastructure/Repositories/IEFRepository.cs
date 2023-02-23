using Entities.Models;

namespace Infrastructure.Repositories
{
    public interface IEFRepository
    {
        Task AddAsync(object entity);
        Task AddRangeAsync(object entity);
        Task Delete(object entity);
        Task<BookAllocation> GetBookAllocationById(int id);
        Task<IEnumerable<BookAllocation>> GetBookAllocations();
        Task<Book> GetBookById(int id);
        Task<IEnumerable<Book>> GetBooks();
        Task<Department> GetDepartmentById(int id);
        Task<IEnumerable<Department>> GetDepartments();
        Task<Designation> GetDesignationById(int id);
        Task<IEnumerable<Designation>> GetDesignations();
        Task<Employee> GetEmployeeById(int id);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<Gender> GetGenderById(int id);
        Task<IEnumerable<Gender>> GetGenders();
        Task<Grade> GetGradeById(int id);
        Task<IEnumerable<Grade>> GetGrades();
        Task<Head> GetHeadById(int id);
        Task<IEnumerable<Head>> GetHeads();
        //Task<Month> GetMonthById(int id);
        //Task<IEnumerable<Month>> GetMonths();
        Task<Section> GetSectionById(int id);
        Task<IEnumerable<Section>> GetSections();
        Task<Shoora> GetShooraById(int id);
        Task<IEnumerable<Shoora>> GetShooras();
        Task<SubDepartment> GetSubDepartmentById(int id);
        Task<IEnumerable<SubDepartment>> GetSubDepartments();
        Task<Subject> GetSubjectById(int id);
        Task<IEnumerable<Subject>> GetSubjects();
        Task<Term> GetTermById(int id);
        Task<IEnumerable<Term>> GetTerms();
        Task<User> GetUserByEMail(string email);
        Task<User> GetUserById(int id);
        Task<User> GetUserByUserName(string UserName);
        Task<IEnumerable<User>> GetUsers();
        Task<Year> GetYearById(int id);
        Task<IEnumerable<Year>> GetYears();
        Task<bool> SaveChanges();
        Task UpdateAsync(object entity);
    }
}