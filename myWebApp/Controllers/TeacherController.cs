using Entities.Models;
using myWebApp.ViewModels.Teacher;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace myWebApp.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        public readonly SchoolDbContext _db;
        public readonly IEFRepository _repository;
        public TeacherController(SchoolDbContext db, IEFRepository repository)
        {
            _db = db;
            _repository = repository;
        }
        #region TeachingMethodology
        [HttpGet]
        public async Task<IActionResult> TeachingMethodology()
        {
            ViewBag.TMethods = await _db.TeachingMethodologies.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TeachingMethodology(TeachingMethodologyVM TMethod)
        {
            var newTeachingMethodology = new TeachingMethodology
            {
                TMethodologyName = TMethod.TMethodName
            };
            await _repository.AddAsync(newTeachingMethodology);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("TeachingMethodology");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(TMethod);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateTeachingMethodology(int id)
        {
            var temp = await _db.TeachingMethodologies.Where(x => x.TeachingMethodologyId == id).FirstOrDefaultAsync();

            if (temp == null)
            {
                return NotFound();
            }
            var TeachingMethodology = new TeachingMethodologyVM
            {
                TeachingMethodologyId = temp.TeachingMethodologyId,
                TMethodName = temp.TMethodologyName,
                IsActive = (bool)temp.IsActive
            };
            return View(TeachingMethodology);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTeachingMethodology(TeachingMethodologyVM TMethod)
        {
            var temp = await _db.TeachingMethodologies.Where(x => x.TeachingMethodologyId == TMethod.TeachingMethodologyId).FirstOrDefaultAsync();
            if (temp == null)
            {
                return NotFound();
            }
            temp.TMethodologyName = TMethod.TMethodName;
            temp.IsActive = TMethod.IsActive;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("TeachingMethodology");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(TMethod);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteTeachingMethodology(int id)
        {
            var temp = await _db.TeachingMethodologies.Where(x => x.TeachingMethodologyId == id).FirstOrDefaultAsync();
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("TeachingMethodology");
            }
            return RedirectToAction("TeachingMethodology");
        }
        #endregion
        [Authorize(Policy = "Student.Attendance")]
        [HttpGet]
        public IActionResult Attendance()
        {
            var userId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            StudentAttendanceVM studentAttendanceVM = new StudentAttendanceVM();
            studentAttendanceVM.ClassName = (from a in _db.Sections
                                            from b in _db.Grades
                                            where a.ClassTeacherId == userId && b.GradeId == a.GradeId
                                            select b.GradeName + " " + a.SectionName).FirstOrDefault();
            studentAttendanceVM.Date = DateTime.Now.ToString("dd-MMM-yyyy");
            studentAttendanceVM.TeacherName = User?.FindFirst("Username").ToString();
            studentAttendanceVM.AttendanceList = (from a in _db.Sections
                                                    from b in _db.Students
                                                    join d in _db.LeaveApplications on b.StudentId equals d.StudentId into StudentLeaves
                                                    from leaves in StudentLeaves.DefaultIfEmpty()
                                                    from c in _db.Parents
                                                    where a.ClassTeacherId == userId && b.ClassId == a.SectionId && c.ParentId == b.ParentId && b.Status == true
                                                    select new StudentAttendanceList
                                                    {
                                                        StudentId = b.StudentId,
                                                        ClassId = b.ClassId,
                                                        RollNo = b.RollNo,
                                                        StudentName = b.FName + " " + b.LName,
                                                        ParentName = c.FName + " " + c.LName,
                                                        LeaveReason = leaves.StartDate <= DateTime.Now && leaves.IsActive == true ? leaves.Reason:null,
                                                        StartDate = leaves.StartDate <= DateTime.Now && leaves.IsActive == true ? Convert.ToDateTime(leaves.StartDate).ToString("dd-MMM-yyyy"): null,
                                                        EndDate = leaves.StartDate <= DateTime.Now && leaves.IsActive == true ? Convert.ToDateTime(leaves.EndDate).ToString("dd-MMM-yyyy"): null,
                                                        LeaveStatus = leaves.EndDate < DateTime.Now && leaves.IsActive == true ? "": leaves.ApplicationStatus
                                                    }).Distinct().ToList();
            return View(studentAttendanceVM);
        }
        [Authorize(Policy = "Student.Attendance")]
        [HttpPost]
        public async Task<IActionResult> Attendance(StudentAttendanceVM studentAttendanceVM)
        {
            if (ModelState.IsValid)
            {
                List<StudentAttendance> studentAttendance = new List<StudentAttendance>();
                studentAttendance = studentAttendanceVM.AttendanceList.Select(x => new StudentAttendance { AttendanceStatus = x.LeaveStatus == "Approved" ? "Leave":x.AttendanceStatus, ClassId = x.ClassId, Date = DateTime.Now, LateOrOnTime = x.OnTimeOrLate, StudentId = x.StudentId }).ToList();
                foreach (var student in studentAttendanceVM.AttendanceList)
                {
                    if(student.StartDate != null)
                    {
                        var leave = _db.LeaveApplications.Where(x => x.StudentId == student.StudentId && x.StartDate == Convert.ToDateTime(student.StartDate)).FirstOrDefault();
                        leave.ApplicationStatus = student.LeaveResponse;
                        await _repository.UpdateAsync(leave);
                    }
                }
                await _db.AddRangeAsync(studentAttendance);
                if(await _repository.SaveChanges())
                {
                    return RedirectToAction("Attendance");
                }
                ModelState.AddModelError("", "Error While Saving in Database!");
            }
            return View(studentAttendanceVM);
        }
    }
}
