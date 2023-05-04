using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Entities.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Routing.Template;
using System.Reflection.PortableExecutable;
using myWebApp.ViewModels.Setups;
using myWebApp.ViewModels.HumanResource;
using myWebApp.ViewModels.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using myWebApp.ViewModels.Setups.School;
using myWebApp.ViewModels.Setups.Campus;
using myWebApp.ViewModels.Setups.SchoolSection;
using Microsoft.AspNetCore.Authorization;

namespace myWebApp.Controllers
{
    [Authorize]
    public class SetupController : Controller
    {
        private readonly IEFRepository _repository;
        private readonly SchoolDbContext _db;

        public SetupController(IEFRepository repository,  SchoolDbContext db)
        {
            _repository = repository;
            _db = db;
        }

        #region School-Setup
        [Authorize(Policy = "School.Read")]
        [HttpGet]
        public async Task<IActionResult> School()
        {
            ViewBag.Schools = await _db.Schools.ToListAsync();
            return View();
        }
        [Authorize(Policy = "School.Create")]
        [HttpPost]
        public async Task<IActionResult> AddSchool(SchoolVM school)
        {
            var newSchool = new School
            {
                CEOName = school.CEOName,
                Abbrevation = school.Abbrevation,
                address = school.address,
                Email = school.Email,
                RegistrationNo = school.RegistrationNo,
                PhoneNo = school.PhoneNo,
                SchoolName = school.SchoolName
            };
            await _repository.AddAsync(newSchool);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("School");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(school);
        }
        [Authorize(Policy = "School.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateSchool(int id)
        {
            var temp = await _db.Schools.Where(x => x.SchoolId == id).FirstOrDefaultAsync();
            var school = new SchoolVM
            {
                SchoolId = temp.SchoolId,
                SchoolName = temp.SchoolName,
                Abbrevation = temp.Abbrevation,
                address = temp.address,
                CEOName = temp.CEOName,
                Email = temp.Email,
                PhoneNo = temp.PhoneNo,
                RegistrationNo = temp.RegistrationNo
            };
            return View(school);
        }
        [Authorize(Policy = "School.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateSchool(SchoolVM school)
        {
            bool active = false;
            var temp = await _db.Schools.Where(x => x.SchoolId == school.SchoolId).FirstOrDefaultAsync();
            temp.SchoolName = school.SchoolName;
            temp.PhoneNo = school.PhoneNo;
            temp.CEOName = school.CEOName;
            temp.Email = school.Email;
            temp.address = school.address;
            temp.Abbrevation = school.Abbrevation;
            temp.RegistrationNo = school.RegistrationNo;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("School");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(Department);
        }
        [Authorize(Policy = "School.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteSchool(int id)
        {
            var temp = await _db.Schools.Where(x => x.SchoolId == id).FirstOrDefaultAsync();
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("School");
            }
            return RedirectToAction("School");
        }
        #endregion

        #region Campus-Setup
        [Authorize(Policy = "Campus.Read")]
        [HttpGet]
        public async Task<IActionResult> Campus()
        {
            ViewBag.Schools = await _db.Schools.ToListAsync();
            CampusVM campus = new CampusVM();
            var campuses = from s in _db.Schools
                           from c in _db.Campuses
                           where s.SchoolId == c.SchoolId
                           select new CampusVM
                           {
                               SchoolName = s.SchoolName,
                               PrincipalName = c.PrincipalName,
                               PhoneNo = c.PhoneNo,
                               Email = c.Email,
                               CampusName = c.CampusName,
                               address = c.address,
                               Abbrevation = c.Abbrevation,
                               CampusId = c.CampusId,
                               IsActive = (bool)c.IsActive
                           };
            campus.campuses.AddRange(await campuses.Select(x => new CampusVM {IsActive = x.IsActive,CampusId = x.CampusId,Abbrevation = x.Abbrevation, address = x.address, CampusName = x.CampusName, PrincipalName = x.PrincipalName, Email = x.Email, SchoolName = x.SchoolName, PhoneNo = x.PhoneNo }).ToListAsync());
            return View(campus);
        }
        [Authorize(Policy = "Campus.Create")]
        [HttpPost]
        public async Task<IActionResult> AddCampus(CampusVM campus)
        {
            var newCampus = new Campus
            {
                PrincipalName = campus.PrincipalName,
                Abbrevation = campus.Abbrevation,
                address = campus.address,
                Email = campus.Email,
                PhoneNo = campus.PhoneNo,
                SchoolId = campus.SchoolId,
                CampusName = campus.CampusName
            };
            await _repository.AddAsync(newCampus);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Campus");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(campus);
        }
        [Authorize(Policy = "Campus.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateCampus(int id)
        {
            var temp = await _db.Campuses.Where(x => x.CampusId == id).FirstOrDefaultAsync();
            var campus = new CampusVM
            {
                CampusId = temp.CampusId,
                SchoolId = (int)temp.SchoolId,
                CampusName = temp.CampusName,
                Abbrevation = temp.Abbrevation,
                address = temp.address,
                PrincipalName = temp.PrincipalName,
                Email = temp.Email,
                PhoneNo = temp.PhoneNo,
                IsActive = (bool)temp.IsActive
            };
            ViewBag.Schools = await _db.Schools.ToListAsync();
            return View(campus);
        }
        [Authorize(Policy = "Campus.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateCampus(Campus campus)
        {
            var temp = await _db.Campuses.Where(x => x.CampusId == campus.CampusId).FirstOrDefaultAsync();
            temp.CampusName = campus.CampusName;
            temp.PhoneNo = campus.PhoneNo;
            temp.PrincipalName = campus.PrincipalName;
            temp.Email = campus.Email;
            temp.address = campus.address;
            temp.Abbrevation = campus.Abbrevation;
            temp.SchoolId = campus.SchoolId;
            temp.IsActive = campus.IsActive;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Campus");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(campus);
        }
        [Authorize(Policy = "Campus.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteCampus(int id)
        {
            var temp = await _db.Campuses.Where(x => x.CampusId == id).FirstOrDefaultAsync();
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Campus");
            }
            return RedirectToAction("Campus");
        }
        #endregion

        #region SchoolSection-Setup
        [Authorize(Policy = "SchoolSection.Read")]
        [HttpGet]
        public async Task<IActionResult> SchoolSection()
        {
            ViewBag.ACs = await (from a in _db.Employees
                          from b in _db.Roles
                          where b.RoleId == a.RoleId && b.RollName == "Assistant Coordinator"
                          select a).ToListAsync();
            SchoolSectionVM sSection = new SchoolSectionVM();
            sSection.SchoolSections = await (from s in _db.SchoolSections
                           join c in _db.Campuses on s.CampusId equals c.CampusId into SchoolCampus
                           from Campus in SchoolCampus.DefaultIfEmpty()
                           join d in _db.Employees on s.AssistantCoordinatorId equals d.EmployeeId into AssCordinator
                           from AC in AssCordinator.DefaultIfEmpty()
                           where s.IsActive == true
                           select new SchoolSectionVM
                           {
                               SectionName = s.SectionName,
                               SchoolSectionId = s.SchoolSectionId,
                               PhoneNo = s.PhoneNo,
                               Email = AC.Email,
                               CampusName = Campus.CampusName,
                               address = s.address,
                               Abbrevation = s.Abbrevation,
                               SectionHead = AC.FName + " " + AC.LName,
                               SchoolId = s.SchoolId,
                               ACId = (int)s.AssistantCoordinatorId
                           }).ToListAsync(); 
            ViewBag.Campuses = await _db.Campuses.ToListAsync();
            ViewBag.Schools = await _db.Schools.ToListAsync();
            return View(sSection);
        }
        [Authorize(Policy = "SchoolSection.Create")]
        [HttpPost]
        public async Task<IActionResult> AddSchoolSection(SchoolSectionVM sSection)
        {
            var newSchoolSection = new SchoolSection
            {
                SectionHead = sSection.SectionHead,
                Abbrevation = sSection.Abbrevation,
                address = sSection.address,
                Email = sSection.Email,
                PhoneNo = sSection.PhoneNo,
                CampusId = sSection.CampusId,
                SectionName = sSection.SectionName,
                SchoolId = sSection.SchoolId,
                AssistantCoordinatorId = sSection.ACId
            };
            await _repository.AddAsync(newSchoolSection);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("SchoolSection");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(sSection);
        }
        [Authorize(Policy = "SchoolSection.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateSchoolSection(int id)
        {
            var temp = await _db.SchoolSections.Where(x => x.SchoolSectionId == id).FirstOrDefaultAsync();
            ViewBag.ACs = await (from a in _db.Employees
                                 from b in _db.Roles
                                 where b.RoleId == a.RoleId && b.RollName == "Assistant Coordinator"
                                 select a).ToListAsync();
            var Ssection = new SchoolSectionVM
            {
                CampusId = temp.CampusId,
                SchoolSectionId = temp.SchoolSectionId,
                SectionName = temp.SectionName,
                Abbrevation = temp.Abbrevation,
                address = temp.address,
                SectionHead = temp.SectionHead,
                Email = temp.Email,
                PhoneNo = temp.PhoneNo,
                SchoolId = temp.SchoolId,
                IsActive = (bool)temp.IsActive,
                ACId = (int)temp.AssistantCoordinatorId
            };
            ViewBag.Campuses = await _db.Campuses.ToListAsync();
            ViewBag.Schools = await _db.Schools.ToListAsync();
            return View(Ssection);
        }
        [Authorize(Policy = "SchoolSection.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateSchoolSection(SchoolSectionVM sSection)
        {
            var temp = await _db.SchoolSections.Where(x => x.SchoolSectionId == sSection.SchoolSectionId).FirstOrDefaultAsync();
            temp.SectionName = sSection.SectionName;
            temp.PhoneNo = sSection.PhoneNo;
            temp.SectionHead = sSection.SectionHead;
            temp.Email = sSection.Email;
            temp.address = sSection.address;
            temp.Abbrevation = sSection.Abbrevation;
            temp.CampusId = sSection.CampusId;
            temp.SchoolId = sSection.SchoolId;
            temp.IsActive = sSection.IsActive;
            temp.AssistantCoordinatorId = sSection.ACId;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("SchoolSection");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(sSection);
        }
        [Authorize(Policy = "SchoolSection.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteSchoolSection(int id)
        {
            var temp = await _db.SchoolSections.Where(x => x.SchoolSectionId == id).FirstOrDefaultAsync();
            if (temp == null)
            {
                return NotFound();
            }
            temp.IsActive = false;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("SchoolSection");
            }
            return RedirectToAction("SchoolSection");
        }
        #endregion

        #region Department-Setup
        [Authorize(Policy = "Department.Read")]
        [HttpGet]
        public async Task<IActionResult> Department()
        {
            var result = from department in _db.Departments
                         from shoora in _db.Shoora
                         where shoora.ShooraId == department.DepartmentHeadId
                         select new DepartmentList
                         {
                             DepartmentId = department.DepartmentId,
                             DepartmentHeadName = shoora.FName + " " + shoora.LName,
                             DepartmentName = department.DepartmentName,
                             Description = department.Description,
                             ShortDescription = department.ShortDescripiton,
                             IsActive = department.IsActive
                         };
            DepartmentVM dep = new DepartmentVM();

            dep.departmentLists = await result.Select(x => new DepartmentList { DepartmentId = x.DepartmentId, DepartmentHeadName = x.DepartmentHeadName, DepartmentName = x.DepartmentName, Description = x.Description, ShortDescription = x.ShortDescription, IsActive = x.IsActive }).ToListAsync();

            return View(dep);
        }
        [Authorize(Policy = "Department.Create")]
        [HttpGet]
        public async Task<IActionResult> AddDepartment()
        {
            ViewBag.Heads = await _repository.GetShooras();
            return View();
        }
        [Authorize(Policy = "Department.Create")]
        [HttpPost]
        public async Task<IActionResult> AddDepartment(DepartmentVM Department)
        {
            var newDepartment = new Department
            {
                DepartmentName = Department.DepartmentName,
                Description = Department.Description,
                DepartmentHeadId = Department.DepartmentHeadId,
                ShortDescripiton = Department.ShortDescription
            };
            await _repository.AddAsync(newDepartment);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Department");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(Department);
        }
        [Authorize(Policy = "Department.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateDepartment(int id)
        {
            ViewBag.Heads = await _db.Shoora.ToListAsync();
            var temp = await _repository.GetDepartmentById(id);
            if (temp == null)
            {
                return NotFound();
            }
            var Department = new UpdateDepartmentVM
            {
                DepartmentId = temp.DepartmentId,
                DepartmentName = temp.DepartmentName,
                Description = temp.Description,
                DepartmentHeadId = (int)temp.DepartmentHeadId,
                ShortDescription = temp.ShortDescripiton,
                IsActive = (bool)temp.IsActive
            };
            return View(Department);
        }
        [Authorize(Policy = "Department.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateDepartment(UpdateDepartmentVM Department)
        {
            var temp = await _repository.GetDepartmentById(Department.DepartmentId);
            if (temp == null)
            {
                return NotFound();
            }
            temp.DepartmentName = Department.DepartmentName;
            temp.Description = Department.Description;
            temp.DepartmentHeadId = Department.DepartmentHeadId;
            temp.ShortDescripiton = Department.ShortDescription;
            temp.IsActive = Department.IsActive;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Department");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(Department);
        }
        [Authorize(Policy = "Department.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var temp = await _repository.GetDepartmentById(id);
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Department");
            }
            return RedirectToAction("Department");
        }
        #endregion

        #region SubDepartment-Setup
        [Authorize(Policy = "SubDepartment.Read")]
        [HttpGet]
        public async Task<IActionResult> SubDepartment()
        {
            SubDepartmentVM subDepartment = new SubDepartmentVM();

            var depNames = from subd in _db.subDepartments
                           join dep in _db.Departments on subd.MainDepartmentId equals dep.DepartmentId into mainDep
                           from mD in mainDep.DefaultIfEmpty()
                           join head in _db.Heads on subd.HeadId equals head.HeadId into DepartmentHead
                           from dhead in DepartmentHead.DefaultIfEmpty()
                           join emp in _db.Employees on dhead.EmployeeId equals emp.EmployeeId into headEmployee
                           from hemp in headEmployee.DefaultIfEmpty()
                           join shura in _db.Shoora on dhead.ShooraId equals shura.ShooraId into headShoora
                           from hshura in headShoora.DefaultIfEmpty()
                           select new SubDepartmentList
                           {
                               SubDepartmentName = subd.DepartmentName,
                               SubDepartmentId = subd.SubDepartmentId,
                               MainDepartmentName = mD.DepartmentName,
                               IsActive = subd.IsActive,
                               HeadEmployee = dhead.EmployeeId == null?hshura.FName + " " + hshura.LName + " - Shoora": hemp.FName + " " + hemp.LName + " - Employee"
                               //HeadEmployee = emps.FName + " " + emps.LName,
                               //HeadShoora = shoora.FName + " " + shoora.LName
                           };
            //var empNames = from dep in depNames
            //               join h in _db.Heads on dep.HeadId equals h.HeadId into heads
            //               from head in heads.DefaultIfEmpty()
            //               join e in _db.Employees on head.EmployeeId equals e.EmployeeId into employees
            //               from emp in employees
            //               select new
            //               {
            //                   EmployeeName = emp.FName + " " + emp.LName
            //               };
            //var ShuraNames = from dep in depNames
            //                 from head in _db.Heads
            //                 from shura in _db.Shoora
            //                 where head.HeadId == dep.HeadId && shura.ShooraId == head.ShooraId
            //                 select new
            //                 {
            //                     ShooraName = shura.FName + " " + shura.LName
            //                 };
            subDepartment.subdepartments = await depNames.Select(x => new SubDepartmentList {IsActive = x.IsActive,MainDepartmentName = x.MainDepartmentName, SubDepartmentId = x.SubDepartmentId, SubDepartmentName = x.SubDepartmentName }).ToListAsync();
            #region Comment
            //var depNames = from sdep in _db.subDepartments
            //               from dep in _db.Departments
            //               where dep.DepartmentId == sdep.MainDepartmentId
            //               select new
            //               {
            //                   MainDepartmentName = dep.DepartmentName,
            //                   SubDepartmentId = sdep.SubDepartmentId,
            //                   SubDepartmentName = sdep.DepartmentName,
            //                   HeadId = sdep.HeadId
            //               };
            //var Heads = from a in depNames
            //            from b in _db.Heads
            //            where b.HeadId == a.HeadId
            //            select new
            //            {
            //                EmployeeId = b.EmployeeId,
            //                ShooraId = b.ShooraId
            //            };
            ////depNames.Where(x => x.HeadId == Heads.)
            //var Employee = from head in Heads
            //               from emp in _db.Employees
            //               where emp.EmployeeId == head.EmployeeId
            //               select new
            //               {
            //                   EmployeeName = emp.FName + " " + emp.LName
            //               };
            //var shoora = from h in Heads
            //             from shu in _db.Shoora
            //             where shu.ShooraId == h.ShooraId
            //             select new
            //             {
            //                 ShooraName = shu.FName + " " + shu.LName
            //             };
            //var sqldata = _db.Database.ExecuteSqlRaw("select sd.DepartmentName,d.DepartmentName,h.EmployeeId,h.ShooraId,e.FName,s.FName from [dbo].[Setup_SubDepartment] sd \r\njoin [dbo].[Setup_Department] d on d.DepartmentId=sd.MainDepartmentId\r\njoin [dbo].[Setup_Head] h on h.HeadId=sd.HeadId\r\nleft join [dbo].[Employee] e on e.EmployeeId=h.EmployeeId\r\nleft join [dbo].[Shoora] s on s.ShooraId=h.ShooraId");
            //join head in _db.Heads on dwpd
            //from emp in _db.Employees on dhead.EmployeeId equals depWithHead.
            //left join shura in _db.Shoora on dhead.ShooraId equals shura.ShooraId
            //select new SubDepartmentList
            //{
            //    MainDepartmentName = dep.DepartmentName,
            //    SubDepartmentId = sdep.SubDepartmentId,
            //    SubDepartmentName = sdep.DepartmentName,
            //    HeadEmployee = emp.FName + " " + emp.LName,
            //    HeadShoora = shura.FName + " " + shura.LName
            //};
            //subDepartment.subdepartments = await depNames.Select(x => new SubDepartmentList { MainDepartmentName = x.MainDepartmentName, SubDepartmentId = x.SubDepartmentId, SubDepartmentName = x.SubDepartmentName, HeadShoora = x.HeadShoora, HeadEmployee = x.HeadEmployee }).ToListAsync();

            //var HeadENames = from depname in depNames
            //                 from heads in _db.Heads
            //                 where depname.HeadId == heads.HeadId
            //                 select new
            //                 {
            //                     HeadEmployeeId = heads.HeadId,
            //                     HeadShooraId = heads.ShooraId
            //                 };
            //var HeadEmps = from HENames in HeadENames
            //               from emps in _db.Employees
            //               where emps.EmployeeId == emps.EmployeeId
            //               select new SubDepartmentList
            //               {
            //                   HeadEmployee = emps.FName + " " + emps.LName
            //               };
            //var HeadShuras = from HENAMES in HeadENames
            //                 from shura in _db.Shoora
            //                 where shura.ShooraId == HENAMES.HeadShooraId
            //                 select new SubDepartmentList
            //                 {
            //                     HeadShoora = shura.FName + " " + shura.LName
            //                 };
            //subDepartment.subdepartments = depNames.Where(x => x.)
            //var getData = from sdep in _db.subDepartments
            //              join dep in _db.Departments on sdep.MainDepartmentId equals dep.DepartmentId
            //              join head in _db.Heads on sdep.HeadId equals head.HeadId
            //              //join emp in _db.Employees on head.EmployeeId equals emp.EmployeeId
            //              //join shoora in _db.Shoora on head.ShooraId equals shoora.ShooraId
            //              select new
            //              {
            //                  //HeadName = head.FName + " " + head.LName,
            //                  MainDepartmentName = dep.DepartmentName,
            //                  SubDepartmentId = sdep.SubDepartmentId,
            //                  SubDepartmentName = sdep.DepartmentName
            //              };
            //HeadName = x.HeadName, 

            //}
            //else
            //{
            //    var getData = from sdep in _db.subDepartments
            //                  join dep in _db.Departments on sdep.MainDepartmentId equals dep.DepartmentId
            //                  select new
            //                  {
            //                      MainDepartmentName = dep.DepartmentName,
            //                      SubDepartmentId = sdep.SubDepartmentId,
            //                      SubDepartmentName = sdep.DepartmentName
            //                  };
            //subDepartment.subdepartments = await getData.Select(x => new SubDepartmentList {MainDepartmentName = x.MainDepartmentName, SubDepartmentId = x.SubDepartmentId, SubDepartmentName = x.SubDepartmentName }).ToListAsync();
            //}
            #endregion
            return View(subDepartment);
        }
        [Authorize(Policy = "SubDepartment.Create")]
        [HttpGet]
        public async Task<IActionResult> AddSubDepartment()
        {
            ViewBag.Departments = await _repository.GetDepartments();
            List<HeadViewList> heads = new List<HeadViewList>();
            var HeadEmployees = from a in _db.Heads
                                from b in _db.Employees
                                where a.EmployeeId == b.EmployeeId
                                select new
                                {
                                    HeadId = a.HeadId,
                                    EmployeeName = b.FName + " " + b.LName,
                                    IsActive = a.IsActive
                                };
            var HeadShoora = from a in _db.Heads
                             from b in _db.Shoora
                             where a.ShooraId == b.ShooraId
                             select new
                             {
                                 HeadId = a.HeadId,
                                 ShooraName = b.FName + " " + b.LName,
                                 IsActive = a.IsActive
                             };
            heads.AddRange(await HeadEmployees.Select(x => new HeadViewList { HeadId = x.HeadId, EmployeeName = x.EmployeeName, IsActive = x.IsActive }).ToListAsync());
            heads.AddRange(await HeadShoora.Select(x => new HeadViewList { HeadId = x.HeadId, ShooraName = x.ShooraName, IsActive = x.IsActive }).ToListAsync());
            SubDepartmentVM subDepartment = new SubDepartmentVM();
            subDepartment.heads.AddRange(heads);
            return View(subDepartment);
        }
        [Authorize(Policy = "SubDepartment.Create")]
        [HttpPost]
        public async Task<IActionResult> AddSubDepartment(SubDepartmentVM Department)
        {
            var newDepartment = new SubDepartment
            {
                DepartmentName = Department.DepartmentName,
                HeadId = Department.HeadId,
                MainDepartmentId = Department.MainDepartmentId
            };
            await _repository.AddAsync(newDepartment);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("SubDepartment");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }

            return View(Department);
        }
        [Authorize(Policy = "SubDepartment.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateSubDepartment(int id)
        {
            ViewBag.Departments = await _repository.GetDepartments();
            ViewBag.Employees = await _repository.GetEmployees();
            var temp = await _repository.GetSubDepartmentById(id);
            if (temp == null)
            {
                return NotFound();
            }
            var SubDepartment = new UpdateSubDepartmentVM
            {
                SubDepartmentId = temp.SubDepartmentId,
                DepartmentName = temp.DepartmentName,
                //IsActive = temp.IsActive,
                HeadId = (int)temp.HeadId,
                MainDepartmentId = (int)temp.MainDepartmentId,
                IsActive = (bool)temp.IsActive
            };
            return View(SubDepartment);
        }
        [Authorize(Policy = "SubDepartment.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateSubDepartment(UpdateSubDepartmentVM Department)
        {
            var temp = await _repository.GetSubDepartmentById(Department.SubDepartmentId);
            if (temp == null)
            {
                return NotFound();
            }
            temp.DepartmentName = Department.DepartmentName;
            temp.HeadId = Department.HeadId;
            temp.MainDepartmentId = Department.MainDepartmentId;
            temp.IsActive = Department.IsActive;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("SubDepartment");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(Department);
        }
        [Authorize(Policy = "SubDepartment.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteSubDepartment(int id)
        {
            var temp = await _repository.GetSubDepartmentById(id);
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("SubDepartment");
            }
            return RedirectToAction("SubDepartment");
        }
        #endregion

        #region Designation-Setup
        [Authorize(Policy = "Designation.Read")]
        [HttpGet]
        public async Task<IActionResult> Designation()
        {
            var result = await _repository.GetDesignations();
            return View(result);
        }
        [Authorize(Policy = "Designation.Create")]
        [HttpGet]
        public IActionResult AddDesignation()
        {
            return View();
        }
        [Authorize(Policy = "Designation.Create")]
        [HttpPost]
        public async Task<IActionResult> AddDesignation(DesignationVM Designation)
        {
            var newDesignation = new Designation
            {
                Name = Designation.DesignationName
            };
            await _repository.AddAsync(newDesignation);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Designation");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(Designation);
        }
        [Authorize(Policy = "Designation.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateDesignation(int id)
        {
            var temp = await _repository.GetDesignationById(id);
            if (temp == null)
            {
                return NotFound();
            }
            var Location = new UpdateDesignationVM
            {
                DesigationId = temp.DesignationId,
                DesignationName = temp.Name,
                IsActive = temp.IsActive
            };
            return View(Location);
        }
        [Authorize(Policy = "Designation.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateDesignation(UpdateDesignationVM Designation)
        {
            var temp = await _repository.GetDesignationById(Designation.DesigationId);
            if (temp == null)
            {
                return NotFound();
            }
            temp.Name = Designation.DesignationName;
            temp.IsActive = Designation.IsActive;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Designation");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(Designation);
        }
        [Authorize(Policy = "Designation.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            var temp = await _repository.GetDesignationById(id);
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Designation");
            }
            return RedirectToAction("Designation");
        }

        #endregion

        #region Head-Setup
        [Authorize(Policy = "Heads.Read")]
        public async Task<IActionResult> Head()
        {
            List<HeadViewList> heads = new List<HeadViewList>();
            var HeadEmployees = from a in _db.Heads
                          from b in _db.Employees
                          where a.EmployeeId == b.EmployeeId
                          select new
                          {
                              HeadId = a.HeadId,
                              EmployeeName = b.FName + " " + b.LName,
                              IsActive = a.IsActive
                          };
            var HeadShoora = from a in _db.Heads
                                from b in _db.Shoora
                                where a.ShooraId == b.ShooraId
                                select new
                                {
                                    HeadId = a.HeadId,
                                    ShooraName = b.FName + " " + b.LName,
                                    IsActive = a.IsActive
                                };
            heads.AddRange(await HeadEmployees.Select(x => new HeadViewList { HeadId = x.HeadId, EmployeeName = x.EmployeeName, IsActive = x.IsActive}).ToListAsync());
            heads.AddRange(await HeadShoora.Select(x => new HeadViewList { HeadId = x.HeadId,ShooraName = x.ShooraName, IsActive = x.IsActive}).ToListAsync());
            return View(heads);
        }
        [Authorize(Policy = "Heads.Create")]
        [HttpGet]
        public async Task<IActionResult> AddHead()
        {
            ViewBag.Employees = await _repository.GetEmployees();
            ViewBag.ShooraMembers = await _repository.GetShooras();
            return View();
        }
        [Authorize(Policy = "Heads.Create")]
        [HttpPost]
        public async Task<IActionResult> AddHead(HeadVM head)
        {
            //bool active = false;
            //if (head.IsActive == 1)
            //{
            //    active = true;
            //}
            var NewHead = new Head
            {
                EmployeeId = head.EmployeeId,
                ShooraId = head.ShooraId
                //IsActive = active
            };
            await _repository.AddAsync(NewHead);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Head");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }

            return View(head);
        }
        [Authorize(Policy = "Heads.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateHead(int id)
        {
            ViewBag.Employees = await _repository.GetEmployees();
            ViewBag.ShooraMembers = await _repository.GetShooras();
            var temp = await _repository.GetHeadById(id);
            if (temp == null)
            {
                return NotFound();
            }
            var head = new UpdateHeadVM
            {
                Headid = temp.HeadId,
                EmployeeId = temp.EmployeeId,
                ShooraId = temp.ShooraId,
                IsActive = temp.IsActive
            };
            return View(head);
        }
        [Authorize(Policy = "Heads.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateHead(UpdateHeadVM head)
        {
            var temp = await _repository.GetHeadById(head.Headid);
            if (temp == null)
            {
                return NotFound();
            }
            temp.EmployeeId = head.EmployeeId;
            temp.ShooraId = head.ShooraId;
            temp.IsActive = head.IsActive;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Head");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(head);
        }
        [Authorize(Policy = "Heads.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteHead(int id)
        {
            var temp = await _repository.GetHeadById(id);
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Head");
            }
            return RedirectToAction("Head");
        }

        #endregion

        #region Gender
        public async Task<IActionResult> Gender()
        {
            var result = await _repository.GetGenders();
            return View(result);
        }
        [HttpGet]
        public IActionResult AddGender()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddGender(GenderVM genders)
        {
            if (ModelState.IsValid)
            {
                var NewGender = new Gender
                {
                    WGender = genders.Gender
                };
                await _repository.AddAsync(NewGender);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Gender");
                }
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(genders);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateGender(int id)
        {
            var temp = await _repository.GetGenderById(id);
            if (temp == null)
            {
                return NotFound();
            }
            var head = new UpdateGenderVM
            {
                GenderId = temp.GenderId,
                Gender = temp.WGender
            };
            return View(head);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateGender(UpdateGenderVM genders)
        {
            var temp = await _repository.GetGenderById(genders.GenderId);
            if (temp == null)
            {
                return NotFound();
            }
            temp.WGender = genders.Gender;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Gender");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(genders);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteGender(int id)
        {
            var temp = await _repository.GetGenderById(id);
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Gender");
            }
            return RedirectToAction("Gender");
        }

        #endregion

        

                //#region Location-Setup
        //        public async Task<IActionResult> Location()
        //        {
        //            var result = await _repository.GetLocations();
        //            return View(result);
        //        }
        //        [HttpGet]
        //        public IActionResult AddLocation()
        //        {
        //            return View();
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> AddLocation(AddLocation location)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var newLocation = new Location
        //                {
        //                    LocationCode = location.Code,
        //                    Description = location.Description,
        //                    ShortDescripiton = location.ShortDescription,
        //                    IsActive = location.IsActive
        //                };
        //                await _repository.AddAsync(newLocation);
        //                if (await _repository.SaveChanges())
        //                {
        //                    return RedirectToAction("Location");
        //                }
        //                ModelState.AddModelError("", "Error While Saving to Database");
        //            }
        //            return View(location);
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> UpdateLocation(int id)
        //        {
        //            var temp = await _repository.GetLocationById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            var Location = new UpdateLocation
        //            {
        //                Id = temp.LocationId,
        //                Code = temp.LocationCode,
        //                Description = temp.Description,
        //                ShortDescription = temp.ShortDescripiton,
        //                IsActive = temp.IsActive
        //            };
        //            return View(Location);
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> UpdateLocation(UpdateLocation location)
        //        {
        //            var temp = await _repository.GetLocationById(location.Id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            temp.LocationCode = location.Code;
        //            temp.Description = location.Description;
        //            temp.ShortDescripiton = location.ShortDescription;
        //            temp.IsActive = location.IsActive;
        //            await _repository.UpdateAsync(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Location");
        //            }
        //            ModelState.AddModelError("", "Error While Saving to Database");
        //            return View(location);
        //        }

        //        [HttpGet]
        //        public async Task<IActionResult> DeleteLocation(int id)
        //        {
        //            var temp = await _repository.GetLocationById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            await _repository.Delete(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Location");
        //            }
        //            return RedirectToAction("Location");
        //        }
        //        #endregion




        //        #region Grade-Setup

        //        public async Task<IActionResult> Grade()
        //        {
        //            var result = await _repository.GetGrades();
        //            return View(result);
        //        }

        //        [HttpGet]
        //        public async Task<IActionResult> AddGrade()
        //        {
        //            ViewBag.Campuses = await _repository.GetCampuses();
        //            return View();
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> AddGrade(Setup Grade)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var newGrade = new Grade
        //                {
        //                    Code = Grade.Code,
        //                    Description = Grade.Description,
        //                    ShortDescripiton = Grade.ShortDescription,
        //                    IsActive = Grade.IsActive,
        //                    CampusID = Grade.CampusId
        //                };
        //                await _repository.AddAsync(newGrade);
        //                if (await _repository.SaveChanges())
        //                {
        //                    return RedirectToAction("Grade");
        //                }
        //                ModelState.AddModelError("", "Error While Saving to Database");
        //            }
        //            return View(Grade);
        //        }

        //        [HttpGet]
        //        public async Task<IActionResult> UpdateGrade(int id)
        //        {
        //            var temp = await _repository.GetGradeById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            var grade = new UpdateSetup
        //            {
        //                Id = temp.GradeId,
        //                Code = temp.Code,
        //                Description = temp.Description,
        //                ShortDescription = temp.ShortDescripiton,
        //                CampusId = temp.CampusID,
        //                IsActive = temp.IsActive
        //            };
        //            return View(grade);
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> UpdateGrade(UpdateSetup Grade)
        //        {
        //            var temp = await _repository.GetGradeById(Grade.Id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            temp.Code = Grade.Code;
        //            temp.Description = Grade.Description;
        //            temp.ShortDescripiton = Grade.ShortDescription;
        //            temp.IsActive = Grade.IsActive;
        //            temp.CampusID = Grade.CampusId;
        //            await _repository.UpdateAsync(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Grade");
        //            }
        //            ModelState.AddModelError("", "Error While Saving to Database");
        //            return View(Grade);
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> DeleteGrade(int id)
        //        {
        //            var temp = await _repository.GetGradeById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            await _repository.Delete(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Grade");
        //            }
        //            return RedirectToAction("Grade");
        //        }
        //        #endregion

        //        #region EmployeeType-Setup

        //        public async Task<IActionResult> EmployeeType()
        //        {
        //            var result = await _repository.GetEmployeeTypes();
        //            return View(result);
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> AddEmployeeType()
        //        {
        //            ViewBag.Campuses = await _repository.GetCampuses();
        //            return View();
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> AddEmployeeType(Setup EmployeeType)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var newEmployeeType = new EmployeeType
        //                {
        //                    Code = EmployeeType.Code,
        //                    Description = EmployeeType.Description,
        //                    ShortDescripiton = EmployeeType.ShortDescription,
        //                    IsActive = EmployeeType.IsActive,
        //                    CampusID = EmployeeType.CampusId
        //                };
        //                await _repository.AddAsync(newEmployeeType);
        //                if (await _repository.SaveChanges())
        //                {
        //                    return RedirectToAction("EmployeeType");
        //                }
        //                ModelState.AddModelError("", "Error While Saving to Database");
        //            }
        //            return View(EmployeeType);
        //        }

        //        [HttpGet]
        //        public async Task<IActionResult> UpdateEmployeeType(int id)
        //        {
        //            var temp = await _repository.GetEmployeeTypeById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            var grade = new UpdateSetup
        //            {
        //                Id = temp.EmployeeTypeId,
        //                Code = temp.Code,
        //                Description = temp.Description,
        //                ShortDescription = temp.ShortDescripiton,
        //                CampusId = temp.CampusID,
        //                IsActive = temp.IsActive
        //            };
        //            return View(grade);
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> UpdateEmployeeType(UpdateSetup EmployeeType)
        //        {
        //            var temp = await _repository.GetEmployeeTypeById(EmployeeType.Id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            temp.Code = EmployeeType.Code;
        //            temp.Description = EmployeeType.Description;
        //            temp.ShortDescripiton = EmployeeType.ShortDescription;
        //            temp.IsActive = EmployeeType.IsActive;
        //            temp.CampusID = EmployeeType.CampusId;
        //            await _repository.UpdateAsync(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("EmployeeType");
        //            }
        //            ModelState.AddModelError("", "Error While Saving to Database");
        //            return View(EmployeeType);
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> DeleteEmployeeType(int id)
        //        {
        //            var temp = await _repository.GetEmployeeTypeById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            await _repository.Delete(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Section");
        //            }
        //            return RedirectToAction("Section");
        //        }
        //        #endregion

        //        #region Shift-Setup
        //        public async Task<IActionResult> Shift()
        //        {
        //            var result = await _repository.GetShifts();
        //            return View(result);
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> AddShift()
        //        {
        //            ViewBag.Campuses = await _repository.GetCampuses();
        //            return View();
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> AddShift(Setup Shift)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var newShift = new Shift
        //                {
        //                    Code = Shift.Code,
        //                    Description = Shift.Description,
        //                    ShortDescripiton = Shift.ShortDescription,
        //                    IsActive = Shift.IsActive,
        //                    CampusID = Shift.CampusId
        //                };
        //                await _repository.AddAsync(newShift);
        //                if (await _repository.SaveChanges())
        //                {
        //                    return RedirectToAction("Shift");
        //                }
        //                ModelState.AddModelError("", "Error While Saving to Database");
        //            }
        //            return View(Shift);
        //        }

        //        [HttpGet]
        //        public async Task<IActionResult> UpdateShift(int id)
        //        {
        //            var temp = await _repository.GetShiftById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            var model = new UpdateSetup
        //            {
        //                Id = temp.ShiftId,
        //                Code = temp.Code,
        //                Description = temp.Description,
        //                ShortDescription = temp.ShortDescripiton,
        //                IsActive = temp.IsActive,
        //                CampusId = temp.CampusID
        //            };
        //            return View(model);
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> UpdateShift(UpdateSetup shift)
        //        {
        //            var temp = await _repository.GetShiftById(shift.Id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            temp.Code = shift.Code;
        //            temp.Description = shift.Description;
        //            temp.ShortDescripiton = shift.ShortDescription;
        //            temp.IsActive = shift.IsActive;
        //            temp.CampusID = shift.CampusId;
        //            await _repository.UpdateAsync(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Shift");
        //            }
        //            ModelState.AddModelError("", "Error While Saving to Database");
        //            return View(shift);
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> DeleteShift(int id)
        //        {
        //            var temp = await _repository.GetShiftById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            await _repository.Delete(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Shift");
        //            }
        //            return RedirectToAction("Shift");
        //        }
        //        #endregion

        //        #region Section-Setup
        //        public async Task<IActionResult> Section()
        //        {
        //            var result = await _repository.GetSections();
        //            return View(result);
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> AddSection()
        //        {
        //            ViewBag.Campuses = await _repository.GetCampuses();
        //            return View();
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> AddSection(Setup Section)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var newSection = new Section
        //                {
        //                    Code = Section.Code,
        //                    Description = Section.Description,
        //                    ShortDescripiton = Section.ShortDescription,
        //                    IsActive = Section.IsActive,
        //                    CampusID = Section.CampusId
        //                };
        //                await _repository.AddAsync(newSection);
        //                if (await _repository.SaveChanges())
        //                {
        //                    return RedirectToAction("Section");
        //                }
        //                ModelState.AddModelError("", "Error While Saving to Database");
        //            }
        //            return View(Section);
        //        }

        //        [HttpGet]
        //        public async Task<IActionResult> UpdateSection(int id)
        //        {
        //            var temp = await _repository.GetSectionById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            var model = new UpdateSetup
        //            {
        //                Id = temp.SectionId,
        //                Code = temp.Code,
        //                Description = temp.Description,
        //                ShortDescription = temp.ShortDescripiton,
        //                IsActive = temp.IsActive,
        //                CampusId = temp.CampusID
        //            };
        //            return View(model);
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> UpdateSection(UpdateSetup Section)
        //        {
        //            var temp = await _repository.GetSectionById(Section.Id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            temp.Code = Section.Code;
        //            temp.Description = Section.Description;
        //            temp.ShortDescripiton = Section.ShortDescription;
        //            temp.IsActive = Section.IsActive;
        //            temp.CampusID = Section.CampusId;
        //            await _repository.UpdateAsync(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Section");
        //            }
        //            ModelState.AddModelError("", "Error While Saving to Database");
        //            return View(Section);
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> DeleteSection(int id)
        //        {
        //            var temp = await _repository.GetSectionById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            await _repository.Delete(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Section");
        //            }
        //            return RedirectToAction("Section");
        //        }
        //        #endregion

        //        #region Project-Setup
        //        public async Task<IActionResult> Project()
        //        {
        //            var result = await _repository.GetProjects();
        //            return View(result);
        //        }

        //        [HttpGet]
        //        public async Task<IActionResult> AddProject()
        //        {
        //            ViewBag.Campuses = await _repository.GetCampuses();
        //            return View();
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> AddProject(Setup Project)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var newProject = new Project
        //                {
        //                    Code = Project.Code,
        //                    Description = Project.Description,
        //                    ShortDescripiton = Project.ShortDescription,
        //                    IsActive = Project.IsActive,
        //                    CampusID = Project.CampusId
        //                };
        //                await _repository.AddAsync(newProject);
        //                if (await _repository.SaveChanges())
        //                {
        //                    return RedirectToAction("Project");
        //                }
        //                ModelState.AddModelError("", "Error While Saving to Database");
        //            }
        //            return View(Project);
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> UpdateProject(int id)
        //        {
        //            var temp = await _repository.GetSectionById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            var model = new UpdateSetup
        //            {
        //                Id = temp.SectionId,
        //                Code = temp.Code,
        //                Description = temp.Description,
        //                ShortDescription = temp.ShortDescripiton,
        //                IsActive = temp.IsActive,
        //                CampusId = temp.CampusID
        //            };
        //            return View(model);
        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> UpdateProject(UpdateSetup Project)
        //        {
        //            var temp = await _repository.GetSectionById(Project.Id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            temp.Code = Project.Code;
        //            temp.Description = Project.Description;
        //            temp.ShortDescripiton = Project.ShortDescription;
        //            temp.IsActive = Project.IsActive;
        //            temp.CampusID = Project.CampusId;
        //            await _repository.UpdateAsync(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Project");
        //            }
        //            ModelState.AddModelError("", "Error While Saving to Database");
        //            return View(Project);
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> DeleteProject(int id)
        //        {
        //            var temp = await _repository.GetProjectById(id);
        //            if (temp == null)
        //            {
        //                return NotFound();
        //            }
        //            await _repository.Delete(temp);
        //            if (await _repository.SaveChanges())
        //            {
        //                return RedirectToAction("Project");
        //            }
        //            return RedirectToAction("Project");
        //        }
        //        #endregion
    }
}