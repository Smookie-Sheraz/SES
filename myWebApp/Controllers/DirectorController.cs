using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Entities.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using myWebApp.ViewModels.Director;
using myWebApp.ViewModels.BookAllocation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using myWebApp.ViewModels.Auth;
using System.Security.Claims;

namespace Entities.Controllers
{
    [Authorize]
    public class DirectorController : Controller
    {

        //        public DirectorController(IEFRepository repository)
        //        {
        //            _repository = repository;
        //        }
        private readonly IEFRepository _repository;
        private readonly SchoolDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DirectorController(SchoolDbContext db, IEFRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var user = User.FindFirst(ClaimTypes.Sid)?.Value;
            int uIdint = Convert.ToInt32(user);
            var emp = _db.Employees.Where(x => x.EmployeeId == uIdint).FirstOrDefault();
            ViewBag.UserName =  emp?.FName + " " + emp?.LName;
            return View();
        }

        #region Comment
        //        public IActionResult Teacher()
        //        {
        //            return View();
        //        }
        //        public async Task<IActionResult> Employee()
        //        {
        //            var result = await _repository.GetEmployees();
        //            return View(result);
        //        }
        //        public async Task<IActionResult> AddEmployee()
        //        {
        //            ViewBag.Placements = await _repository.GetPlacements();
        //            return View();
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> AddEmployee(AddEmployee employee)
        //        {

        //            var newEmployee = new Employee
        //            {
        //                EmployeeCode = employee.EmployeeCode,
        //                FName = employee.FName,
        //                LName = employee.LName,
        //                FatherName = employee.FatherName,
        //                SpouseName = employee.SpouseName,
        //                Gender = employee.Gender,
        //                MaritalStatus = employee.MaritalStatus,
        //                Mobile = employee.Mobile,
        //                DOB = employee.DOB,
        //                CNICNo = employee.CNICNo,
        //                CNICIssueDate = employee.CNICIssueDate,
        //                CNICExpiryDate = employee.CNICExpiryDate,
        //                Email = employee.Email,
        //                Address = employee.Address,
        //                JoiningDate = employee.JoiningDate,
        //                ProbationPeriod = employee.ProbationPeriod,
        //                EndofProbationDate = employee.EndofProbationDate,
        //                ConfirmationDate = employee.ConfirmationDate,
        //                ConfirmationDelay = employee.ConfirmationDelay,
        //                ConfirDelayReason = employee.ConfirDelayReason,
        //                PlacementId = employee.PlacementId
        //            };
        //            await _repository.AddAsync(newEmployee);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Employee");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Error While Saving to Database");

