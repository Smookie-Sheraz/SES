using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myWebApp.ViewModels.HumanResource;
using System.Security.Claims;

namespace myWebApp.Controllers
{
    [Authorize]
    public class HumanResourceController : Controller
    {
        private readonly IEFRepository _repository;
        private readonly SchoolDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HumanResourceController(IEFRepository repository, SchoolDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Employee
        [Authorize(Policy = "Employee.Read")]
        [HttpGet]
        public async Task<IActionResult> Employee()
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            List<UpdateEmployeeVM> employees = new List<UpdateEmployeeVM>();
            if (!User.IsInRole("Subject Teacher"))
            {
                if (User.IsInRole("Class Teacher"))
                {
                    employees = await (from a in _db.Employees
                                       join b in _db.Genders on a.GenderId equals b.GenderId into EmpGend
                                       from gender in EmpGend.DefaultIfEmpty()
                                       join c in _db.Schools on a.SchoolId equals c.SchoolId into EmpSchool
                                       from school in EmpSchool.DefaultIfEmpty()
                                       join d in _db.Campuses on a.CampusId equals d.CampusId into EmpCampus
                                       from campus in EmpCampus.DefaultIfEmpty()
                                       join e in _db.SchoolSections on a.SchoolSectionId equals e.SchoolSectionId into EmpSSection
                                       from section in EmpSSection.DefaultIfEmpty()
                                       join f in _db.Departments on a.DepartmentId equals f.DepartmentId into EmpDepartment
                                       from department in EmpDepartment.DefaultIfEmpty()
                                       join g in _db.subDepartments on a.SubDepartmentId equals g.SubDepartmentId into EmpSDepartment
                                       from sDepartment in EmpSDepartment.DefaultIfEmpty()
                                       join h in _db.Designations on a.DesignationId equals h.DesignationId into EmpDesignation
                                       from designation in EmpDesignation.DefaultIfEmpty()
                                       join i in _db.Roles on a.RoleId equals i.RoleId into empRole
                                       from role in empRole.DefaultIfEmpty()
                                       join j in _db.Grades on section.SchoolSectionId equals j.SchoolSectionId into EmpGrades
                                       from EGs in EmpGrades.DefaultIfEmpty()
                                       join k in _db.Sections on EGs.GradeId equals k.GradeId into CTClass
                                       from Class in CTClass.DefaultIfEmpty()
                                       where (role.RollName == "Assistant Teacher" || role.RollName == "Subject Teacher") && Class.ClassTeacherId == empId && a.IsActive == true
                                       //where b.GenderId == a.GenderId && c.SchoolId == a.SchoolId && d.CampusId == a.CampusId && e.SchoolSectionId == a.SchoolSectionId && f.DepartmentId == a.DepartmentId && g.SubDepartmentId == a.SubDepartmentId && h.DesignationId == a.DesignationId
                                       select new UpdateEmployeeVM
                                       {
                                           CampusName = campus.CampusName,
                                           SchoolName = school.SchoolName,
                                           GenderName = gender.WGender,
                                           Address = a.Address,
                                           CNICExpiryDate = a.CNICExpiryDate,
                                           CNICIssueDate = a.CNICIssueDate,
                                           CNIC = a.CNICNo,
                                           DOB = a.DOB,
                                           Email = a.Email,
                                           FatherName = a.FatherName,
                                           FName = a.FName,
                                           LName = a.LName,
                                           MaritalStatus = a.MaritalStatus,
                                           Mobile = a.Mobile,
                                           SpouseName = a.SpouseName,
                                           EmployeeId = a.EmployeeId,
                                           Role = role.RollName,
                                           SchoolSectionName = section.SectionName,
                                           ProbationPeriod = a.ProbationPeriod,
                                           StartofPeriodDate = a.StartofPeriodDate,
                                           EndofPeriodDate = a.EndofPeriodDate,
                                           StartofProbationDate = a.StartofProbationDate,
                                           ConfirmationDate = a.ConfirmationDate,
                                           DepartmentName = department.DepartmentName,
                                           SubDepartmentName = sDepartment.DepartmentName,
                                           DesignationName = designation.Name,
                                           EndofProbationDate = a.EndofProbationDate,
                                           FieldOfSpecialization = a.FieldOfSpecialization,
                                           EmployeeType = a.EmployeeType,
                                           JoiningDate = a.JoiningDate,
                                           Period = a.Period,
                                           UserImageUrl = a.UserImageUrl,
                                           IsActive = (bool)a.IsActive
                                       }).Distinct().ToListAsync();
                }
                else if (User.IsInRole("Grade Manager"))
                {
                    employees = await (from a in _db.Employees
                                       join b in _db.Genders on a.GenderId equals b.GenderId into EmpGend
                                       from gender in EmpGend.DefaultIfEmpty()
                                       join c in _db.Schools on a.SchoolId equals c.SchoolId into EmpSchool
                                       from school in EmpSchool.DefaultIfEmpty()
                                       join d in _db.Campuses on a.CampusId equals d.CampusId into EmpCampus
                                       from campus in EmpCampus.DefaultIfEmpty()
                                       join e in _db.SchoolSections on a.SchoolSectionId equals e.SchoolSectionId into EmpSSection
                                       from section in EmpSSection.DefaultIfEmpty()
                                       join f in _db.Departments on a.DepartmentId equals f.DepartmentId into EmpDepartment
                                       from department in EmpDepartment.DefaultIfEmpty()
                                       join g in _db.subDepartments on a.SubDepartmentId equals g.SubDepartmentId into EmpSDepartment
                                       from sDepartment in EmpSDepartment.DefaultIfEmpty()
                                       join h in _db.Designations on a.DesignationId equals h.DesignationId into EmpDesignation
                                       from designation in EmpDesignation.DefaultIfEmpty()
                                       join i in _db.Roles on a.RoleId equals i.RoleId into empRole
                                       from role in empRole.DefaultIfEmpty()
                                       join k in _db.Grades on section.SchoolSectionId equals k.SchoolSectionId into GMGrade
                                       from GM in GMGrade.DefaultIfEmpty()
                                       where (role.RollName == "Assistant Teacher" || role.RollName == "Subject Teacher" || role.RollName == "Class Teacher") && GM.GradeManagerId == empId && a.IsActive == true
                                       //where b.GenderId == a.GenderId && c.SchoolId == a.SchoolId && d.CampusId == a.CampusId && e.SchoolSectionId == a.SchoolSectionId && f.DepartmentId == a.DepartmentId && g.SubDepartmentId == a.SubDepartmentId && h.DesignationId == a.DesignationId
                                       select new UpdateEmployeeVM
                                       {
                                           CampusName = campus.CampusName,
                                           SchoolName = school.SchoolName,
                                           GenderName = gender.WGender,
                                           Address = a.Address,
                                           CNICExpiryDate = a.CNICExpiryDate,
                                           CNICIssueDate = a.CNICIssueDate,
                                           CNIC = a.CNICNo,
                                           DOB = a.DOB,
                                           Email = a.Email,
                                           FatherName = a.FatherName,
                                           FName = a.FName,
                                           LName = a.LName,
                                           MaritalStatus = a.MaritalStatus,
                                           Mobile = a.Mobile,
                                           SpouseName = a.SpouseName,
                                           EmployeeId = a.EmployeeId,
                                           Role = role.RollName,
                                           SchoolSectionName = section.SectionName,
                                           ProbationPeriod = a.ProbationPeriod,
                                           StartofPeriodDate = a.StartofPeriodDate,
                                           EndofPeriodDate = a.EndofPeriodDate,
                                           StartofProbationDate = a.StartofProbationDate,
                                           ConfirmationDate = a.ConfirmationDate,
                                           DepartmentName = department.DepartmentName,
                                           SubDepartmentName = sDepartment.DepartmentName,
                                           DesignationName = designation.Name,
                                           EndofProbationDate = a.EndofProbationDate,
                                           FieldOfSpecialization = a.FieldOfSpecialization,
                                           EmployeeType = a.EmployeeType,
                                           JoiningDate = a.JoiningDate,
                                           Period = a.Period,
                                           UserImageUrl = a.UserImageUrl,
                                           IsActive = (bool)a.IsActive
                                       }).Distinct().ToListAsync();
                }
                else if (User.IsInRole("Assistant Coordinator"))
                {
                    employees = await (from a in _db.Employees
                                       join b in _db.Genders on a.GenderId equals b.GenderId into EmpGend
                                       from gender in EmpGend.DefaultIfEmpty()
                                       join c in _db.Schools on a.SchoolId equals c.SchoolId into EmpSchool
                                       from school in EmpSchool.DefaultIfEmpty()
                                       join d in _db.Campuses on a.CampusId equals d.CampusId into EmpCampus
                                       from campus in EmpCampus.DefaultIfEmpty()
                                       join e in _db.SchoolSections on a.SchoolSectionId equals e.SchoolSectionId into EmpSSection
                                       from section in EmpSSection.DefaultIfEmpty()
                                       join f in _db.Departments on a.DepartmentId equals f.DepartmentId into EmpDepartment
                                       from department in EmpDepartment.DefaultIfEmpty()
                                       join g in _db.subDepartments on a.SubDepartmentId equals g.SubDepartmentId into EmpSDepartment
                                       from sDepartment in EmpSDepartment.DefaultIfEmpty()
                                       join h in _db.Designations on a.DesignationId equals h.DesignationId into EmpDesignation
                                       from designation in EmpDesignation.DefaultIfEmpty()
                                       join i in _db.Roles on a.RoleId equals i.RoleId into empRole
                                       from role in empRole.DefaultIfEmpty()
                                       where (role.RollName == "Assistant Teacher" || role.RollName == "Subject Teacher" || role.RollName == "Class Teacher" || role.RollName == "Grade Manager") && section.AssistantCoordinatorId == empId && a.IsActive == true
                                       //where b.GenderId == a.GenderId && c.SchoolId == a.SchoolId && d.CampusId == a.CampusId && e.SchoolSectionId == a.SchoolSectionId && f.DepartmentId == a.DepartmentId && g.SubDepartmentId == a.SubDepartmentId && h.DesignationId == a.DesignationId
                                       select new UpdateEmployeeVM
                                       {
                                           CampusName = campus.CampusName,
                                           SchoolName = school.SchoolName,
                                           GenderName = gender.WGender,
                                           Address = a.Address,
                                           CNICExpiryDate = a.CNICExpiryDate,
                                           CNICIssueDate = a.CNICIssueDate,
                                           CNIC = a.CNICNo,
                                           DOB = a.DOB,
                                           Email = a.Email,
                                           FatherName = a.FatherName,
                                           FName = a.FName,
                                           LName = a.LName,
                                           MaritalStatus = a.MaritalStatus,
                                           Mobile = a.Mobile,
                                           SpouseName = a.SpouseName,
                                           EmployeeId = a.EmployeeId,
                                           Role = role.RollName,
                                           SchoolSectionName = section.SectionName,
                                           ProbationPeriod = a.ProbationPeriod,
                                           StartofPeriodDate = a.StartofPeriodDate,
                                           EndofPeriodDate = a.EndofPeriodDate,
                                           StartofProbationDate = a.StartofProbationDate,
                                           ConfirmationDate = a.ConfirmationDate,
                                           DepartmentName = department.DepartmentName,
                                           SubDepartmentName = sDepartment.DepartmentName,
                                           DesignationName = designation.Name,
                                           EndofProbationDate = a.EndofProbationDate,
                                           FieldOfSpecialization = a.FieldOfSpecialization,
                                           EmployeeType = a.EmployeeType,
                                           JoiningDate = a.JoiningDate,
                                           Period = a.Period,
                                           UserImageUrl = a.UserImageUrl,
                                           IsActive = (bool)a.IsActive
                                       }).Distinct().ToListAsync();
                }
                else if(User.IsInRole("Deputy Coordinator") || User.IsInRole("Director Academics"))
                {
                    employees = await (from a in _db.Employees
                                       join b in _db.Genders on a.GenderId equals b.GenderId into EmpGend
                                       from gender in EmpGend.DefaultIfEmpty()
                                       join c in _db.Schools on a.SchoolId equals c.SchoolId into EmpSchool
                                       from school in EmpSchool.DefaultIfEmpty()
                                       join d in _db.Campuses on a.CampusId equals d.CampusId into EmpCampus
                                       from campus in EmpCampus.DefaultIfEmpty()
                                       join e in _db.SchoolSections on a.SchoolSectionId equals e.SchoolSectionId into EmpSSection
                                       from section in EmpSSection.DefaultIfEmpty()
                                       join f in _db.Departments on a.DepartmentId equals f.DepartmentId into EmpDepartment
                                       from department in EmpDepartment.DefaultIfEmpty()
                                       join g in _db.subDepartments on a.SubDepartmentId equals g.SubDepartmentId into EmpSDepartment
                                       from sDepartment in EmpSDepartment.DefaultIfEmpty()
                                       join h in _db.Designations on a.DesignationId equals h.DesignationId into EmpDesignation
                                       from designation in EmpDesignation.DefaultIfEmpty()
                                       join i in _db.Roles on a.RoleId equals i.RoleId into empRole
                                       from role in empRole.DefaultIfEmpty()
                                           //where b.GenderId == a.GenderId && c.SchoolId == a.SchoolId && d.CampusId == a.CampusId && e.SchoolSectionId == a.SchoolSectionId && f.DepartmentId == a.DepartmentId && g.SubDepartmentId == a.SubDepartmentId && h.DesignationId == a.DesignationId
                                       select new UpdateEmployeeVM
                                       {
                                           CampusName = campus.CampusName,
                                           SchoolName = school.SchoolName,
                                           GenderName = gender.WGender,
                                           Address = a.Address,
                                           CNICExpiryDate = a.CNICExpiryDate,
                                           CNICIssueDate = a.CNICIssueDate,
                                           CNIC = a.CNICNo,
                                           DOB = a.DOB,
                                           Email = a.Email,
                                           FatherName = a.FatherName,
                                           FName = a.FName,
                                           LName = a.LName,
                                           MaritalStatus = a.MaritalStatus,
                                           Mobile = a.Mobile,
                                           SpouseName = a.SpouseName,
                                           EmployeeId = a.EmployeeId,
                                           Role = role.RollName,
                                           SchoolSectionName = section.SectionName,
                                           ProbationPeriod = a.ProbationPeriod,
                                           StartofPeriodDate = a.StartofPeriodDate,
                                           EndofPeriodDate = a.EndofPeriodDate,
                                           StartofProbationDate = a.StartofProbationDate,
                                           ConfirmationDate = a.ConfirmationDate,
                                           DepartmentName = department.DepartmentName,
                                           SubDepartmentName = sDepartment.DepartmentName,
                                           DesignationName = designation.Name,
                                           EndofProbationDate = a.EndofProbationDate,
                                           FieldOfSpecialization = a.FieldOfSpecialization,
                                           EmployeeType = a.EmployeeType,
                                           JoiningDate = a.JoiningDate,
                                           Period = a.Period,
                                           UserImageUrl = a.UserImageUrl,
                                           IsActive = (bool)a.IsActive
                                       }).Distinct().ToListAsync();
                }
            }
            //employee.AddRange(employees.Select(x => new UpdateEmployeeVM { Role = x.Role, IsActive = x.IsActive, UserImageUrl = x.UserImageUrl, Address = x.Address, CampusName = x.CampusName, GenderName = x.GenderName, FName = x.FName, FatherName = x.FatherName, Email = x.Email, CNIC = x.CNIC, DOB = x.DOB, CNICExpiryDate = x.CNICExpiryDate, CNICIssueDate = x.CNICIssueDate, LName = x.LName, SchoolName = x.SchoolName, SpouseName = x.SpouseName, MaritalStatus = x.MaritalStatus, Mobile = x.Mobile, EmployeeId = x.EmployeeId, Period = x.Period, JoiningDate = x.JoiningDate, EmployeeType = x.EmployeeType, FieldOfSpecialization = x.FieldOfSpecialization, EndofProbationDate = x.EndofProbationDate, ConfirmationDate = x.ConfirmationDate, DepartmentName = x.DepartmentName, DesignationName = x.DesignationName, EndofPeriodDate = x.EndofPeriodDate, ProbationPeriod = x.ProbationPeriod, SchoolSectionName = x.SchoolSectionName, StartofPeriodDate = x.StartofPeriodDate, StartofProbationDate = x.StartofProbationDate, SubDepartmentName = x.SubDepartmentName }).ToList());
            return View(employees);
        }
        [Authorize(Policy = "Employee.Create")]
        [HttpGet]
        public async Task<IActionResult> AddEmployee()
        {
            ViewBag.Departments = await _repository.GetDepartments();
            ViewBag.SubDepartments = await _repository.GetSubDepartments();
            ViewBag.Gender = await _repository.GetGenders();
            ViewBag.Designations = await _repository.GetDesignations();
            ViewBag.Schools = await _db.Schools.ToListAsync();
            ViewBag.Campuses = await _db.Campuses.ToListAsync();
            ViewBag.SchoolSections = await _db.SchoolSections.Where(x => x.IsActive == true).ToListAsync();
            ViewBag.Roles = await _db.Roles.Where(x => x.IsActive == true).ToListAsync();
            return View();
        }
        [Authorize(Policy = "Employee.Create")]
        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeVM employee)
        {
            string UserImage = employee.UserImage == null ? null : FileSaver(employee.FName, employee.LName, employee.UserImage);
            var newEmployee = new Employee
            {
                //EmployeeCode = employee.EmployeeCode,
                FName = employee.FName,
                LName = employee.LName,
                FatherName = employee.FatherName,
                SpouseName = employee.SpouseName,
                GenderId = employee.GenderId,
                MaritalStatus = employee.MaritalStatus,
                Mobile = employee.Mobile,
                DOB = employee.DOB,
                CNICNo = employee.CNIC,
                CNICIssueDate = employee.CNICIssueDate,
                CNICExpiryDate = employee.CNICExpiryDate,
                Email = employee.Email,
                Address = employee.Address,
                JoiningDate = employee.JoiningDate,
                ProbationPeriod = employee.ProbationPeriod,
                StartofProbationDate = employee.StartofProbationDate,
                EndofProbationDate = employee.EndofProbationDate,
                ConfirmationDate = employee.ConfirmationDate,
                DepartmentId = employee.DepartmentId,
                DesignationId = employee.DesignationId,
                SubDepartmentId = employee.SubDepartmentId,
                EmployeeType = employee.EmployeeType,
                FieldOfSpecialization = employee.FieldOfSpecialization,
                Period = employee.Period,
                EndofPeriodDate = employee.EndofPeriodDate,
                StartofPeriodDate = employee.StartofPeriodDate,
                SchoolId = employee.SchoolId,
                CampusId = employee.CampusId,
                SchoolSectionId = employee.SchoolSectionId,
                RoleId = employee.RoleId,
                UserImageUrl = UserImage
            };
            await _repository.AddAsync(newEmployee);
            if (await _repository.SaveChanges())
            {
                var RecentEmployee = await _db.Employees.OrderBy(x => x.EmployeeId).LastOrDefaultAsync();
                var ifUserExists = await _db.Users.Where(x => x.Email == RecentEmployee.Email && x.IsActive == true).FirstOrDefaultAsync();
                if (ifUserExists == null)
                {
                    var user = new Entities.Models.User
                    {
                        FName = RecentEmployee.FName,
                        LName = RecentEmployee.LName,
                        FatherName = RecentEmployee.FatherName,
                        Email = RecentEmployee.Email,
                        Password = RecentEmployee.Email,
                        UserName = RecentEmployee.FName + RecentEmployee.LName
                    };
                    await _repository.AddAsync(user);
                    if (await _repository.SaveChanges())
                    {
                        return RedirectToAction("Employee");
                    }
                    ModelState.AddModelError("", "Error While Saving to Database");
                }
                else
                {
                    return RedirectToAction("Employee");
                }
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(employee);
        }
        [Authorize(Policy = "Employee.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(int id)
        {
            ViewBag.Departments = await _db.Departments.ToListAsync();
            ViewBag.SubDepartments = await _repository.GetSubDepartments();
            ViewBag.Gender = await _repository.GetGenders();
            ViewBag.Designations = await _repository.GetDesignations();
            ViewBag.Schools = await _db.Schools.ToListAsync();
            ViewBag.Campuses = await _db.Campuses.ToListAsync();
            ViewBag.SchoolSections = await _db.SchoolSections.Where(x => x.IsActive == true).ToListAsync();
            ViewBag.Roles = await _db.Roles.Where(x => x.IsActive == true).ToListAsync();
            var shoora = await _repository.GetEmployeeById(id);
            if (shoora == null)
            {
                return NotFound();
            }
            var employee = new UpdateEmployeeVM
            {
                EmployeeId = shoora.EmployeeId,
                EmployeeCode = shoora.EmployeeCode,
                FName = shoora.FName,
                LName = shoora.LName,
                FatherName = shoora.FatherName,
                SpouseName = shoora.SpouseName,
                GenderId = shoora.GenderId,
                MaritalStatus = shoora.MaritalStatus,
                Mobile = shoora.Mobile,
                DOB = shoora.DOB,
                CNIC = shoora.CNICNo,
                CNICIssueDate = shoora.CNICIssueDate,
                CNICExpiryDate = shoora.CNICExpiryDate,
                Email = shoora.Email,
                Address = shoora.Address,
                JoiningDate = shoora.JoiningDate,
                ProbationPeriod = shoora.ProbationPeriod,
                StartofProbationDate = shoora.StartofProbationDate,
                EndofProbationDate = shoora.EndofProbationDate,
                ConfirmationDate = shoora.ConfirmationDate,
                DepartmentId = (int)shoora.DepartmentId,
                DesignationId = shoora.DesignationId,
                SubDepartmentId = shoora.SubDepartmentId,
                EmployeeType = shoora.EmployeeType,
                FieldOfSpecialization = shoora.FieldOfSpecialization,
                Period = shoora.Period,
                EndofPeriodDate = shoora.EndofPeriodDate,
                StartofPeriodDate = shoora.StartofPeriodDate,
                SchoolId = (int)shoora.SchoolId,
                CampusId = (int)shoora.CampusId,
                SchoolSectionId = (int)shoora.SchoolSectionId,
                RoleId = shoora.RoleId,
                UserImageUrl = shoora.UserImageUrl,
                IsActive = (bool)shoora.IsActive
            };
            return View(employee);
        }
        [Authorize(Policy = "Employee.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeVM employee)
        {
            var shoora = await _repository.GetEmployeeById(employee.EmployeeId);
            var user = await _db.Users.Where(x => x.Email == shoora.Email).FirstOrDefaultAsync();
            if (shoora == null)
            {
                return NotFound();
            }
            shoora.EmployeeCode = employee.EmployeeCode;
            shoora.FName = employee.FName;
            shoora.LName = employee.LName;
            shoora.FatherName = employee.FatherName;
            shoora.SpouseName = employee.SpouseName;
            shoora.GenderId = employee.GenderId;
            shoora.MaritalStatus = employee.MaritalStatus;
            shoora.Mobile = employee.Mobile;
            shoora.DOB = employee.DOB;
            shoora.CNICNo = employee.CNIC;
            shoora.CNICIssueDate = employee.CNICIssueDate;
            shoora.CNICExpiryDate = employee.CNICExpiryDate;
            shoora.Email = employee.Email;
            shoora.Address = employee.Address;
            shoora.JoiningDate = employee.JoiningDate;
            shoora.ProbationPeriod = employee.ProbationPeriod;
            shoora.StartofProbationDate = employee.StartofProbationDate;
            shoora.EndofProbationDate = employee.EndofProbationDate;
            shoora.ConfirmationDate = employee.ConfirmationDate;
            shoora.DepartmentId = employee.DepartmentId;
            shoora.DesignationId = employee.DesignationId;
            shoora.SubDepartmentId = employee.SubDepartmentId;
            shoora.EmployeeType = employee.EmployeeType;
            shoora.FieldOfSpecialization = employee.FieldOfSpecialization;
            shoora.Period = employee.Period;
            shoora.EndofPeriodDate = employee.EndofPeriodDate;
            shoora.StartofPeriodDate = employee.StartofPeriodDate;
            shoora.SchoolId = employee.SchoolId;
            shoora.CampusId = employee.CampusId;
            shoora.SchoolSectionId = employee.SchoolSectionId;
            shoora.RoleId = employee.RoleId;
            shoora.IsActive = employee.IsActive;
            if (employee.UserImage != null)
            {
                if (employee.UserImageUrl != null)
                {
                    var oldImage = new FileInfo(@"C:\\Ultra_Pro\\.Net Learning\\Advanced C#\\webApp\\myWebApp\\wwwroot\\UsersImages\\" + employee.UserImageUrl);
                    oldImage.Delete();
                }
                shoora.UserImageUrl = FileSaver(employee.FName, employee.LName, employee.UserImage);
            }
            user.FName = shoora.FName;
            user.LName = shoora.LName;
            user.FatherName = shoora.FatherName;
            user.Email = shoora.Email;
            user.UserName = shoora.FName + shoora.LName;
            if (user.Password == user.Email)
            {
                user.Password = shoora.Email;
            }
            await _repository.UpdateAsync(shoora);
            await _repository.UpdateAsync(user);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Employee");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(employee);
        }
        [Authorize(Policy = "Employee.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var shoora = await _repository.GetEmployeeById(id);
            var user = await _db.Users.Where(x => x.Email == shoora.Email).FirstOrDefaultAsync();
            if (shoora == null)
            {
                return NotFound();
            }
            shoora.IsActive = false;
            await _repository.UpdateAsync(shoora);
            user.IsActive = false;
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Employee");
            }
            return RedirectToAction("Employee");
        }
        [Authorize(Policy = "EmployeeDetails.Read")]
        [HttpGet]
        public async Task<IActionResult> EmployeeDetails(int Id)
        {
            UpdateEmployeeVM employee = new UpdateEmployeeVM();
            var shoramembers = from a in _db.Employees
                               join b in _db.Genders on a.GenderId equals b.GenderId into EmpGend
                               from gender in EmpGend.DefaultIfEmpty()
                               join c in _db.Schools on a.SchoolId equals c.SchoolId into EmpSchool
                               from school in EmpSchool.DefaultIfEmpty()
                               join d in _db.Campuses on a.CampusId equals d.CampusId into EmpCampus
                               from campus in EmpCampus.DefaultIfEmpty()
                               join e in _db.SchoolSections on a.SchoolSectionId equals e.SchoolSectionId into EmpSSection
                               from section in EmpSSection.DefaultIfEmpty()
                               join f in _db.Departments on a.DepartmentId equals f.DepartmentId into EmpDepartment
                               from department in EmpDepartment.DefaultIfEmpty()
                               join g in _db.subDepartments on a.SubDepartmentId equals g.SubDepartmentId into EmpSDepartment
                               from sDepartment in EmpSDepartment.DefaultIfEmpty()
                               join h in _db.Designations on a.DesignationId equals h.DesignationId into EmpDesignation
                               from designation in EmpDesignation.DefaultIfEmpty()
                               where a.EmployeeId == Id
                               select new UpdateEmployeeVM
                               {
                                   CampusName = campus.CampusName,
                                   SchoolName = school.SchoolName,
                                   GenderName = gender.WGender,
                                   Address = a.Address,
                                   CNICExpiryDate = a.CNICExpiryDate,
                                   CNICIssueDate = a.CNICIssueDate,
                                   CNIC = a.CNICNo,
                                   DOB = a.DOB,
                                   Email = a.Email,
                                   FatherName = a.FatherName,
                                   FName = a.FName,
                                   LName = a.LName,
                                   MaritalStatus = a.MaritalStatus,
                                   Mobile = a.Mobile,
                                   SpouseName = a.SpouseName,
                                   EmployeeId = a.EmployeeId,
                                   SchoolSectionName = section.SectionName,
                                   ProbationPeriod = a.ProbationPeriod,
                                   StartofPeriodDate = a.StartofPeriodDate,
                                   EndofPeriodDate = a.EndofPeriodDate,
                                   StartofProbationDate = a.StartofProbationDate,
                                   ConfirmationDate = a.ConfirmationDate,
                                   DepartmentName = department.DepartmentName,
                                   SubDepartmentName = sDepartment.DepartmentName,
                                   DesignationName = designation.Name,
                                   EndofProbationDate = a.EndofProbationDate,
                                   FieldOfSpecialization = a.FieldOfSpecialization,
                                   EmployeeType = a.EmployeeType,
                                   JoiningDate = a.JoiningDate,
                                   Period = a.Period
                               };

            employee = await shoramembers.Select(x => new UpdateEmployeeVM { Address = x.Address, CampusName = x.CampusName, GenderName = x.GenderName, FName = x.FName, FatherName = x.FatherName, Email = x.Email, CNIC = x.CNIC, DOB = x.DOB, CNICExpiryDate = x.CNICExpiryDate, CNICIssueDate = x.CNICIssueDate, LName = x.LName, SchoolName = x.SchoolName, SpouseName = x.SpouseName, MaritalStatus = x.MaritalStatus, Mobile = x.Mobile, EmployeeId = x.EmployeeId, Period = x.Period, JoiningDate = x.JoiningDate, EmployeeType = x.EmployeeType, FieldOfSpecialization = x.FieldOfSpecialization, EndofProbationDate = x.EndofProbationDate, ConfirmationDate = x.ConfirmationDate, DepartmentName = x.DepartmentName, DesignationName = x.DesignationName, EndofPeriodDate = x.EndofPeriodDate, ProbationPeriod = x.ProbationPeriod, SchoolSectionName = x.SchoolSectionName, StartofPeriodDate = x.StartofPeriodDate, StartofProbationDate = x.StartofProbationDate, SubDepartmentName = x.SubDepartmentName }).FirstOrDefaultAsync();

            return View(employee);
        }
        public string FileSaver(string FName, string LName, IFormFile UserImage)
        {
            var fileInfo = new FileInfo(UserImage.FileName);
            string ImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UsersImages/");
            if (!Directory.Exists(ImagePath)) Directory.CreateDirectory(ImagePath);
            string ImageName = FName + LName + fileInfo.Extension;
            string imageNameWithPath = Path.Combine(ImagePath, ImageName);
            using (var stream = new FileStream(imageNameWithPath, FileMode.Create))
            {
                UserImage.CopyTo(stream);
            }
            return ImageName;
        }
        #endregion

        #region Shoora
        [Authorize(Policy = "Shoora.Read")]
        [HttpGet]
        public async Task<IActionResult> Shoora()
        {
            List<UpdateShooraVM> shoora = new List<UpdateShooraVM>();
            var shoramember = from a in _db.Shoora
                              join gen in _db.Genders on a.GenderId equals gen.GenderId into gender
                              from b in gender.DefaultIfEmpty()
                              join scl in _db.Schools on a.SchoolId equals scl.SchoolId into shura
                              from c in shura.DefaultIfEmpty()
                              join cmps in _db.Campuses on a.CampdusId equals cmps.CampusId into campus
                              from d in campus.DefaultIfEmpty()
                              select new UpdateShooraVM
                              {
                                  CampusName = d.CampusName,
                                  SchoolName = c.SchoolName,
                                  GenderName = b.WGender,
                                  Address = a.Address,
                                  City = a.City,
                                  CNICExpiryDate = a.CNICExpiryDate,
                                  CNICIssueDate = a.CNICIssueDate,
                                  CNICNo = a.CNICNo,
                                  DOB = a.DOB,
                                  Email = a.Email,
                                  FatherName = a.FatherName,
                                  FName = a.FName,
                                  LName = a.LName,
                                  MaritalStatus = a.MaritalStatus,
                                  Mobile = a.Mobile,
                                  SpouseName = a.SpouseName,
                                  ShooraId = a.ShooraId,
                                  IsActive = (bool)a.IsActive
                              };
            shoora.AddRange(await shoramember.Select(x => new UpdateShooraVM { IsActive = x.IsActive, Address = x.Address, CampusName = x.CampusName, GenderName = x.GenderName, FName = x.FName, FatherName = x.FatherName, Email = x.Email, CNICNo = x.CNICNo, DOB = x.DOB, City = x.City, CNICExpiryDate = x.CNICExpiryDate, CNICIssueDate = x.CNICIssueDate, LName = x.LName, SchoolName = x.SchoolName, SpouseName = x.SpouseName, MaritalStatus = x.MaritalStatus, Mobile = x.Mobile, ShooraId = x.ShooraId }).ToListAsync());

            return View(shoora);
        }
        [Authorize(Policy = "Shoora.Create")]
        [HttpGet]
        public async Task<IActionResult> AddShoora()
        {
            ViewBag.Gender = await _repository.GetGenders();
            ViewBag.Schools = await _db.Schools.ToListAsync();
            ViewBag.Campuses = await _db.Campuses.ToListAsync();
            return View();
        }
        [Authorize(Policy = "Shoora.Create")]
        [HttpPost]
        public async Task<IActionResult> Addshoora(ShooraVM shoora)
        {
            var newShoora = new Shoora
            {
                FName = shoora.FName,
                LName = shoora.LName,
                FatherName = shoora.FatherName,
                SpouseName = shoora.SpouseName,
                GenderId = shoora.GenderId,
                MaritalStatus = shoora.MaritalStatus,
                Mobile = shoora.Mobile,
                DOB = shoora.DOB,
                CNICNo = shoora.CNICNo,
                CNICIssueDate = shoora.CNICIssueDate,
                CNICExpiryDate = shoora.CNICExpiryDate,
                Email = shoora.Email,
                Address = shoora.Address,
                City = shoora.City,
                CampdusId = shoora.CampusId,
                SchoolId = shoora.SchoolId
            };
            await _repository.AddAsync(newShoora);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Shoora");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(shoora);
        }
        [Authorize(Policy = "Shoora.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateShoora(int id)
        {
            ViewBag.Gender = await _repository.GetGenders();
            ViewBag.Schools = await _db.Schools.ToListAsync();
            ViewBag.Campuses = await _db.Campuses.ToListAsync();
            var shoora = await _repository.GetShooraById(id);
            if (shoora == null)
            {
                return NotFound();
            }
            var NewShoora = new UpdateShooraVM
            {
                ShooraId = shoora.ShooraId,
                FName = shoora.FName,
                LName = shoora.LName,
                FatherName = shoora.FatherName,
                SpouseName = shoora.SpouseName,
                GenderId = shoora.GenderId,
                MaritalStatus = shoora.MaritalStatus,
                Mobile = shoora.Mobile,
                DOB = shoora.DOB,
                CNICNo = shoora.CNICNo,
                CNICIssueDate = shoora.CNICIssueDate,
                CNICExpiryDate = shoora.CNICExpiryDate,
                Email = shoora.Email,
                Address = shoora.Address,
                City = shoora.City,
                CampusId = shoora.CampdusId,
                SchoolId = shoora.SchoolId,
                IsActive = (bool)shoora.IsActive
            };
            return View(NewShoora);
        }
        [Authorize(Policy = "Shoora.Update")]
        [HttpPost]
        public async Task<IActionResult> Updateshoora(UpdateShooraVM shoora)
        {
            var temp = await _repository.GetShooraById(shoora.ShooraId);
            if (shoora == null)
            {
                return NotFound();
            }
            temp.ShooraId = shoora.ShooraId;
            temp.FName = shoora.FName;
            temp.LName = shoora.LName;
            temp.FatherName = shoora.FatherName;
            temp.SpouseName = shoora.SpouseName;
            temp.GenderId = shoora.GenderId;
            temp.MaritalStatus = shoora.MaritalStatus;
            temp.Mobile = shoora.Mobile;
            temp.DOB = shoora.DOB;
            temp.CNICNo = shoora.CNICNo;
            temp.CNICIssueDate = shoora.CNICIssueDate;
            temp.CNICExpiryDate = shoora.CNICExpiryDate;
            temp.Email = shoora.Email;
            temp.Address = shoora.Address;
            temp.City = shoora.City;
            temp.CampdusId = shoora.CampusId;
            temp.SchoolId = shoora.SchoolId;
            temp.IsActive = (bool)shoora.IsActive;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Shoora");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(shoora);
        }
        [Authorize(Policy = "Shoora.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteShoora(int id)
        {
            var shoora = await _repository.GetShooraById(id);
            if (shoora == null)
            {
                return NotFound();
            }
            shoora.IsActive = false;
            await _repository.UpdateAsync(shoora);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Shoora");
            }
            return RedirectToAction("Shoora");
        }
        #endregion
    }
}
