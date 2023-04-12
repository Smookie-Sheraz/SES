using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Entities.Models;
using myWebApp.ViewModels.Parent;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using myWebApp.ViewModels.School;
using System.Security.Claims;
using System.Diagnostics.Metrics;
using myWebApp.ViewModels.Student;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;

namespace myWebApp.Controllers
{
    [Authorize]
    public class SchoolController : Controller
    {

        private readonly IEFRepository _repository;
        private readonly SchoolDbContext _db;

        public SchoolController(IEFRepository repository, SchoolDbContext db)
        {
            _repository = repository;
            _db = db;
        }

        #region Letter
        [Authorize(Policy = "Letters.Read")]
        [HttpGet]
        public async Task<IActionResult> Letter()
        {
            if (User.IsInRole("Student") || User.IsInRole("Parent"))
            {
                int studentOrParentId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
                ViewBag.Letters = await (from letter in _db.SchoolLatters
                                         from b in _db.Students
                                         from c in _db.Parents
                                         from d in _db.Employees
                                         where b.StudentId == letter.StudentId && c.ParentId == letter.ParentId && d.EmployeeId == letter.IssuingACId && (letter.StudentId == studentOrParentId || letter.ParentId == studentOrParentId)
                                         select new
                                         {
                                             SchoolLetterId = letter.SchoolLatterId,
                                             StudentId = letter.StudentId,
                                             Body = letter.Body,
                                             Closing = letter.Closing,
                                             IsActive = letter.IsActive,
                                             Salutation = letter.Salutation,
                                             ReceiverContact = letter.ReceiverContact,
                                             ReceiverDesignation = letter.ReceiverDesignation,
                                             ReceiverName = letter.ReceiverName,
                                             SenderContact = letter.SenderContact,
                                             SenderDesignation = letter.SenderDesignation,
                                             SenderName = letter.SenderName,
                                             Date = letter.Date,
                                             SendingDate = letter.SendingDate,
                                             IssuingACName = d.FName + " " + d.LName,
                                             ParentName = c.FName + " " + c.LName,
                                             StudentName = b.FName + " " + b.LName
                                         }).ToListAsync();
            }
            else
            {
                ViewBag.Letters = await (from letter in _db.SchoolLatters
                                         from b in _db.Students
                                         from c in _db.Parents
                                         from d in _db.Employees
                                         where b.StudentId == letter.StudentId && c.ParentId == letter.ParentId && d.EmployeeId == letter.IssuingACId
                                         select new
                                         {
                                             SchoolLetterId = letter.SchoolLatterId,
                                             StudentId = letter.StudentId,
                                             Body = letter.Body,
                                             Closing = letter.Closing,
                                             Salutation = letter.Salutation,
                                             IsActive = letter.IsActive,
                                             ReceiverContact = letter.ReceiverContact,
                                             ReceiverDesignation = letter.ReceiverDesignation,
                                             ReceiverName = letter.ReceiverName,
                                             SenderContact = letter.SenderContact,
                                             SenderDesignation = letter.SenderDesignation,
                                             SenderName = letter.SenderName,
                                             Date = letter.Date,
                                             SendingDate = letter.SendingDate,
                                             IssuingACName = d.FName + " " + d.LName,
                                             ParentName = c.FName + " " + c.LName,
                                             StudentName = b.FName + " " + b.LName
                                         }).ToListAsync();
            }
            return View();
        }
        [Authorize(Policy = "Letters.Create")]
        [HttpGet]
        public IActionResult AddLetter(int StudentId)
        {
            LetterVM letterVM = new LetterVM
            {
                StudentId = StudentId
            };
            return View(letterVM);
        }
        [Authorize(Policy = "Letters.Create")]
        [HttpPost]
        public async Task<IActionResult> AddLetter(LetterVM letter)
        {
            if (ModelState.IsValid)
            {
                int ParentId = (int)_db.Students?.Where(x => x.StudentId == letter.StudentId)?.FirstOrDefault()?.ParentId;
                int ACId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
                SchoolLatter schoolLatter = new SchoolLatter
                {
                    StudentId = letter.StudentId,
                    Body = letter.Body,
                    Closing = letter.Closing,
                    ReceiverContact = letter.ReceiverContact,
                    ReceiverDesignation = letter.ReceiverDesignation,
                    ReceiverName = letter.ReceiverName,
                    SenderContact = letter.SenderContact,
                    SenderDesignation = letter.SenderDesignation,
                    SenderName = letter.SenderName,
                    Date = letter.Date,
                    SendingDate = letter.SendingDate,
                    IssuingACId = ACId,
                    ParentId = ParentId
                };
                await _repository.AddAsync(schoolLatter);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Student", "Student");
                }
                ModelState.AddModelError("", "Error While Saving in Database!");
            }
            return View(letter);
        }
        [Authorize(Policy = "Letters.Update")]
        [HttpGet]
        public IActionResult UpdateLetter(int LetterId)
        {
            LetterVM ULetter = new LetterVM();
            ULetter = (from letter in _db.SchoolLatters
                       from b in _db.Students
                       from c in _db.Parents
                       from d in _db.Employees
                       where b.StudentId == letter.StudentId && c.ParentId == letter.ParentId && letter.SchoolLatterId == LetterId && d.EmployeeId == letter.IssuingACId
                       select new LetterVM
                       {
                           SchoolLatterId = letter.SchoolLatterId,
                           StudentId = letter.StudentId,
                           Body = letter.Body,
                           Closing = letter.Closing,
                           ReceiverContact = letter.ReceiverContact,
                           ReceiverDesignation = letter.ReceiverDesignation,
                           ReceiverName = letter.ReceiverName,
                           SenderContact = letter.SenderContact,
                           SenderDesignation = letter.SenderDesignation,
                           SenderName = letter.SenderName,
                           Date = letter.Date,
                           SendingDate = letter.SendingDate,
                           IssuingACId = letter.IssuingACId,
                           ParentId = letter.ParentId
                       })?.FirstOrDefault();
            var Parent = _db.Parents.Where(x => x.ParentId == ULetter.ParentId).FirstOrDefault();
            var AC = _db.Employees.Where(x => x.EmployeeId == ULetter.IssuingACId).FirstOrDefault();
            ViewBag.ACName = AC.FName + " " + AC.LName;
            ViewBag.ParentName = Parent.FName + " " + Parent.LName;
            return View(ULetter);
        }
        [Authorize(Policy = "Letters.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateLetter(LetterVM letter)
        {
            if (ModelState.IsValid)
            {
                var temp = _db.SchoolLatters.Where(x => x.SchoolLatterId == letter.SchoolLatterId).FirstOrDefault();
                temp.Body = letter.Body;
                temp.Closing = letter.Closing;
                temp.ReceiverContact = letter.ReceiverContact;
                temp.ReceiverDesignation = letter.ReceiverDesignation;
                temp.ReceiverName = letter.ReceiverName;
                temp.SenderContact = letter.SenderContact;
                temp.SenderDesignation = letter.SenderDesignation;
                temp.SenderName = letter.SenderName;
                temp.Date = letter.Date;
                temp.SendingDate = letter.SendingDate;
                await _repository.UpdateAsync(temp);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Letter");
                }
                ModelState.AddModelError("", "Error While Saving in Database!");
            }
            return View(letter);
        }
        [Authorize(Policy = "Letters.Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteLetter(int LetterId)
        {
            var letter = _db.SchoolLatters.Where(x => x.SchoolLatterId == LetterId).FirstOrDefault();
            letter.IsActive = false;
            await _repository.UpdateAsync(letter);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Letter");
            }
            ModelState.AddModelError("", "Error While Saving in Database!");
            return View(LetterId);
        }
        [Authorize(Policy = "LetterDetails")]
        [HttpGet]
        public IActionResult LetterDetails(int LetterId)
        {
            ViewBag.Letter = (from letter in _db.SchoolLatters
                              from b in _db.Students
                              from c in _db.Parents
                              from d in _db.Employees
                              where b.StudentId == letter.StudentId && c.ParentId == letter.ParentId && letter.SchoolLatterId == LetterId && d.EmployeeId == letter.IssuingACId
                              select new
                              {
                                  SchoolLatterId = letter.SchoolLatterId,
                                  StudentId = letter.StudentId,
                                  Body = letter.Body,
                                  Closing = letter.Closing,
                                  Salutation = letter.Salutation,
                                  ReceiverContact = letter.ReceiverContact,
                                  ReceiverDesignation = letter.ReceiverDesignation,
                                  ReceiverName = letter.ReceiverName,
                                  SenderContact = letter.SenderContact,
                                  SenderDesignation = letter.SenderDesignation,
                                  SenderName = letter.SenderName,
                                  Date = letter.Date,
                                  SendingDate = letter.SendingDate,
                                  IssuingACName = d.FName + " " + d.LName,
                                  ParentName = c.FName + " " + c.LName,
                                  StudentName = b.FName + " " + b.LName
                              }).FirstOrDefault();
            return View();
        }
        #endregion

        #region Notice
        [Authorize(Policy = "Notices.Read")]
        [HttpGet]
        public async Task<IActionResult> Notice()
        {
            var notice = await _db.SchoolNotices.ToListAsync();
            return View(notice);
        }
        [Authorize(Policy = "Notices.Create")]
        [HttpGet]
        public IActionResult AddNotice()
        {
            return View();
        }
        [Authorize(Policy = "Notices.Create")]
        [HttpPost]
        public async Task<IActionResult> AddNotice(NoticeVM letter)
        {
            if (ModelState.IsValid)
            {
                //int ParentId = (int)_db.Students?.Where(x => x.StudentId == letter.StudentId)?.FirstOrDefault()?.ParentId;
                int ACId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
                SchoolNotice schoolNotice = new SchoolNotice
                {
                    Body = letter.Body,
                    Date = letter.Date,
                    IssuingACId = ACId,
                    BroadcastDate = letter.BroadcastDate,
                    Recipient = letter.Recipient,
                    Salutation = letter.Salutation,
                    Title = letter.Title
                };
                await _repository.AddAsync(schoolNotice);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Notice", "School");
                }
                ModelState.AddModelError("", "Error While Saving in Database!");
            }
            return View(letter);
        }
        [Authorize(Policy = "Notices.Update")]
        [HttpGet]
        public IActionResult UpdateNotice(int NoticeId)
        {
            NoticeVM UNotice = new NoticeVM();
            UNotice = (from letter in _db.SchoolNotices
                       from d in _db.Employees
                       where d.EmployeeId == letter.IssuingACId
                       select new NoticeVM
                       {
                           Body = letter.Body,
                           Date = letter.Date,
                           BroadcastDate = letter.BroadcastDate,
                           Recipient = letter.Recipient,
                           Salutation = letter.Salutation,
                           Title = letter.Title,
                           SchoolNoticeId = letter.SchoolNoticeId
                       })?.FirstOrDefault();
            //var Parent = _db.Parents.Where(x => x.ParentId == ULetter.ParentId).FirstOrDefault();
            //var AC = _db.Employees.Where(x => x.EmployeeId == ULetter.IssuingACId).FirstOrDefault();
            //ViewBag.ACName = AC.FName + " " + AC.LName;
            //ViewBag.ParentName = Parent.FName + " " + Parent.LName;
            return View(UNotice);
        }
        [Authorize(Policy = "Notices.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateNotice(NoticeVM letter)
        {
            if (ModelState.IsValid)
            {
                var temp = _db.SchoolNotices.Where(x => x.SchoolNoticeId == letter.SchoolNoticeId).FirstOrDefault();
                temp.Body = letter.Body;
                temp.Date = letter.Date;
                temp.BroadcastDate = letter.BroadcastDate;
                temp.Recipient = letter.Recipient;
                temp.Salutation = letter.Salutation;
                temp.Title = letter.Title;
                await _repository.UpdateAsync(temp);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Letter");
                }
                ModelState.AddModelError("", "Error While Saving in Database!");
            }
            return View(letter);
        }
        [Authorize(Policy = "Notices.Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteNotice(int NoticeId)
        {
            var letter = _db.SchoolNotices.Where(x => x.SchoolNoticeId == NoticeId).FirstOrDefault();
            letter.IsActive = false;
            await _repository.UpdateAsync(letter);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Notice");
            }
            ModelState.AddModelError("", "Error While Saving in Database!");
            return View(Notice);
        }
        [Authorize(Policy = "NoticeDetails")]
        [HttpGet]
        public IActionResult NoticeDetails(int NoticeId)
        {
            var notice = _db.SchoolNotices.Where(x => x.SchoolNoticeId == NoticeId).FirstOrDefault();
            return View(notice);
        }
        #endregion
    }
}