        //            }
        //            return View(employee);
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> DeleteEmployee(int id)
        //        {
        //            var temp = await _repository.GetEmployeeTypeById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            await _repository.Delete(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Employee");
        //            }
        //            return RedirectToAction("Employee");
        //        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> SubjectTeacherAllocation(int id)
        {
            ClassList classes = new ClassList(); 
            var dbClassName = from a in _db.Grades
                              from b in _db.Sections
                              where b.SectionId == id && a.GradeId == b.GradeId
                              select new
                              {
                                  ClassName = a.GradeName + " - " + b.SectionName,
                                  GradeId = b.GradeId
                              };
            var GradeBooks = from b in _db.Books
                             from c in _db.Subjects
                             where b.GradeId == dbClassName.First().GradeId && c.SubjectId == b.SubjectId
                             select new BookList
                             {
                                 BookId = b.BookId,
                                 BookName = b.BookName,
                                 SubjectName = c.SubjectName
                             };
            var previousAllcoations = await _db.SubjectTeacherAllocations.Where(x => x.SectionId == id).ToListAsync();
            classes.ClassName = dbClassName.FirstOrDefault().ClassName.ToString();
            classes.SectionId = id;
            List<BookList> bookLists = new List<BookList>();
            bookLists.AddRange(await GradeBooks.Select(x => new BookList { BookId = x.BookId, BookName = x.BookName, SubjectName = x.SubjectName,EmployeeId = x.EmployeeId }).ToListAsync());
            foreach (var book in bookLists)
            {
                foreach (var preAllo in previousAllcoations)
                {
                    if(book.BookId == preAllo.BookId)
                    {
                        book.EmployeeId = preAllo.EmployeeId;
                    }
                }
            }
            classes.books = bookLists;
            ViewBag.Employees = from a in _db.SchoolSections
                                from b in _db.Grades
                                from c in _db.Sections
                                from d in _db.Employees
                                from e in _db.Roles
                                where b.SchoolSectionId == a.SchoolSectionId && c.GradeId == b.GradeId && c.SectionId == id && d.SchoolSectionId == a.SchoolSectionId && e.RoleId == d.RoleId && (e.RollName == "Subject Teacher" || e.RollName == "Class Teacher" || e.RollName == "Grade Manager")
                                select d;
            return View(classes);
        }
        [HttpPost]
        public async Task<IActionResult> SubjectTeacherAllocation(ClassList data)
        {
            //foreach (var book in data.books)
            //{
            //    book.
            //}
            var previousAllcoations = await _db.SubjectTeacherAllocations.Where(x => x.SectionId == data.SectionId).ToListAsync();
            _db.RemoveRange(previousAllcoations);
            //SubjectTeacherAllocation subjectTeacher = new SubjectTeacherAllocation();
            //List<SubjectTeacherAllocation> subjectTeachers = new List<SubjectTeacherAllocation>();
            foreach (var book in data.books)
            {
                if(book.EmployeeId != null)
                {
                    var subjectTeacher = new SubjectTeacherAllocation
                    {
                        BookId = book.BookId,
                        SectionId = data.SectionId,
                        EmployeeId = book.EmployeeId
                    };
                    await _db.AddAsync(subjectTeacher);
                }
            }
            if(await _repository.SaveChanges())
            {
                return RedirectToAction("Section","Grade");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(data);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Auth");
        }
        #region User
        [Authorize(Policy = "Users.Read")]
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var result = await _db.Users.Where(x => x.IsActive == true).ToListAsync();
            return View(result);
        }
        [Authorize(Policy = "Users.Create")]
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }
        [Authorize(Policy = "Users.Create")]
        [HttpPost]
        public async Task<IActionResult> AddUser(UserVM user)
        {
            //string UserImage = user.UserImage == null ? null : FileSaver(user);
            var newUser = new Entities.Models.User
            {
                //FName = user.FName,
                //LName = user.LName,
                //FatherName = user.FatherName,
                //Email = user.Email,
                UserName = user.UserName,
                Password = user.Password,
                //UserImageURL = UserImage,
                IsActive = (bool)user.IsActive
            };
            await _repository.AddAsync(newUser);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Users");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(user);
        }
        [Authorize(Policy = "Users.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var temp = await _repository.GetUserById(id);
            var user = new UserVM
            {
                UserId = temp.UserId,
                FName = temp.FName,
                LName = temp.LName,
                FatherName = temp.FatherName,
                Email = temp.Email,
                UserName = temp.UserName,
                Password = temp.Password,
                IsActive = temp.IsActive,
                //UserImageUrl = temp.UserImageURL
            };
            return View(user);
        }
        [Authorize(Policy = "Users.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserVM updateUser)
        {
            var temp = await _repository.GetUserById(updateUser.UserId);
            //temp.FName = updateUser.FName;
            //temp.LName = updateUser.LName;
            //temp.FatherName = updateUser.FatherName;
            //temp.Email = updateUser.Email;
            temp.UserName = updateUser.UserName;
            temp.Password = updateUser.Password;
            temp.IsActive = updateUser.IsActive;
            //if (updateUser.UserImage != null)
            //{
            //    if(updateUser.UserImageUrl != null)
            //    {
            //        var oldImage = new FileInfo(@"C:\\Ultra_Pro\\.Net Learning\\Advanced C#\\webApp\\myWebApp\\wwwroot\\UsersImages\\"+ updateUser.UserImageUrl);
            //        oldImage.Delete();
            //    }
            //    temp.UserImageURL = FileSaver(updateUser);
            //}
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Users");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(updateUser);
        }
        [Authorize(Policy = "Users.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var temp = await _repository.GetUserById(id);
            if (temp == null)
            {
                return NotFound();
            }
            temp.IsActive = false;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Users");
            }
            return RedirectToAction("Users");
        }
        public string FileSaver(UserVM user)
        {
            var fileInfo = new FileInfo(user.UserImage.FileName);
            string ImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UsersImages/");
            if (!Directory.Exists(ImagePath)) Directory.CreateDirectory(ImagePath);
            string ImageName = user.FName + user.LName + fileInfo.Extension;
            string imageNameWithPath = Path.Combine(ImagePath, ImageName);
            using (var stream = new FileStream(imageNameWithPath, FileMode.Create))
            {
                user.UserImage.CopyTo(stream);
            }
            return ImageName;
        }
        #endregion

        #region Roles
        [Authorize(Policy = "Roles.Read")]
        [HttpGet]
        public async Task<IActionResult> Roles()
        {
            RolesVM roleVM = new RolesVM();
            var roles = await _db.Roles.ToListAsync();
            roleVM.roles = roles.Select(x => new RolesList { RoleId = x.RoleId, RoleName = x.RollName, IsActive = (bool)x.IsActive}).Where(x => x.IsActive == true).ToList();
            foreach (var role in roleVM.roles)
            {
                var permissions = from a in _db.UserPermissions
                                  from b in _db.Permissions
                                  where a.RoleId == role.RoleId && b.PermissionId == a.PermissionId
                                  select new
                                  {
                                      PermissionName = b.PermissionName
                                  };
                List<string> permiss = new List<string>();
                role.Permissions = permissions.Select(x => x.PermissionName).ToList();
            }
            return View(roleVM);
        }
        [Authorize(Policy = "Roles.Create")]
        [HttpGet]
        public async Task<IActionResult> AddRoles()
        {
            RolesVM roles = new RolesVM();
            roles.AddUpdatePermissions = await _db.Permissions.Where(x => x.IsActive == true).Select(x => new myWebApp.ViewModels.Director.Permissions { PermissionId = x.PermissionId, PermissionName = x.PermissionName }).ToListAsync();
            return View(roles);
        }
        [Authorize(Policy = "Roles.Create")]
        [HttpPost]
        public async Task<IActionResult> AddRoles(RolesVM role)
        {
            //string UserImage = user.UserImage == null ? null : FileSaver(user);
            var newRole = new Roles()
            {
                RollName = role.RoleName
            };
            await _repository.AddAsync(newRole);
            if (await _repository.SaveChanges())
            {
                var recentRole = await _db.Roles.OrderBy(x => x.RoleId).LastOrDefaultAsync();
                var selectedPermissions = role.AddUpdatePermissions.Where(x => x.isSelected == true).ToList();
                foreach (var permission in selectedPermissions)
                {
                    var rolePermissions = new UserPermissions
                    {
                        RoleId = recentRole.RoleId,
                        PermissionId = permission.PermissionId
                    };
                    await _repository.AddAsync(rolePermissions);
                }
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Roles");
                }
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(role);
        }
        [Authorize(Policy = "Roles.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateRole(int id)
        {
            var temp = await _db.Roles.Where(x => x.RoleId == id).FirstOrDefaultAsync();
            var user = new RolesVM
            {
                RoleId = temp.RoleId,
                //IsActive = (bool)temp.IsActive,
                //RoleName = temp.RollName
            };
            user.AddUpdatePermissions = await _db.Permissions.Select(x => new myWebApp.ViewModels.Director.Permissions { PermissionId = x.PermissionId, PermissionName = x.PermissionName }).ToListAsync();
            var userPermissions = await _db.UserPermissions.Where(x => x.RoleId == id).ToListAsync();
            foreach (var rolep in user.AddUpdatePermissions)
            {
                foreach (var permission in userPermissions)
                {
                    if (rolep.PermissionId == permission.PermissionId)
                    {
                        rolep.isSelected = true;
                    }
                }
            }
            return View(user);
        }
        [Authorize(Policy = "Roles.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateRole(RolesVM role)
        {
            var temp = await _db.Roles.Where(x => x.RoleId == role.RoleId).FirstOrDefaultAsync();
            //temp.RollName = role.RoleName;
            //temp.IsActive = role.IsActive;
            var oldPermissions = await _db.UserPermissions.Where(x => x.RoleId == temp.RoleId).ToListAsync();
            _db.RemoveRange(oldPermissions);
            var newPermissions = role.AddUpdatePermissions.Where(x => x.isSelected == true).ToList();
            foreach (var perm in newPermissions)
            {
                var add = new UserPermissions
                {
                    RoleId = temp.RoleId,
                    PermissionId = perm.PermissionId
                };
                await _repository.AddAsync(add);
            }
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Roles");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(role);
        }
        [Authorize(Policy = "Roles.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var temp = await _db.Roles.Where(x => x.RoleId == id).FirstOrDefaultAsync();
            if (temp == null)
            {
                return NotFound();
            }
            temp.IsActive = false;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Roles");
            }
            return RedirectToAction("Roles");
        }
        public async Task<IActionResult> TestTable()
        {
            #region AllDBChangesLoops
            //var employees = _db.Employees.ToList();
            //foreach (var emp in employees)
            //{
            //    emp.IsActive = true;
            //}
            //_db.UpdateRange(employees);
            //var shura = _db.Shoora.ToList();
            //foreach (var sh in shura)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(shura);
            //var holidays = _db.Holidays.ToList();
            //foreach (var sh in holidays)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(holidays);
            //var resbook = _db.ResourceNoteBooks.ToList();
            //foreach (var sh in resbook)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(resbook);
            //var roles = _db.Roles.ToList();
            //foreach (var sh in roles)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(roles);
            //var bk = _db.Books.ToList();
            //foreach (var sh in bk)
            //{
            //    sh.IsActive = true;
            //    sh.IsWorkBook = false;
            //}
            //_db.UpdateRange(bk);
            //var campus = _db.Campuses.ToList();
            //foreach (var sh in campus)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(campus);
            //var depar = _db.Departments.ToList();
            //foreach (var sh in depar)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(depar);
            //var desig = _db.Designations.ToList();
            //foreach (var sh in desig)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(desig);
            //var gend = _db.Genders.ToList();
            //foreach (var sh in gend)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(gend);
            //var grades = _db.Grades.ToList();
            //foreach (var sh in grades)
            //{
            //    sh.SchoolSectionId = 1;
            //}
            //_db.UpdateRange(grades);
            //var head = _db.Heads.ToList();
            //foreach (var sh in head)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(head);
            //var school = _db.Schools.ToList();
            //foreach (var sh in school)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(school);
            //var sdep = _db.subDepartments.ToList();
            //foreach (var sh in sdep)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(sdep);
            //var subjects = _db.Subjects.ToList();
            //foreach (var sh in subjects)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(subjects);
            //var term = _db.terms.ToList();
            //foreach (var sh in term)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(term);
            //var year = _db.years.ToList();
            //foreach (var sh in year)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(year);
            //var sTopic = _db.subTopics.ToList();
            //foreach (var sh in sTopic)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(sTopic);
            //var tmethod = _db.TeachingMethodologies.ToList();
            //foreach (var sh in tmethod)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(tmethod);
            //var topic = _db.topics.ToList();
            //foreach (var sh in topic)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(topic);
            //var unit = _db.Units.ToList();
            //foreach (var sh in unit)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(unit);
            //var user = _db.Users.ToList();
            //foreach (var sh in user)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(user);
            //var chapter = _db.chapters.ToList();
            //foreach (var sh in chapter)
            //{
            //    sh.IsActive = true;
            //}
            //_db.UpdateRange(chapter);
            //_db.UpdateRange(employees, shura, holidays, resbook, roles, bk, campus, depar, desig, gend, grades, head, school, sdep, subjects, term, year, sTopic,
            //    tmethod, topic, unit, chapter
            //    );
            //await _repository.SaveChanges();
            #endregion
            return View();
        }
        #endregion
    }
}
