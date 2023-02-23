using Entities.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Infrastructure.Repositories
{
    public class EFRepository : IEFRepository
    {
        private readonly SchoolDbContext _context;

        public EFRepository(SchoolDbContext context)
        {
            _context = context;
        }
        #region Generic-Methods
        public async Task AddAsync(object entity)
        {
            await _context.AddAsync(entity);
        }
        public async Task AddRangeAsync(object entity)
        {
            await _context.AddRangeAsync(entity);
        }
        public Task UpdateAsync(object entity)
        {
            _context.Update(entity);
            return Task.CompletedTask;
        }
        public Task Delete(object entity)
        {
            _context.Remove(entity);
            return Task.CompletedTask;
        }
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion

        #region Setup
        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }
        public async Task<Department> GetDepartmentById(int id)
        {
            return await _context.Departments.FirstOrDefaultAsync(p => p.DepartmentId == id);
        }
        public async Task<IEnumerable<Designation>> GetDesignations()
        {
            return await _context.Designations.ToListAsync();
        }
        public async Task<Designation> GetDesignationById(int id)
        {
            return await _context.Designations.FirstOrDefaultAsync(p => p.DesignationId == id);
        }
        public async Task<IEnumerable<Gender>> GetGenders()
        {
            return await _context.Genders.ToListAsync();
        }
        public async Task<Gender> GetGenderById(int id)
        {
            return await _context.Genders.FirstOrDefaultAsync(p => p.GenderId == id);
        }
        public async Task<IEnumerable<Head>> GetHeads()
        {
            return await _context.Heads.ToListAsync();
        }
        public async Task<Head> GetHeadById(int id)
        {
            return await _context.Heads.FirstOrDefaultAsync(p => p.HeadId == id);
        }
        public async Task<IEnumerable<SubDepartment>> GetSubDepartments()
        {
            return await _context.subDepartments.ToListAsync();
        }
        public async Task<SubDepartment> GetSubDepartmentById(int id)
        {
            return await _context.subDepartments.FirstOrDefaultAsync(p => p.SubDepartmentId == id);
        }
        #endregion

        #region HumanResourse
        public async Task<IEnumerable<Shoora>> GetShooras()
        {
            return await _context.Shoora.ToListAsync();
        }
        public async Task<Shoora> GetShooraById(int id)
        {
            return await _context.Shoora.FirstOrDefaultAsync(p => p.ShooraId == id);
        }
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }
        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(p => p.EmployeeId == id);
        }
        #endregion

        #region Auth

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.UserId == id);
        }
        public async Task<User> GetUserByEMail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
        }
        public async Task<User> GetUserByUserName(string UserName)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.UserName == UserName);
        }

        #endregion

        #region Grade
        public async Task<IEnumerable<Subject>> GetSubjects()
        {
            return await _context.Subjects.ToListAsync();
        }
        public async Task<Subject> GetSubjectById(int id)
        {
            return await _context.Subjects.FirstOrDefaultAsync(p => p.SubjectId == id);
        }
        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }
        public async Task<Book> GetBookById(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(p => p.BookId == id);
        }
        public async Task<IEnumerable<Grade>> GetGrades()
        {
            return await _context.Grades.ToListAsync();
        }
        public async Task<Grade> GetGradeById(int id)
        {
            return await _context.Grades.FirstOrDefaultAsync(p => p.GradeId == id);
        }
        public async Task<IEnumerable<Entities.Models.Section>> GetSections()
        {
            return await _context.Sections.ToListAsync();
        }
        public async Task<Entities.Models.Section> GetSectionById(int id)
        {
            return await _context.Sections.FirstOrDefaultAsync(p => p.SectionId == id);
        }
        public async Task<IEnumerable<BookAllocation>> GetBookAllocations()
        {
            return await _context.bookAllocations.ToListAsync();
        }
        public async Task<BookAllocation> GetBookAllocationById(int id)
        {
            return await _context.bookAllocations.FirstOrDefaultAsync(p => p.BookAllocationId == id);
        }
        #endregion

        #region Academic-Calender
        public async Task<IEnumerable<Year>> GetYears()
        {
            return await _context.years.ToListAsync();
        }
        public async Task<Year> GetYearById(int id)
        {
            return await _context.years.FirstOrDefaultAsync(p => p.YearId == id);
        }
        public async Task<IEnumerable<Term>> GetTerms()
        {
            return await _context.terms.ToListAsync();
        }
        public async Task<Term> GetTermById(int id)
        {
            return await _context.terms.FirstOrDefaultAsync(p => p.TermId == id);
        }
        //public async Task<IEnumerable<Month>> GetMonths()
        //{
        //    return await _context.months.ToListAsync();
        //}
        //public async Task<Month> GetMonthById(int id)
        //{
        //    return await _context.months.FirstOrDefaultAsync(p => p.MonthId == id);
        //}
        #endregion

        //        #region SetupController-Methods
        //        public async Task<IEnumerable<Department>> GetDepartments()
        //        {
        //            return await _context.Departments.ToListAsync();
        //        }
        //        public async Task<Department> GetDepartmentById(int id)
        //        {
        //            return await _context.Departments.FirstOrDefaultAsync(p => p.DepartmentId == id);
        //        }
        //        public async Task<IEnumerable<Designation>> GetDesignations()
        //        {
        //            return await _context.Designations.ToListAsync();
        //        }
        //        public async Task<Designation> GetDesignationById(int id)
        //        {
        //            return await _context.Designations.FirstOrDefaultAsync(p => p.DesignationId == id);
        //        }
        //        public async Task<IEnumerable<EmployeeType>> GetEmployeeTypes()
        //        {
        //            return await _context.EmployeeTypes.ToListAsync();
        //        }
        //        public async Task<EmployeeType> GetEmployeeTypeById(int id)
        //        {
        //            return await _context.EmployeeTypes.FirstOrDefaultAsync(p => p.EmployeeTypeId == id);
        //        }
        //        public async Task<IEnumerable<Grade>> GetGrades()
        //        {
        //            return await _context.Grades.ToListAsync();
        //        }
        //        public async Task<Grade> GetGradeById(int id)
        //        {
        //            return await _context.Grades.FirstOrDefaultAsync(p => p.GradeId == id);
        //        }
        //        public async Task<IEnumerable<Location>> GetLocations()
        //        {
        //            return await _context.Locations.ToListAsync();
        //        }
        //        public async Task<Location> GetLocationById(int id)
        //        {
        //            return await _context.Locations.FirstOrDefaultAsync(p => p.LocationId == id);
        //        }
        //        public async Task<IEnumerable<Project>> GetProjects()
        //        {
        //            return await _context.Projects.ToListAsync();
        //        }
        //        public async Task<Project> GetProjectById(int id)
        //        {
        //            return await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        //        }
        //        public async Task<IEnumerable<Section>> GetSections()
        //        {
        //            return await _context.Sections.ToListAsync();
        //        }
        //        public async Task<Section> GetSectionById(int id)
        //        {
        //            return await _context.Sections.FirstOrDefaultAsync(p => p.SectionId == id);
        //        }
        //        public async Task<IEnumerable<Shift>> GetShifts()
        //        {
        //            return await _context.Shifts.ToListAsync();
        //        }
        //        public async Task<Shift> GetShiftById(int id)
        //        {
        //            return await _context.Shifts.FirstOrDefaultAsync(p => p.ShiftId == id);
        //        }
        //        #endregion

        //        #region SchoolController-Methods
        //        public async Task<IEnumerable<School>> GetSchools()
        //        {
        //            return await _context.Schools.ToListAsync();
        //        }
        //        public async Task<School> GetSchoolById(int id)
        //        {
        //            return await _context.Schools.FirstOrDefaultAsync(p => p.SchoolId == id);
        //        }
        //        public async Task<IEnumerable<Campus>> GetCampuses()
        //        {
        //            return await _context.Campuses.ToListAsync();
        //        }
        //        public async Task<Campus> GetCampusById(int id)
        //        {
        //            return await _context.Campuses.FirstOrDefaultAsync(p => p.CampusId == id);
        //        }
        //        public async Task<IEnumerable<GradeGroup>> GetGradeGroups()
        //        {
        //            return await _context.GradeGroups.ToListAsync();
        //        }
        //        public async Task<GradeGroup> GetGradeGroupById(int id)
        //        {
        //            return await _context.GradeGroups.FirstOrDefaultAsync(p => p.GradeGroupId == id);
        //        }
        //        public async Task<IEnumerable<Grades>> GetClassGrades()
        //        {
        //            return await _context.ClassGrades.ToListAsync();
        //        }
        //        public async Task<Grades> GetClassGradeById(int id)
        //        {
        //            return await _context.ClassGrades.FirstOrDefaultAsync(p => p.GradeId == id);
        //        }
        //        public async Task<IEnumerable<GradeSection>> GetGradeSections()
        //        {
        //            return await _context.GradeSections.ToListAsync();
        //        }
        //        public async Task<GradeSection> GetGradeSectionById(int id)
        //        {
        //            return await _context.GradeSections.FirstOrDefaultAsync(p => p.GradesSectionId == id);
        //        }

        //        //public async Task<IEnumerable<GradeGroup>> GetGradeGroups()
        //        //{
        //        //    return await _context.grades.ToListAsync();
        //        //}
        //        //public async Task<Grade> GetGradesById(int id)
        //        //{
        //        //    return await _context.Grades.FirstOrDefaultAsync(p => p.GradeId == id);
        //        //}
        //        #endregion

        //        #region PlacementController-Methods
        //        public async Task<IEnumerable<Placement>> GetPlacements()
        //        {
        //            return await _context.Placements.ToListAsync();
        //        }
        //        public async Task<Placement> GetPlacementById(int id)
        //        {
        //            return await _context.Placements.FirstOrDefaultAsync(p => p.PlacementId == id);
        //        }

        //        #endregion

        //        #region EmployeeController-Methods

        //        public async Task<IEnumerable<Employee>> GetEmployees()
        //        {
        //            return await _context.Employees.ToListAsync();
        //        }
        //        public async Task<Employee> GetEmployeeById(int id)
        //        {
        //            return await _context.Employees.FirstOrDefaultAsync(p => p.EmployeeId == id);
        //        }
        //        #endregion
    }
}
