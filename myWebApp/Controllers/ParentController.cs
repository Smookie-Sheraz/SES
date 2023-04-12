using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myWebApp.ViewModels.HumanResource;
using myWebApp.ViewModels.Parent;
using myWebApp.ViewModels.Student;
using System.Security.Claims;

namespace myWebApp.Controllers
{
    [Authorize]
    public class ParentController : Controller
    {
        private readonly SchoolDbContext _db;
        private readonly IEFRepository _repository;
        public ParentController(SchoolDbContext db, IEFRepository repository)
        {
            _db = db;
            _repository = repository;
        }
        [Authorize(Policy = "Parent.Read")]
        [HttpGet]
        public async Task<IActionResult> Parent()
        {
            ViewBag.Parents = await _db.Parents.Where(x => x.Status == true).ToListAsync();
            return View();
        }
        [Authorize(Policy = "Parent.Create")]
        [HttpGet]
        public IActionResult AddParent()
        {
            return View();
        }
        [Authorize(Policy = "Parent.Create")]
        [HttpPost]
        public async Task< IActionResult > AddParent(ParentVM parent)
        {
            //if (ModelState.IsValid)
            //{
                var newParent = new Parent();
                newParent.ParentType = parent.ParentType;
                newParent.FName = parent.FName;
                newParent.LName = parent.LName;
                newParent.RegistraionDate = parent.RegistraionDate;
                newParent.Gender = parent.Gender;
                newParent.ITSNumber = parent.ITSNumber;
                newParent.Phone = parent.Phone;
                newParent.Mobile = parent.Mobile;
                newParent.SecondMobile = parent.SecondMobile;
                newParent.Address = parent.Address;
                newParent.SecondAddress = parent.SecondAddress;
                newParent.Username = parent.Username;
                newParent.Email = parent.Email;
                newParent.SecondEmail = parent.SecondEmail;
                newParent.AdmissionEmail = parent.AdmissionEmail;
                newParent.Status = parent.Status;
                newParent.CNIC = parent.CNIC;
                newParent.PassportNo = parent.PassportNo;
                newParent.VisaNo = parent.VisaNo;
                newParent.PassportValidity = parent.PassportValidity;
                newParent.VisaValidity = parent.VisaValidity;
                newParent.ResidentCardNo = parent.ResidentCardNo;
                newParent.FamilyId = parent.FamilyId;
                newParent.MaritalStatus = parent.MaritalStatus;
                newParent.Occupation = parent.Occupation;
                newParent.Designation = parent.Designation;
                newParent.Employer = parent.Employer;
                newParent.OfficeAddress = parent.OfficeAddress;
                newParent.OfficeNumber = parent.OfficeNumber;
                newParent.DegreeQualification = parent.DegreeQualification;
                newParent.EducationInstituion = parent.EducationInstituion;
                newParent.InstituionStartDate = parent.InstituionStartDate;
                newParent.InstituionEndDate = parent.InstituionEndDate;
                newParent.VaccinationFirstDose = parent.VaccinationFirstDose;
                newParent.VaccinationSecondDose = parent.VaccinationSecondDose;
                newParent.VaccinationThirdDose = parent.VaccinationThirdDose;
                newParent.VaccinationFourthDose = parent.VaccinationFourthDose;
                newParent.FirstContactName = parent.FirstContactName;
                newParent.FirstContactEmail = parent.FirstContactEmail;
                newParent.FirstContactRelation = parent.FirstContactRelation;
                newParent.FirstContactOfficeNo = parent.FirstContactOfficeNo;
                newParent.FirstContactAddress = parent.FirstContactAddress;
                newParent.SecondContactName = parent.SecondContactName;
                newParent.SecondContactEmail = parent.SecondContactEmail;
                newParent.SecondContactRelation = parent.SecondContactRelation;
                newParent.SecondContactOfficeNo = parent.SecondContactOfficeNo;
                newParent.SecondContactAddress = parent.SecondContactAddress;
                newParent.ThirdContactName = parent.ThirdContactName;
                newParent.ThirdContactEmail = parent.ThirdContactEmail;
                newParent.ThirdContactRelation = parent.ThirdContactRelation;
                newParent.ThirdContactOfficeNo = parent.ThirdContactOfficeNo;
                newParent.ThirdContactAddress = parent.ThirdContactAddress;
                newParent.FourthContactName = parent.FourthContactName;
                newParent.FourthContactEmail = parent.FourthContactEmail;
                newParent.FourthContactRelation = parent.FourthContactRelation;
                newParent.FourthContactOfficeNo = parent.FourthContactOfficeNo;
                newParent.FourthContactAddress = parent.FourthContactAddress;
                await _repository.AddAsync(newParent);
                if(await _repository.SaveChanges())
                {
                var RecentParent = await _db.Parents.OrderBy(x => x.ParentId).LastOrDefaultAsync();
                var ifUserExists = await _db.Users.Where(x => x.Email == RecentParent.Email && x.IsActive == true).FirstOrDefaultAsync();
                if (ifUserExists == null)
                {
                    var user = new Entities.Models.User
                    {
                        FName = RecentParent.FName,
                        LName = RecentParent.LName,
                        Email = RecentParent.Email,
                        Password = parent.Password,
                        UserName = parent.Username,
                    };
                    await _repository.AddAsync(user);
                    if (await _repository.SaveChanges())
                    {
                        return RedirectToAction("Parent");
                    }
                    ModelState.AddModelError("", "Error While Saving to Database");
                }
                else
                {
                    return RedirectToAction("Parent");
                }
            }
                else
                {
                    ModelState.AddModelError("", "Error While Saving in the Database!");
                }
            //}
        return View(parent);
        }
        [Authorize(Policy = "Parent.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateParent(int ParentId)
        {
            var parent = await _db.Parents.Where(x => x.ParentId == ParentId).FirstOrDefaultAsync();
            var credentials = await _db.Users.Where(x => x.Email == parent.Email).FirstOrDefaultAsync();
            ParentVM newParent = new ParentVM();
            newParent.ParentType = parent.ParentType;
            newParent.FName = parent.FName;
            newParent.LName = parent.LName;
            newParent.RegistraionDate = parent.RegistraionDate;
            newParent.Gender = parent.Gender;
            newParent.ITSNumber = parent.ITSNumber;
            newParent.Phone = parent.Phone;
            newParent.Mobile = parent.Mobile;
            newParent.SecondMobile = parent.SecondMobile;
            newParent.Address = parent.Address;
            newParent.SecondAddress = parent.SecondAddress;
            newParent.Username = parent.Username;
            newParent.UpdateEmail = parent.Email;
            newParent.SecondEmail = parent.SecondEmail;
            newParent.AdmissionEmail = parent.AdmissionEmail;
            newParent.Status = parent.Status;
            newParent.UpdateCNIC = parent.CNIC;
            newParent.PassportNo = parent.PassportNo;
            newParent.VisaNo = parent.VisaNo;
            newParent.PassportValidity = parent.PassportValidity;
            newParent.VisaValidity = parent.VisaValidity;
            newParent.ResidentCardNo = parent.ResidentCardNo;
            newParent.FamilyId = parent.FamilyId;
            newParent.MaritalStatus = parent.MaritalStatus;
            newParent.Occupation = parent.Occupation;
            newParent.Designation = parent.Designation;
            newParent.Employer = parent.Employer;
            newParent.OfficeAddress = parent.OfficeAddress;
            newParent.OfficeNumber = parent.OfficeNumber;
            newParent.DegreeQualification = parent.DegreeQualification;
            newParent.EducationInstituion = parent.EducationInstituion;
            newParent.InstituionStartDate = parent.InstituionStartDate;
            newParent.InstituionEndDate = parent.InstituionEndDate;
            newParent.VaccinationFirstDose = parent.VaccinationFirstDose;
            newParent.VaccinationSecondDose = parent.VaccinationSecondDose;
            newParent.VaccinationThirdDose = parent.VaccinationThirdDose;
            newParent.VaccinationFourthDose = parent.VaccinationFourthDose;
            newParent.FirstContactName = parent.FirstContactName;
            newParent.FirstContactEmail = parent.FirstContactEmail;
            newParent.FirstContactRelation = parent.FirstContactRelation;
            newParent.FirstContactOfficeNo = parent.FirstContactOfficeNo;
            newParent.FirstContactAddress = parent.FirstContactAddress;
            newParent.SecondContactName = parent.SecondContactName;
            newParent.SecondContactEmail = parent.SecondContactEmail;
            newParent.SecondContactRelation = parent.SecondContactRelation;
            newParent.SecondContactOfficeNo = parent.SecondContactOfficeNo;
            newParent.SecondContactAddress = parent.SecondContactAddress;
            newParent.ThirdContactName = parent.ThirdContactName;
            newParent.ThirdContactEmail = parent.ThirdContactEmail;
            newParent.ThirdContactRelation = parent.ThirdContactRelation;
            newParent.ThirdContactOfficeNo = parent.ThirdContactOfficeNo;
            newParent.ThirdContactAddress = parent.ThirdContactAddress;
            newParent.FourthContactName = parent.FourthContactName;
            newParent.FourthContactEmail = parent.FourthContactEmail;
            newParent.FourthContactRelation = parent.FourthContactRelation;
            newParent.FourthContactOfficeNo = parent.FourthContactOfficeNo;
            newParent.FourthContactAddress = parent.FourthContactAddress;
            newParent.Username = credentials.UserName;
            newParent.Password = credentials.Password;
            newParent.PasswordRepeat = credentials.Password;
            return View(newParent);
        }
        [Authorize(Policy = "Parent.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateParent(ParentVM parent)
        {
            //if (ModelState.IsValid)
            //{
                var newParent = await _db.Parents.Where(x => x.ParentId == parent.ParentId)?.FirstOrDefaultAsync();
            var user = await _db.Users.Where(x => x.Email == newParent.Email).FirstOrDefaultAsync();
                newParent.ParentType = parent.ParentType;
                newParent.FName = parent.FName;
                newParent.LName = parent.LName;
                newParent.RegistraionDate = parent.RegistraionDate;
                newParent.Gender = parent.Gender;
                newParent.ITSNumber = parent.ITSNumber;
                newParent.Phone = parent.Phone;
                newParent.Mobile = parent.Mobile;
                newParent.SecondMobile = parent.SecondMobile;
                newParent.Address = parent.Address;
                newParent.SecondAddress = parent.SecondAddress;
                newParent.Username = parent.Username;
                newParent.Email = parent.UpdateEmail;
                newParent.SecondEmail = parent.SecondEmail;
                newParent.AdmissionEmail = parent.AdmissionEmail;
                newParent.Status = parent.Status;
                newParent.CNIC = parent.UpdateCNIC;
                newParent.PassportNo = parent.PassportNo;
                newParent.VisaNo = parent.VisaNo;
                newParent.PassportValidity = parent.PassportValidity;
                newParent.VisaValidity = parent.VisaValidity;
                newParent.ResidentCardNo = parent.ResidentCardNo;
                newParent.FamilyId = parent.FamilyId;
                newParent.MaritalStatus = parent.MaritalStatus;
                newParent.Occupation = parent.Occupation;
                newParent.Designation = parent.Designation;
                newParent.Employer = parent.Employer;
                newParent.OfficeAddress = parent.OfficeAddress;
                newParent.OfficeNumber = parent.OfficeNumber;
                newParent.DegreeQualification = parent.DegreeQualification;
                newParent.EducationInstituion = parent.EducationInstituion;
                newParent.InstituionStartDate = parent.InstituionStartDate;
                newParent.InstituionEndDate = parent.InstituionEndDate;
                newParent.VaccinationFirstDose = parent.VaccinationFirstDose;
                newParent.VaccinationSecondDose = parent.VaccinationSecondDose;
                newParent.VaccinationThirdDose = parent.VaccinationThirdDose;
                newParent.VaccinationFourthDose = parent.VaccinationFourthDose;
                newParent.FirstContactName = parent.FirstContactName;
                newParent.FirstContactEmail = parent.FirstContactEmail;
                newParent.FirstContactRelation = parent.FirstContactRelation;
                newParent.FirstContactOfficeNo = parent.FirstContactOfficeNo;
                newParent.FirstContactAddress = parent.FirstContactAddress;
                newParent.SecondContactName = parent.SecondContactName;
                newParent.SecondContactEmail = parent.SecondContactEmail;
                newParent.SecondContactRelation = parent.SecondContactRelation;
                newParent.SecondContactOfficeNo = parent.SecondContactOfficeNo;
                newParent.SecondContactAddress = parent.SecondContactAddress;
                newParent.ThirdContactName = parent.ThirdContactName;
                newParent.ThirdContactEmail = parent.ThirdContactEmail;
                newParent.ThirdContactRelation = parent.ThirdContactRelation;
                newParent.ThirdContactOfficeNo = parent.ThirdContactOfficeNo;
                newParent.ThirdContactAddress = parent.ThirdContactAddress;
                newParent.FourthContactName = parent.FourthContactName;
                newParent.FourthContactEmail = parent.FourthContactEmail;
                newParent.FourthContactRelation = parent.FourthContactRelation;
                newParent.FourthContactOfficeNo = parent.FourthContactOfficeNo;
                newParent.FourthContactAddress = parent.FourthContactAddress;
                await _repository.UpdateAsync(newParent);
                user.FName = parent.FName;
                user.LName = parent.LName;
                user.Email = parent.Email;
                user.UserName = parent.Username;
                user.Password = parent.Password;
                await _repository.UpdateAsync(user);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Parent");
                }
                else
                {
                    ModelState.AddModelError("", "Error While Saving in the Database!");
                }
            //}
            return View(parent);
        }
        [Authorize(Policy = "Parent.Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteParent(int ParentId)
        {
            var parent = await _db.Parents.Where(x => x.ParentId == ParentId).FirstOrDefaultAsync();
            var user = await _db.Users.Where(x => x.Email == parent.Email).FirstOrDefaultAsync();
            user.IsActive = false;
            parent.Status = false;
            await _repository.UpdateAsync(parent);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Parent");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving in Database!");
            }
            return RedirectToAction("Parent");
        }
        [Authorize(Policy = "Parent.Dashboard")]
        [HttpGet]
        public async Task<IActionResult> Dashboard(int ParentId)
        {
            int parentId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var children = await _db.Students.Where(x => x.ParentId == parentId).ToListAsync();
            return View(children);
        }
        [Authorize(Policy = "StudentLeaveApplication")]
        [HttpGet]
        public IActionResult LeaveApplication(int StudentId)
        {
            LeaveApplicationVM leave = new LeaveApplicationVM();
            var prevLeave = _db.LeaveApplications.Where(x => x.StudentId == StudentId && x.EndDate > DateTime.Now && x.ApplicationStatus == "Approved" && x.IsActive == true).FirstOrDefault();
            leave.MinDate = prevLeave != null ? Convert.ToDateTime(prevLeave.EndDate).ToString("yyyy-MM-dd") : Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
            leave.StudentId = StudentId;
            ViewBag.Leaves = _db.LeaveApplications.Where(x => x.StudentId == StudentId && x.IsActive == true).ToList();
            return View(leave);
        }
        [Authorize(Policy = "StudentLeaveApplication")]
        [HttpPost]
        public async Task<IActionResult> LeaveApplication(LeaveApplicationVM leave)
        {
            int parentId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            if (ModelState.IsValid)
            {
                Entities.Models.LeaveApplication newLeave = new LeaveApplication
                {
                    StudentId = leave.StudentId,
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    Reason = leave.Reason
                };
                await _repository.AddAsync(newLeave);
                if(await _repository.SaveChanges())
                {
                    RedirectToAction("LeaveApplication", new { StudentId = leave.StudentId });
                }
                ModelState.AddModelError("", "Error While Saving in Database!");
            }
            return View(leave);
        }
        [Authorize(Policy = "StudentLeaveApplication")]
        [HttpPost]
        public async Task<IActionResult> DeleteLeaveApplication(int LeaveId, int studentId)
        {
            if (ModelState.IsValid)
            {
                var LeaveApp = await _db.LeaveApplications.Where(x => x.StudentId == LeaveId).FirstOrDefaultAsync();
                LeaveApp.IsActive = false;
                await _repository.UpdateAsync(LeaveApp);
                if(await _repository.SaveChanges())
                {
                    return RedirectToAction("LeaveApplication", new { StudentId = studentId });
                }
                ModelState.AddModelError("", "Error While Saving in Database");
            }
            return View(LeaveId);
        }
    }
}
