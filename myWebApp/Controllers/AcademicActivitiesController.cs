using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using myWebApp.ViewModels.AcademicActivities;

namespace myWebApp.Controllers
{
    [Authorize]
    public class AcademicActivitiesController : Controller
    {
        private readonly IEFRepository _repository;
        private readonly SchoolDbContext _db;
        private readonly IWebHostEnvironment _Environment;
        public AcademicActivitiesController(IEFRepository repository, SchoolDbContext db, IWebHostEnvironment Environment)
        {
            _repository = repository;
            _db = db;
            _Environment = Environment;
        }


        #region Subject
        [Authorize(Policy = "Diary.Read")]
        public async Task<IActionResult> Diary()
        {
            if (User.IsInRole("Parent"))
            {
                int ParentId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
                ViewBag.Diary = await (from a in _db.Diaries
                                       join b in _db.Tests on a.TestId equals b.TestId into DiaryTest
                                       from test in DiaryTest.DefaultIfEmpty()
                                       join c in _db.Sections on a.ClassId equals c.SectionId into DiarySection
                                       from Class in DiarySection.DefaultIfEmpty()
                                       join d in _db.Subjects on a.SubjectId equals d.SubjectId into DiarySubject
                                       from subject in DiarySubject.DefaultIfEmpty()
                                       join e in _db.Grades on Class.GradeId equals e.GradeId into DiaryGrade
                                       from grade in DiaryGrade.DefaultIfEmpty()
                                       from student in _db.Students
                                       where student.ParentId == ParentId
                                       select new
                                       {
                                           HomeWork = a.HomeWork,
                                           ClassWork = a.ClassWork,
                                           Test = test.TestTitle == null ? null : test.TestTitle,
                                           Subject = subject.SubjectName == null ? null : subject.SubjectName,
                                           ClassName = grade.GradeName + Class.SectionName,
                                           DiaryId = a.DiaryId,
                                           IsActive = a.IsActive
                                       }).ToListAsync();
            }
            ViewBag.Diary = await (from a in _db.Diaries
                                   join b in _db.Tests on a.TestId equals b.TestId into DiaryTest
                                   from test in DiaryTest.DefaultIfEmpty()
                                   join c in _db.Sections on a.ClassId equals c.SectionId into DiarySection
                                   from Class in DiarySection.DefaultIfEmpty()
                                   join d in _db.Subjects on a.SubjectId equals d.SubjectId into DiarySubject
                                   from subject in DiarySubject.DefaultIfEmpty()
                                   join e in _db.Grades on Class.GradeId equals e.GradeId into DiaryGrade
                                   from grade in DiaryGrade.DefaultIfEmpty()
                                   select new
                                   {
                                       HomeWork = a.HomeWork,
                                       ClassWork = a.ClassWork,
                                       Test = test.TestTitle == null ? null:test.TestTitle,
                                       Subject = subject.SubjectName == null ? null: subject.SubjectName,
                                       ClassName = grade.GradeName+Class.SectionName,
                                       DiaryId = a.DiaryId,
                                       IsActive = a.IsActive
                                   }).ToListAsync();
            return View();
        }
        [Authorize(Policy = "Diary.Create")]
        [HttpGet]
        public async Task<IActionResult> AddDiary()
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Classes = await (from a in _db.SubjectTeacherAllocations
                                     from b in _db.Sections
                                     from c in _db.Grades
                                     where a.EmployeeId == empId && a.SectionId == b.SectionId && c.GradeId == b.GradeId
                                     select new
                                     {
                                         SectionId = b.SectionId,
                                         SectionName = c.GradeName + b.SectionName

                                     }).ToListAsync();
            return View();
        }
        [Authorize(Policy = "Diary.Create")]
        [HttpPost]
        public async Task<IActionResult> AddDiary(DiaryVM diary)
        {
            var newDiary = new Diary
            {
                ClassId = diary.ClassId,
                ClassWork = diary.ClassWork,
                HomeWork = diary.HomeWork,
                SubjectId = diary.SubjectId,
                TestId = diary.TestId,
                Date = DateTime.Now
            };
            await _repository.AddAsync(newDiary);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Diary");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(diary);
        }
        [Authorize(Policy = "Diary.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateDiary(int DiaryId)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Classes = await (from a in _db.SubjectTeacherAllocations
                                     from b in _db.Sections
                                     where a.EmployeeId == empId && a.SectionId == b.SectionId
                                     select b).ToListAsync();
            var temp = await _db.Diaries.Where(x => x.DiaryId == DiaryId).FirstOrDefaultAsync();
            if (temp == null)
            {
                return NotFound();
            }
            var Diary = new DiaryVM
            {
                ClassId = temp.ClassId,
                ClassWork = temp.ClassWork,
                HomeWork = temp.HomeWork,
                DiaryId = temp.DiaryId,
                IsActive = temp.IsActive,
            };
            return View(temp);
        }
        [Authorize(Policy = "DailyDiary.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateDiary(DiaryVM diary)
        {
            var temp = await _db.Diaries.Where(x => x.DiaryId == diary.DiaryId).FirstOrDefaultAsync();
            if (temp == null)
            {
                return NotFound();
            }
            temp.HomeWork = diary.HomeWork;
            temp.IsActive = diary.IsActive;
            temp.ClassWork = diary.ClassWork;
            temp.SubjectId = diary.SubjectId;
            temp.ClassId = diary.ClassId;
            temp.TestId = diary.TestId;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Diary");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(diary);
        }
        [Authorize(Policy = "Diary.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteDiary(int DiaryId)
        {
            var temp = await _db.Diaries.Where(x => x.DiaryId == DiaryId).FirstOrDefaultAsync();
            temp.IsActive = false;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Diary");
            }
            ModelState.AddModelError("", "Error While Saving in Database!");
            return View(DiaryId);
        }
        #endregion

        #region Book
        [Authorize(Policy = "ClassTests.Read")]
        public async Task<IActionResult> Test(int sectionId)
        {
            ViewBag.Tests = await (from a in _db.Tests
                                   join b in _db.Subjects on a.SubjectId equals b.SubjectId into TestSubject
                                   from subject in TestSubject.DefaultIfEmpty()
                                   join c in _db.Sections on a.ClassId equals c.SectionId into TestClass
                                   from Class in TestClass.DefaultIfEmpty()
                                   join d in _db.Books on a.BookId equals d.BookId into TestBook
                                   from book in TestBook.DefaultIfEmpty()
                                   join f in _db.Grades on Class.GradeId equals f.GradeId into TestGrade
                                   from grade in TestGrade.DefaultIfEmpty()
                                   select new
                                   {
                                       TestTitle = a.TestTitle,
                                       TestId = a.TestId,
                                       Book = book.BookName,
                                       Subject = subject.SubjectName,
                                       Class = grade.GradeName + Class.SectionName,
                                       IsActive = a.IsActive
                                   }).ToListAsync();
            return View();
        }
        [Authorize(Policy = "ClassTests.Create")]
        [HttpGet]
        public async Task<IActionResult> AddTest()
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Classes = await (from a in _db.SubjectTeacherAllocations
                                     from b in _db.Sections
                                     from c in _db.Grades
                                     where a.EmployeeId == empId && a.SectionId == b.SectionId && c.GradeId == b.GradeId
                                     select new
                                     {
                                         SectionId = b.SectionId,
                                         SectionName =  c.GradeId + b.SectionName
                                     }).Distinct().ToListAsync();
            return View();
        }
        [Authorize(Policy = "ClassTests.Create")]
        [HttpPost]
        public async Task<IActionResult> AddTest(TestVM test)
        {
            var newTest = new Test
            {
                BookId = test.BookId,
                ClassId = test.ClassId,
                ObtainedMarks = test.ObtainedMarks,
                TotalMarks = test.TotalMarks,
                SubjectId = test.SubjectId,
                TestTitle = test.TestTitle
            };
            await _repository.AddAsync(newTest);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Test");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(test);
        }
        [Authorize(Policy = "ClassTests.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateTest(int TestId)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Classes = await (from a in _db.SubjectTeacherAllocations
                                     from b in _db.Sections
                                     where a.EmployeeId == empId && a.SectionId == b.SectionId
                                     select b).ToListAsync();
            var test = await _db.Tests.Where(x => x.TestId == TestId).FirstOrDefaultAsync();
            return View();
        }
        [Authorize(Policy = "ClassTests.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateTest(TestVM test)
        {
            var temp = await _db.Tests.Where(x => x.TestId == test.TestId).FirstOrDefaultAsync();
            if (temp == null)
            {
                return NotFound();
            }
            //if (book.IsActive == 1)
            //{
            //    active = true;
            //}
            temp.TestTitle = test.TestTitle;
            temp.TotalMarks = test.TotalMarks;
            temp.ObtainedMarks = test.ObtainedMarks;
            temp.BookId = test.BookId;
            temp.SubjectId = test.SubjectId;
            temp.ClassId = test.ClassId;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Test");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(test);
        }
        [Authorize(Policy = "ClassTests.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteTest(int TestId)
        {
            var temp = await _db.Tests.Where(x => x.TestId == TestId).FirstOrDefaultAsync();
            temp.IsActive = false;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Test");
            }
            ModelState.AddModelError("", "Error While Saving in Database!");
            return View(TestId);
        }
        #endregion

        #region Dynamic-Data

        public async Task<JsonResult> GetDiarySubjects(int ClassId)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var subjects = await (from a in _db.SubjectTeacherAllocations
                                  from b in _db.Books
                                  from c in _db.Subjects
                                  where b.BookId == a.BookId && a.EmployeeId == empId && c.SubjectId == b.SubjectId && a.SectionId == ClassId
                                  select c).Distinct().ToListAsync();
            return Json(subjects);
        }

        public async Task<JsonResult> GetDiaryTests(int SubjectId)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var subjects = await _db.Tests.Where(x => x.SubjectId == SubjectId).Distinct().ToListAsync();
            return Json(subjects);
        }

        public async Task<JsonResult> GetTestSubjects(int ClassId)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var subjects = await (from a in _db.SubjectTeacherAllocations
                                          from b in _db.Books
                                          from c in _db.Subjects
                                          where a.EmployeeId == empId && b.BookId == a.BookId && c.SubjectId == b.SubjectId && a.SectionId == ClassId
                                          select c).Distinct().ToListAsync();
            return Json(subjects);
        }
        public async Task<JsonResult> GetTestBooks(int SubjectId)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var subjects = await (from a in _db.SubjectTeacherAllocations
                                  from b in _db.Books
                                  where a.EmployeeId == empId && b.BookId == a.BookId && b.SubjectId == SubjectId
                                  select b).Distinct().ToListAsync();
            return Json(subjects);
        }

        #endregion
    }
}