using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myWebApp.ViewModels.HumanResource;
using myWebApp.ViewModels.Student;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace myWebApp.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly SchoolDbContext _db;
        private readonly IEFRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        public StudentController(SchoolDbContext db, IEFRepository repository, IWebHostEnvironment webHostEnvironment, IEmailSender EmailSender)
        {
            _db = db;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = EmailSender;
        }
        [Authorize(Policy = "Student.Read")]
        [HttpGet]
        public async Task<IActionResult> Student()
        {
            if (User.IsInRole("Deputy Coordinator") || User.IsInRole("Director Academics"))
            {
                ViewBag.Students = await (from a in _db.Students
                                          join b in _db.Parents on a.ParentId equals b.ParentId into STParent
                                          from parent in STParent.DefaultIfEmpty()
                                          join d in _db.Sections on a.ClassId equals d.SectionId into STClass
                                          from Class in STClass.DefaultIfEmpty()
                                          join c in _db.Grades on Class.GradeId equals c.GradeId into STGrade
                                          from Grade in STGrade.DefaultIfEmpty()
                                          join e in _db.Employees on Class.ClassTeacherId equals e.EmployeeId into ClassTeacher
                                          from CT in ClassTeacher.DefaultIfEmpty()
                                          select new
                                          {
                                              FName = a.FName,
                                              LName = a.LName,
                                              StudentId = a.StudentId,
                                              Picture = a.Picture,
                                              Status = a.Status,
                                              ClassName = Grade.GradeName + Class.SectionName,
                                              ParentName = parent.FName + " " + parent.LName,
                                              ClassTeacher = CT.FName + " " + CT.LName,
                                              RegNo = a.StudentRegistraionNo,
                                              WhatsAppNo = a.WhastAppNo
                                          }).ToListAsync();
            }
            else
            {
                ViewBag.Students = await (from a in _db.Students
                                          join b in _db.Parents on a.ParentId equals b.ParentId into STParent
                                          from parent in STParent.DefaultIfEmpty()
                                          join d in _db.Sections on a.ClassId equals d.SectionId into STClass
                                          from Class in STClass.DefaultIfEmpty()
                                          join c in _db.Grades on Class.GradeId equals c.GradeId into STGrade
                                          from Grade in STGrade.DefaultIfEmpty()
                                          join e in _db.Employees on Class.ClassTeacherId equals e.EmployeeId into ClassTeacher
                                          from CT in ClassTeacher.DefaultIfEmpty()
                                          where a.Status == true
                                          select new
                                          {
                                              FName = a.FName,
                                              LName = a.LName,
                                              StudentId = a.StudentId,
                                              Picture = a.Picture,
                                              Status = a.Status,
                                              ClassName = Grade.GradeName + Class.SectionName,
                                              ParentName = parent.FName + " " + parent.LName,
                                              ClassTeacher = CT.FName + " " + CT.LName,
                                              RegNo = a.StudentRegistraionNo,
                                              WhatsAppNo = a.WhastAppNo
                                          }).ToListAsync();
            }
            return View();
        }
        [Authorize(Policy = "Student.Create")]
        [HttpGet]
        public IActionResult AddStudent()
        {
            ViewBag.Parents = from a in _db.Parents
                              select new
                              {
                                  ParentId = a.ParentId,
                                  FName = a.FName,
                                  LName = a.LName,
                                  CNIC = a.CNIC
                              };
            ViewBag.Classes = from a in _db.Grades
                              from b in _db.Sections
                              where b.GradeId == a.GradeId
                              select new
                              {
                                  SectionId = b.SectionId,
                                  ClassName = a.GradeName + b.SectionName
                              };
            return View();
        }
        [Authorize(Policy = "Student.Create")]
        [HttpPost]
        public async Task<IActionResult> AddStudent(StudentVM student)
        {
            string StudentImage = student.Picture == null ? null : FileSaver(student.StudentId, student.FName, student.LName, student.Picture);
            var newStudent = new Student();
            if (student.ParentId == null)
            {
                var parent = new Parent
                {
                    FName = student.ParentFName,
                    LName = student.ParentLName,
                    Designation = student.ParentDesignation,
                    Employer = student.ParentEmployer,
                    ParentType = student.ParentType,
                    CNIC = student.ParentCNIC,
                    Mobile = student.ParentMobile,
                    Occupation = student.ParentOccupation,
                    OfficeAddress = student.ParentOfficeAddress,
                    CreatedDate = student.ParentRegistraionDate,
                    Email = student.ParentEmail
                };
                var parentUser = new User
                {
                    FName = student.ParentFName,
                    LName = student.ParentLName,
                    Email = student.ParentEmail,
                    Password = student.ParentEmail,
                    UserName = student.ParentFName + student.ParentLName
                };
                await _repository.AddAsync(parentUser);
                await _repository.AddAsync(parent);
                if (await _repository.SaveChanges())
                {
                    //var EmailHtmlBody = $"<!DOCTYPE html>\r\n<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" lang=\"en\">\r\n\r\n<head>\r\n\t<title></title>\r\n\t<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n\t<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><!--[if mso]><xml><o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml><![endif]--><!--[if !mso]><!-->\r\n\t<link href=\"https://fonts.googleapis.com/css?family=Abril+Fatface\" rel=\"stylesheet\" type=\"text/css\">\r\n\t<link href=\"https://fonts.googleapis.com/css?family=Cabin\" rel=\"stylesheet\" type=\"text/css\"><!--<![endif]-->\r\n\t<style>\r\n\t\t* {{{{\r\n\t\t\tbox-sizing: border-box;\r\n\t\t}}\r\n\r\n\t\tbody {{{{\r\n\t\t\tmargin: 0;\r\n\t\t\tpadding: 0;\r\n\t\t}}\r\n\r\n\t\ta[x-apple-data-detectors] {{{{\r\n\t\t\tcolor: inherit !important;\r\n\t\t\ttext-decoration: inherit !important;\r\n\t\t}}\r\n\r\n\t\t#MessageViewBody a {{\r\n\t\t\tcolor: inherit;\r\n\t\t\ttext-decoration: none;\r\n\t\t}}\r\n\r\n\t\tp {{\r\n\t\t\tline-height: inherit\r\n\t\t}}\r\n\r\n\t\t.desktop_hide,\r\n\t\t.desktop_hide table {{\r\n\t\t\tmso-hide: all;\r\n\t\t\tdisplay: none;\r\n\t\t\tmax-height: 0px;\r\n\t\t\toverflow: hidden;\r\n\t\t}}\r\n\r\n\t\t.image_block img+div {{\r\n\t\t\tdisplay: none;\r\n\t\t}}\r\n\r\n\t\t@media (max-width:670px) {{\r\n\t\t\t.social_block.desktop_hide .social-table {{\r\n\t\t\t\tdisplay: inline-block !important;\r\n\t\t\t}}\r\n\r\n\t\t\t.row-content {{\r\n\t\t\t\twidth: 100% !important;\r\n\t\t\t}}\r\n\r\n\t\t\t.mobile_hide {{\r\n\t\t\t\tdisplay: none;\r\n\t\t\t}}\r\n\r\n\t\t\t.stack .column {{\r\n\t\t\t\twidth: 100%;\r\n\t\t\t\tdisplay: block;\r\n\t\t\t}}\r\n\r\n\t\t\t.mobile_hide {{\r\n\t\t\t\tmin-height: 0;\r\n\t\t\t\tmax-height: 0;\r\n\t\t\t\tmax-width: 0;\r\n\t\t\t\toverflow: hidden;\r\n\t\t\t\tfont-size: 0px;\r\n\t\t\t}}\r\n\r\n\t\t\t.desktop_hide,\r\n\t\t\t.desktop_hide table {{\r\n\t\t\t\tdisplay: table !important;\r\n\t\t\t\tmax-height: none !important;\r\n\t\t\t}}\r\n\t\t}}\r\n\t</style>\r\n</head>\r\n\r\n<body style=\"background-color: #C1C558; margin: 0; padding: 0; -webkit-text-size-adjust: none; text-size-adjust: none;\">\r\n\t<table class=\"nl-container\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #C1C558;\">\r\n\t\t<tbody>\r\n\t\t\t<tr>\r\n\t\t\t\t<td>\r\n\t\t\t\t\t<table class=\"row row-1\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #FFF6F2;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 20px; padding-top: 25px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\" style=\"padding-left:10px;padding-right:10px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: 'Courier New', Courier, monospace\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class style=\"font-size: 12px; font-family: 'Courier New', Courier, 'Lucida Sans Typewriter', 'Lucida Typewriter', monospace; mso-line-height-alt: 18px; color: #CD705C; line-height: 1.5;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; text-align: center; font-size: 12px; mso-line-height-alt: 63px;\"><span style=\"font-size:42px;\">SEERAT EDUCATION SYSTEM</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-2\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #e8bdac; background-image: url('https://d1oco4z2z1fhwp.cloudfront.net/templates/default/291/easter_ani_2.gif'); background-position: top center; background-repeat: repeat;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-top: 60px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\" style=\"padding-bottom:10px;padding-left:60px;padding-right:60px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: sans-serif\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class style=\"font-size: 12px; font-family: 'Cabin', Arial, 'Helvetica Neue', Helvetica, sans-serif; mso-line-height-alt: 18px; color: #555555; line-height: 1.5;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 22px; text-align: center; mso-line-height-alt: 27px;\"><span style=\"font-size:18px;\">Your Credentials to Access the Online Portal of the School are:</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 22px; text-align: center; mso-line-height-alt: 33px;\"><span style=\"font-size:22px;\">Email: {parentUser.Email}</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 22px; text-align: center; mso-line-height-alt: 33px;\"><span style=\"font-size:22px;\">Password: {parentUser.Password}</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-3\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #c1c558;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 10px; padding-top: 20px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: Arial, sans-serif\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class style=\"font-family: 'Abril Fatface', Arial, 'Helvetica Neue', Helvetica, sans-serif; font-size: 12px; mso-line-height-alt: 14.399999999999999px; color: #639175; line-height: 1.2;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; text-align: center; font-size: 12px; mso-line-height-alt: 14.399999999999999px;\"><span style=\"font-size:38px;\">Quality and Excellence in Education</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-4\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #c1c558; background-image: url('https://d1oco4z2z1fhwp.cloudfront.net/templates/default/291/bottom_green.png'); background-repeat: repeat;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 10px; padding-top: 20px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"spacer_block block-1\" style=\"height:20px;line-height:20px;font-size:1px;\">&#8202;</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-5\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #639175;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 25px; padding-top: 20px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"social_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"alignment\" align=\"center\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"social-table\" width=\"111px\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; display: inline-block;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"padding:0 5px 0 0px;\"><a href=\"https://www.facebook.com/\" target=\"_blank\"><img src=\"https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-circle-white/facebook@2x.png\" width=\"32\" height=\"32\" alt=\"Facebook\" title=\"Facebook\" style=\"display: block; height: auto; border: 0;\"></a></td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"padding:0 5px 0 0px;\"><a href=\"https://twitter.com/\" target=\"_blank\"><img src=\"https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-circle-white/twitter@2x.png\" width=\"32\" height=\"32\" alt=\"Twitter\" title=\"Twitter\" style=\"display: block; height: auto; border: 0;\"></a></td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"padding:0 5px 0 0px;\"><a href=\"https://instagram.com/\" target=\"_blank\"><img src=\"https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-circle-white/instagram@2x.png\" width=\"32\" height=\"32\" alt=\"Instagram\" title=\"Instagram\" style=\"display: block; height: auto; border: 0;\"></a></td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"text_block block-2\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: sans-serif\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class style=\"font-size: 12px; font-family: 'Cabin', Arial, 'Helvetica Neue', Helvetica, sans-serif; mso-line-height-alt: 14.399999999999999px; color: #FFFFFF; line-height: 1.2;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 14px; text-align: center; mso-line-height-alt: 16.8px;\">Sattalite Town Quetta.</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t</tbody>\r\n\t</table><!-- End -->\r\n</body>\r\n\r\n</html>";
                    //await _emailSender.SendEmailAsync(parentUser.Email, "Your Update Email Address", EmailHtmlBody);
                }

            }
            var newParent = await _db.Parents.OrderBy(x => x.ParentId).LastOrDefaultAsync();
            newStudent.Address = student.Address;
            newStudent.WhastAppNo = student.WhatsAppNo;
            newStudent.DOB = student.DOB;
            newStudent.AdmissionEmail = student.AdmissionEmail;
            newStudent.AdmissionTestResult = student.AdmissionTestResult;
            newStudent.AdmittedClassOrSection = student.AdmittedClassorSection;
            newStudent.AdmittedSession = student.AdmittedSession;
            newStudent.Allergies = student.Allergies;
            newStudent.BloodGroup = student.BloodGroup;
            newStudent.BoardMarksObtained = student.BoardMarksObtained;
            newStudent.BoardOrEnrollmentNo = student.BoardOrEnrollmentNo;
            newStudent.BoardOrUniversityName = student.BoardOrUniversityName;
            newStudent.CandidateName = student.CandidateName;
            newStudent.CandidateNo = student.CandidateNo;
            newStudent.Cast = student.Cast;
            newStudent.Category = student.Category;
            newStudent.City = student.City;
            newStudent.ClassId = student.ClassId;
            newStudent.CNIC = student.CNIC;
            newStudent.CountryOfBirth = student.CountryOfBirth;
            newStudent.ElectricityBillNo = student.ElectricityBillNo;
            newStudent.Email = student.Email;
            newStudent.EmergencyContactName = student.EmergencyContactName;
            newStudent.EmergencyContactNumber = student.EmergencyContactNumber;
            newStudent.ExtraCurricularActivities = student.ExtraCurricularActivities;
            newStudent.FName = student.FName;
            newStudent.FromSchool = student.FromSchool;
            newStudent.Gender = student.Gender;
            newStudent.IgnoreFeeDefaulterRestrictLogin = student.IgnoreFeeDefaulterRestrictLogin;
            newStudent.ITSNumber = student.ITSNumber;
            newStudent.LanguageSpken = student.LanguageSpken;
            newStudent.LName = student.LName;
            newStudent.LoginFeeDefualterRestrictLogin = student.LoginFeeDefualterRestrictLogin;
            newStudent.Mobile = student.Mobile;
            newStudent.ModeOfTransport = student.ModeOfTransport;
            newStudent.Nationality = student.Nationality;
            newStudent.NationalityType = student.NationalityType;
            newStudent.OnlyRegisteredNoAdmitted = student.OnlyRegisteredNoAdmitted;
            newStudent.PalceOfBirth = student.PalceOfBirth;
            newStudent.ParentId = student.ParentId == null ? newParent?.ParentId : student.ParentId;
            newStudent.PassportNo = student.PassportNo;
            newStudent.PassportValidity = student.PassportValidity;
            newStudent.Phone = student.Phone;
            newStudent.RegistraionNo = student.RegistraionNo;
            newStudent.Religion = student.Religion;
            newStudent.ResidentCardNo = student.ResidentCardNo;
            newStudent.RestrictLogin = student.RestrictLogin;
            newStudent.RollNo = student.RollNo;
            newStudent.ScholarchipAmount = student.ScholarchipAmount;
            newStudent.SeatNo = student.SeatNo;
            newStudent.SecondAddress = student.SecondAddress;
            newStudent.SecondEmail = student.SecondEmail;
            newStudent.StudentRegistraionNo = student.StudentRegistraionNo;
            newStudent.TaxPercentage = student.TaxPercentage;
            newStudent.ToSchool = student.ToSchool;
            newStudent.VaccinationFirstDose = student.VaccinationFirstDose;
            newStudent.VaccinationFourthDose = student.VaccinationFourthDose;
            newStudent.VaccinationSecondDose = student.VaccinationSecondDose;
            newStudent.VaccinationThirdDose = student.VaccinationThirdDose;
            newStudent.VisaValidity = student.VisaValidity;
            newStudent.WaterBillNo = student.WaterBillNo;
            newStudent.Picture = StudentImage;
            await _repository.AddAsync(newStudent);
            if (await _repository.SaveChanges())
            {
                var RecentStudent = await _db.Students.OrderBy(x => x.StudentId).LastOrDefaultAsync();
                var ifUserExists = await _db.Users.Where(x => x.Email == RecentStudent.Email && x.IsActive == true).FirstOrDefaultAsync();
                if (ifUserExists == null)
                {
                    var user = new Entities.Models.User
                    {
                        FName = RecentStudent.FName,
                        LName = RecentStudent.LName,
                        Email = RecentStudent.Email,
                        Password = RecentStudent.Email,
                        UserName = RecentStudent.Username,
                    };
                    await _repository.AddAsync(user);
                    if (await _repository.SaveChanges())
                    {
                        var recentStudent = await _db.Students.OrderBy(x => x.StudentId).LastOrDefaultAsync();
                        recentStudent.StudentRegistraionNo = "SES-" + Convert.ToString(recentStudent.Gender) + "-" + recentStudent.StudentId + "-" + Convert.ToDateTime(DateTime.Now).ToString("yyyy");
                        var RecentUser = await _db.Users.Where(x => x.Email == recentStudent.Email).FirstOrDefaultAsync();
                        await _repository.UpdateAsync(recentStudent);
                        if (await _repository.SaveChanges())
                        {
                            //var EmailHtmlBody = $"<!DOCTYPE html>\r\n<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" lang=\"en\">\r\n\r\n<head>\r\n\t<title></title>\r\n\t<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n\t<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><!--[if mso]><xml><o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml><![endif]--><!--[if !mso]><!-->\r\n\t<link href=\"https://fonts.googleapis.com/css?family=Abril+Fatface\" rel=\"stylesheet\" type=\"text/css\">\r\n\t<link href=\"https://fonts.googleapis.com/css?family=Cabin\" rel=\"stylesheet\" type=\"text/css\"><!--<![endif]-->\r\n\t<style>\r\n\t\t* {{{{\r\n\t\t\tbox-sizing: border-box;\r\n\t\t}}\r\n\r\n\t\tbody {{{{\r\n\t\t\tmargin: 0;\r\n\t\t\tpadding: 0;\r\n\t\t}}\r\n\r\n\t\ta[x-apple-data-detectors] {{{{\r\n\t\t\tcolor: inherit !important;\r\n\t\t\ttext-decoration: inherit !important;\r\n\t\t}}\r\n\r\n\t\t#MessageViewBody a {{\r\n\t\t\tcolor: inherit;\r\n\t\t\ttext-decoration: none;\r\n\t\t}}\r\n\r\n\t\tp {{\r\n\t\t\tline-height: inherit\r\n\t\t}}\r\n\r\n\t\t.desktop_hide,\r\n\t\t.desktop_hide table {{\r\n\t\t\tmso-hide: all;\r\n\t\t\tdisplay: none;\r\n\t\t\tmax-height: 0px;\r\n\t\t\toverflow: hidden;\r\n\t\t}}\r\n\r\n\t\t.image_block img+div {{\r\n\t\t\tdisplay: none;\r\n\t\t}}\r\n\r\n\t\t@media (max-width:670px) {{\r\n\t\t\t.social_block.desktop_hide .social-table {{\r\n\t\t\t\tdisplay: inline-block !important;\r\n\t\t\t}}\r\n\r\n\t\t\t.row-content {{\r\n\t\t\t\twidth: 100% !important;\r\n\t\t\t}}\r\n\r\n\t\t\t.mobile_hide {{\r\n\t\t\t\tdisplay: none;\r\n\t\t\t}}\r\n\r\n\t\t\t.stack .column {{\r\n\t\t\t\twidth: 100%;\r\n\t\t\t\tdisplay: block;\r\n\t\t\t}}\r\n\r\n\t\t\t.mobile_hide {{\r\n\t\t\t\tmin-height: 0;\r\n\t\t\t\tmax-height: 0;\r\n\t\t\t\tmax-width: 0;\r\n\t\t\t\toverflow: hidden;\r\n\t\t\t\tfont-size: 0px;\r\n\t\t\t}}\r\n\r\n\t\t\t.desktop_hide,\r\n\t\t\t.desktop_hide table {{\r\n\t\t\t\tdisplay: table !important;\r\n\t\t\t\tmax-height: none !important;\r\n\t\t\t}}\r\n\t\t}}\r\n\t</style>\r\n</head>\r\n\r\n<body style=\"background-color: #C1C558; margin: 0; padding: 0; -webkit-text-size-adjust: none; text-size-adjust: none;\">\r\n\t<table class=\"nl-container\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #C1C558;\">\r\n\t\t<tbody>\r\n\t\t\t<tr>\r\n\t\t\t\t<td>\r\n\t\t\t\t\t<table class=\"row row-1\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #FFF6F2;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 20px; padding-top: 25px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\" style=\"padding-left:10px;padding-right:10px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: 'Courier New', Courier, monospace\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class style=\"font-size: 12px; font-family: 'Courier New', Courier, 'Lucida Sans Typewriter', 'Lucida Typewriter', monospace; mso-line-height-alt: 18px; color: #CD705C; line-height: 1.5;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; text-align: center; font-size: 12px; mso-line-height-alt: 63px;\"><span style=\"font-size:42px;\">SEERAT EDUCATION SYSTEM</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-2\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #e8bdac; background-image: url('https://d1oco4z2z1fhwp.cloudfront.net/templates/default/291/easter_ani_2.gif'); background-position: top center; background-repeat: repeat;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-top: 60px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\" style=\"padding-bottom:10px;padding-left:60px;padding-right:60px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: sans-serif\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class style=\"font-size: 12px; font-family: 'Cabin', Arial, 'Helvetica Neue', Helvetica, sans-serif; mso-line-height-alt: 18px; color: #555555; line-height: 1.5;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 22px; text-align: center; mso-line-height-alt: 27px;\"><span style=\"font-size:18px;\">Your Credentials to Access the Online Portal of the School are:</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 22px; text-align: center; mso-line-height-alt: 33px;\"><span style=\"font-size:22px;\">Email: {RecentUser.Email}</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 22px; text-align: center; mso-line-height-alt: 33px;\"><span style=\"font-size:22px;\">Password: {RecentUser.Password}</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-3\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #c1c558;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 10px; padding-top: 20px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: Arial, sans-serif\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class style=\"font-family: 'Abril Fatface', Arial, 'Helvetica Neue', Helvetica, sans-serif; font-size: 12px; mso-line-height-alt: 14.399999999999999px; color: #639175; line-height: 1.2;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; text-align: center; font-size: 12px; mso-line-height-alt: 14.399999999999999px;\"><span style=\"font-size:38px;\">Quality and Excellence in Education</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-4\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #c1c558; background-image: url('https://d1oco4z2z1fhwp.cloudfront.net/templates/default/291/bottom_green.png'); background-repeat: repeat;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 10px; padding-top: 20px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"spacer_block block-1\" style=\"height:20px;line-height:20px;font-size:1px;\">&#8202;</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-5\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #639175;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 25px; padding-top: 20px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"social_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"alignment\" align=\"center\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"social-table\" width=\"111px\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; display: inline-block;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"padding:0 5px 0 0px;\"><a href=\"https://www.facebook.com/\" target=\"_blank\"><img src=\"https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-circle-white/facebook@2x.png\" width=\"32\" height=\"32\" alt=\"Facebook\" title=\"Facebook\" style=\"display: block; height: auto; border: 0;\"></a></td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"padding:0 5px 0 0px;\"><a href=\"https://twitter.com/\" target=\"_blank\"><img src=\"https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-circle-white/twitter@2x.png\" width=\"32\" height=\"32\" alt=\"Twitter\" title=\"Twitter\" style=\"display: block; height: auto; border: 0;\"></a></td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"padding:0 5px 0 0px;\"><a href=\"https://instagram.com/\" target=\"_blank\"><img src=\"https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-circle-white/instagram@2x.png\" width=\"32\" height=\"32\" alt=\"Instagram\" title=\"Instagram\" style=\"display: block; height: auto; border: 0;\"></a></td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"text_block block-2\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: sans-serif\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class style=\"font-size: 12px; font-family: 'Cabin', Arial, 'Helvetica Neue', Helvetica, sans-serif; mso-line-height-alt: 14.399999999999999px; color: #FFFFFF; line-height: 1.2;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 14px; text-align: center; mso-line-height-alt: 16.8px;\">Sattalite Town Quetta.</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t</tbody>\r\n\t</table><!-- End -->\r\n</body>\r\n\r\n</html>";
                            //await _emailSender.SendEmailAsync(RecentUser.Email, "Your Update Email Address", EmailHtmlBody);
                            return RedirectToAction("Student");
                        }
                        ModelState.AddModelError("", "Error While Saving to Database");
                    }
                    ModelState.AddModelError("", "Error While Saving to Database");
                }
                else
                {
                    return RedirectToAction("Student");
                }
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            //}
            //else
            //{
            //    return View(student);
            //}
            return View(student);
        }
        [Authorize(Policy = "Student.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateStudent(int StudentId)
        {
            ViewBag.Parents = from a in _db.Parents
                              select new
                              {
                                  ParentId = a.ParentId,
                                  FName = a.FName,
                                  LName = a.LName,
                                  CNIC = a.CNIC
                              };
            ViewBag.Classes = from a in _db.Grades
                              from b in _db.Sections
                              where b.GradeId == a.GradeId
                              select new
                              {
                                  SectionId = b.SectionId,
                                  ClassName = a.GradeName + b.SectionName
                              };
            var student = await _db.Students.Where(x => x.StudentId == StudentId).FirstOrDefaultAsync();
            var credentials = await _db.Users.Where(x => x.Email == student.Email).FirstOrDefaultAsync();
            var parentCredentials = await (from a in _db.Parents
                                           from b in _db.Users
                                           where a.ParentId == student.ParentId && b.Email == a.Email && b.IsActive == true
                                           select b).FirstOrDefaultAsync();
            StudentVM newStudent = new StudentVM();
            newStudent.DOB = student?.DOB;
            newStudent.Address = student?.Address;
            newStudent.AdmissionEmail = student?.AdmissionEmail;
            newStudent.AdmissionTestResult = student.AdmissionTestResult;
            newStudent.AdmittedClassorSection = student.AdmittedClassOrSection;
            newStudent.AdmittedSession = student.AdmittedSession;
            newStudent.Allergies = student.Allergies;
            newStudent.BloodGroup = student.BloodGroup;
            newStudent.BoardMarksObtained = student.BoardMarksObtained;
            newStudent.BoardOrEnrollmentNo = student.BoardOrEnrollmentNo;
            newStudent.BoardOrUniversityName = student.BoardOrUniversityName;
            newStudent.CandidateName = student.CandidateName;
            newStudent.CandidateNo = student.CandidateNo;
            newStudent.Cast = student.Cast;
            newStudent.Category = student.Category;
            newStudent.City = student.City;
            newStudent.ClassId = (int)student?.ClassId;
            newStudent.UpdateCNIC = student?.CNIC;
            newStudent.CountryOfBirth = student.CountryOfBirth;
            newStudent.ElectricityBillNo = student.ElectricityBillNo;
            newStudent.UpdateEmail = student?.Email;
            newStudent.EmergencyContactName = student.EmergencyContactName;
            newStudent.EmergencyContactNumber = student.EmergencyContactNumber;
            newStudent.ExtraCurricularActivities = student.ExtraCurricularActivities;
            newStudent.FName = student?.FName;
            newStudent.FromSchool = student.FromSchool;
            newStudent.Gender = student?.Gender;
            newStudent.IgnoreFeeDefaulterRestrictLogin = (bool)student?.IgnoreFeeDefaulterRestrictLogin;
            newStudent.ITSNumber = student.ITSNumber;
            newStudent.LanguageSpken = student.LanguageSpken;
            newStudent.LName = student?.LName;
            newStudent.LoginFeeDefualterRestrictLogin = (bool)student?.LoginFeeDefualterRestrictLogin;
            newStudent.Mobile = student.Mobile;
            newStudent.ModeOfTransport = student.ModeOfTransport;
            newStudent.Nationality = student.Nationality;
            newStudent.NationalityType = student.NationalityType;
            newStudent.OnlyRegisteredNoAdmitted = (bool)student?.OnlyRegisteredNoAdmitted;
            newStudent.PalceOfBirth = student.PalceOfBirth;
            newStudent.ParentId = (int)student?.ParentId;
            newStudent.PassportNo = student.PassportNo;
            newStudent.PassportValidity = student.PassportValidity;
            newStudent.Phone = student.Phone;
            newStudent.WhatsAppNo = student.WhastAppNo;
            newStudent.PictureURL = student.Picture;
            newStudent.RegistraionNo = student.RegistraionNo;
            newStudent.Religion = student.Religion;
            newStudent.ResidentCardNo = student.ResidentCardNo;
            newStudent.RestrictLogin = (bool)student?.RestrictLogin;
            newStudent.RollNo = student.RollNo;
            newStudent.ScholarchipAmount = student.ScholarchipAmount;
            newStudent.SeatNo = student.SeatNo;
            newStudent.SecondAddress = student.SecondAddress;
            newStudent.SecondEmail = student.SecondEmail;
            newStudent.Status = (bool)student?.Status;
            newStudent.TaxPercentage = student.TaxPercentage;
            newStudent.ToSchool = student.ToSchool;
            newStudent.VaccinationFirstDose = student.VaccinationFirstDose;
            newStudent.VaccinationFourthDose = student.VaccinationFourthDose;
            newStudent.VaccinationSecondDose = student.VaccinationSecondDose;
            newStudent.VaccinationThirdDose = student.VaccinationThirdDose;
            newStudent.VisaValidity = student.VisaValidity;
            newStudent.WaterBillNo = student.WaterBillNo;
            var parent = await _db.Parents.Where(x => x.ParentId == student.ParentId).FirstOrDefaultAsync();
            newStudent.ParentFName = parent?.FName;
            newStudent.ParentLName = parent?.LName;
            newStudent.ParentDesignation = parent?.Designation;
            newStudent.ParentType = parent?.ParentType;
            newStudent.ParentCNIC = parent?.CNIC;
            newStudent.ParentMobile = parent?.Mobile;
            newStudent.ParentOccupation = parent?.Occupation;
            newStudent.ParentOfficeAddress = parent?.OfficeAddress;
            newStudent.ParentRegistraionDate = parent?.CreatedDate;
            newStudent.ParentUpdateEmail = parent?.Email;
            newStudent.ParentEmployer = parent?.Employer;
            var oldImage = new FileInfo(@"C:\\Users\\SmoOkie\\source\\repos\\SESLocal\\myWebApp\\wwwroot\\StudentsImages\\" + newStudent.PictureURL);
            return View(newStudent);
        }
        [Authorize(Policy = "Student.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateStudent(StudentVM student)
        {
            //if (ModelState.IsValid)
            //{
            var user = await _db.Users.Where(x => x.Email == student.UpdateEmail).FirstOrDefaultAsync();
            var newStudent = await _db.Students.Where(x => x.StudentId == student.StudentId).FirstOrDefaultAsync();
            var doParentExist = await _db.Parents.Where(x => x.ParentId == student.ParentId).FirstOrDefaultAsync();
            var parentUser = await _db.Users.Where(x => x.Email == student.ParentUpdateEmail).FirstOrDefaultAsync();
            if (doParentExist == null)
            {
                var parent = new Parent
                {
                    FName = student.ParentFName,
                    LName = student.ParentLName,
                    Designation = student.ParentDesignation,
                    Employer = student.ParentEmployer,
                    ParentType = student.ParentType,
                    CNIC = student.ParentCNIC,
                    Mobile = student.ParentMobile,
                    Occupation = student.ParentOccupation,
                    OfficeAddress = student.ParentOfficeAddress,
                    Email = student.ParentUpdateEmail
                };
                var NewParentUser = new User
                {
                    FName = student.ParentFName,
                    LName = student.ParentLName,
                    Email = student.ParentUpdateEmail,
                    Password = student.ParentUpdateEmail,
                    UserName = student.ParentFName + student.ParentLName
                };
                await _repository.AddAsync(NewParentUser);
                await _repository.AddAsync(parent);
                if (await _repository.SaveChanges())
                {
                    //var EmailHtmlBody = $"<!DOCTYPE html>\r\n<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" lang=\"en\">\r\n\r\n<head>\r\n\t<title></title>\r\n\t<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n\t<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><!--[if mso]><xml><o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml><![endif]--><!--[if !mso]><!-->\r\n\t<link href=\"https://fonts.googleapis.com/css?family=Abril+Fatface\" rel=\"stylesheet\" type=\"text/css\">\r\n\t<link href=\"https://fonts.googleapis.com/css?family=Cabin\" rel=\"stylesheet\" type=\"text/css\"><!--<![endif]-->\r\n\t<style>\r\n\t\t* {{{{\r\n\t\t\tbox-sizing: border-box;\r\n\t\t}}\r\n\r\n\t\tbody {{{{\r\n\t\t\tmargin: 0;\r\n\t\t\tpadding: 0;\r\n\t\t}}\r\n\r\n\t\ta[x-apple-data-detectors] {{{{\r\n\t\t\tcolor: inherit !important;\r\n\t\t\ttext-decoration: inherit !important;\r\n\t\t}}\r\n\r\n\t\t#MessageViewBody a {{\r\n\t\t\tcolor: inherit;\r\n\t\t\ttext-decoration: none;\r\n\t\t}}\r\n\r\n\t\tp {{\r\n\t\t\tline-height: inherit\r\n\t\t}}\r\n\r\n\t\t.desktop_hide,\r\n\t\t.desktop_hide table {{\r\n\t\t\tmso-hide: all;\r\n\t\t\tdisplay: none;\r\n\t\t\tmax-height: 0px;\r\n\t\t\toverflow: hidden;\r\n\t\t}}\r\n\r\n\t\t.image_block img+div {{\r\n\t\t\tdisplay: none;\r\n\t\t}}\r\n\r\n\t\t@media (max-width:670px) {{\r\n\t\t\t.social_block.desktop_hide .social-table {{\r\n\t\t\t\tdisplay: inline-block !important;\r\n\t\t\t}}\r\n\r\n\t\t\t.row-content {{\r\n\t\t\t\twidth: 100% !important;\r\n\t\t\t}}\r\n\r\n\t\t\t.mobile_hide {{\r\n\t\t\t\tdisplay: none;\r\n\t\t\t}}\r\n\r\n\t\t\t.stack .column {{\r\n\t\t\t\twidth: 100%;\r\n\t\t\t\tdisplay: block;\r\n\t\t\t}}\r\n\r\n\t\t\t.mobile_hide {{\r\n\t\t\t\tmin-height: 0;\r\n\t\t\t\tmax-height: 0;\r\n\t\t\t\tmax-width: 0;\r\n\t\t\t\toverflow: hidden;\r\n\t\t\t\tfont-size: 0px;\r\n\t\t\t}}\r\n\r\n\t\t\t.desktop_hide,\r\n\t\t\t.desktop_hide table {{\r\n\t\t\t\tdisplay: table !important;\r\n\t\t\t\tmax-height: none !important;\r\n\t\t\t}}\r\n\t\t}}\r\n\t</style>\r\n</head>\r\n\r\n<body style=\"background-color: #C1C558; margin: 0; padding: 0; -webkit-text-size-adjust: none; text-size-adjust: none;\">\r\n\t<table class=\"nl-container\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #C1C558;\">\r\n\t\t<tbody>\r\n\t\t\t<tr>\r\n\t\t\t\t<td>\r\n\t\t\t\t\t<table class=\"row row-1\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #FFF6F2;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 20px; padding-top: 25px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\" style=\"padding-left:10px;padding-right:10px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: 'Courier New', Courier, monospace\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class style=\"font-size: 12px; font-family: 'Courier New', Courier, 'Lucida Sans Typewriter', 'Lucida Typewriter', monospace; mso-line-height-alt: 18px; color: #CD705C; line-height: 1.5;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; text-align: center; font-size: 12px; mso-line-height-alt: 63px;\"><span style=\"font-size:42px;\">SEERAT EDUCATION SYSTEM</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-2\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #e8bdac; background-image: url('https://d1oco4z2z1fhwp.cloudfront.net/templates/default/291/easter_ani_2.gif'); background-position: top center; background-repeat: repeat;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-top: 60px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\" style=\"padding-bottom:10px;padding-left:60px;padding-right:60px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: sans-serif\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class style=\"font-size: 12px; font-family: 'Cabin', Arial, 'Helvetica Neue', Helvetica, sans-serif; mso-line-height-alt: 18px; color: #555555; line-height: 1.5;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 22px; text-align: center; mso-line-height-alt: 27px;\"><span style=\"font-size:18px;\">Your Credentials to Access the Online Portal of the School are:</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 22px; text-align: center; mso-line-height-alt: 33px;\"><span style=\"font-size:22px;\">Email: {NewParentUser.Email}</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 22px; text-align: center; mso-line-height-alt: 33px;\"><span style=\"font-size:22px;\">Password: {NewParentUser.Password}</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-3\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #c1c558;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 10px; padding-top: 20px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: Arial, sans-serif\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class style=\"font-family: 'Abril Fatface', Arial, 'Helvetica Neue', Helvetica, sans-serif; font-size: 12px; mso-line-height-alt: 14.399999999999999px; color: #639175; line-height: 1.2;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; text-align: center; font-size: 12px; mso-line-height-alt: 14.399999999999999px;\"><span style=\"font-size:38px;\">Quality and Excellence in Education</span></p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-4\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #c1c558; background-image: url('https://d1oco4z2z1fhwp.cloudfront.net/templates/default/291/bottom_green.png'); background-repeat: repeat;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 10px; padding-top: 20px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"spacer_block block-1\" style=\"height:20px;line-height:20px;font-size:1px;\">&#8202;</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-5\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #639175;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 650px;\" width=\"650\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 25px; padding-top: 20px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"social_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"alignment\" align=\"center\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"social-table\" width=\"111px\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; display: inline-block;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"padding:0 5px 0 0px;\"><a href=\"https://www.facebook.com/\" target=\"_blank\"><img src=\"https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-circle-white/facebook@2x.png\" width=\"32\" height=\"32\" alt=\"Facebook\" title=\"Facebook\" style=\"display: block; height: auto; border: 0;\"></a></td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"padding:0 5px 0 0px;\"><a href=\"https://twitter.com/\" target=\"_blank\"><img src=\"https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-circle-white/twitter@2x.png\" width=\"32\" height=\"32\" alt=\"Twitter\" title=\"Twitter\" style=\"display: block; height: auto; border: 0;\"></a></td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"padding:0 5px 0 0px;\"><a href=\"https://instagram.com/\" target=\"_blank\"><img src=\"https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-circle-white/instagram@2x.png\" width=\"32\" height=\"32\" alt=\"Instagram\" title=\"Instagram\" style=\"display: block; height: auto; border: 0;\"></a></td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"text_block block-2\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: sans-serif\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class style=\"font-size: 12px; font-family: 'Cabin', Arial, 'Helvetica Neue', Helvetica, sans-serif; mso-line-height-alt: 14.399999999999999px; color: #FFFFFF; line-height: 1.2;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0; font-size: 14px; text-align: center; mso-line-height-alt: 16.8px;\">Sattalite Town Quetta.</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t</tbody>\r\n\t</table><!-- End -->\r\n</body>\r\n\r\n</html>";
                    //await _emailSender.SendEmailAsync(NewParentUser.Email, "Your Update Email Address", EmailHtmlBody);
                }
            }
            var newParent = await _db.Parents.OrderBy(x => x.ParentId).LastOrDefaultAsync();
            if (newStudent.Status == false && student.Status == true)
            {
                newStudent.Status = true;
                user.IsActive = true;
                var InActiveParentUser = await _db.Users.Where(x => x.Email == student.ParentUpdateEmail).FirstOrDefaultAsync();
                InActiveParentUser.IsActive = true;
                await _repository.UpdateAsync(InActiveParentUser);
            }
            newStudent.DOB = student.DOB;
            newStudent.WhastAppNo = student.WhatsAppNo;
            newStudent.Address = student.Address;
            newStudent.AdmissionEmail = student.AdmissionEmail;
            newStudent.AdmissionTestResult = student.AdmissionTestResult;
            newStudent.AdmittedClassOrSection = student.AdmittedClassorSection;
            newStudent.AdmittedSession = student.AdmittedSession;
            newStudent.Allergies = student.Allergies;
            newStudent.BloodGroup = student.BloodGroup;
            newStudent.BoardMarksObtained = student.BoardMarksObtained;
            newStudent.BoardOrEnrollmentNo = student.BoardOrEnrollmentNo;
            newStudent.BoardOrUniversityName = student.BoardOrUniversityName;
            newStudent.CandidateName = student.CandidateName;
            newStudent.CandidateNo = student.CandidateNo;
            newStudent.Cast = student.Cast;
            newStudent.Category = student.Category;
            newStudent.City = student.City;
            newStudent.ClassId = student.ClassId;
            newStudent.CNIC = student.UpdateCNIC;
            newStudent.CountryOfBirth = student.CountryOfBirth;
            newStudent.ElectricityBillNo = student.ElectricityBillNo;
            newStudent.Email = student.UpdateEmail;
            newStudent.EmergencyContactName = student.EmergencyContactName;
            newStudent.EmergencyContactNumber = student.EmergencyContactNumber;
            newStudent.ExtraCurricularActivities = student.ExtraCurricularActivities;
            newStudent.FName = student.FName;
            newStudent.FromSchool = student.FromSchool;
            newStudent.Gender = student.Gender;
            newStudent.IgnoreFeeDefaulterRestrictLogin = student.IgnoreFeeDefaulterRestrictLogin;
            newStudent.ITSNumber = student.ITSNumber;
            newStudent.LanguageSpken = student.LanguageSpken;
            newStudent.LName = student.LName;
            newStudent.LoginFeeDefualterRestrictLogin = student.LoginFeeDefualterRestrictLogin;
            newStudent.Mobile = student.Mobile;
            newStudent.ModeOfTransport = student.ModeOfTransport;
            newStudent.Nationality = student.Nationality;
            newStudent.NationalityType = student.NationalityType;
            newStudent.OnlyRegisteredNoAdmitted = student.OnlyRegisteredNoAdmitted;
            newStudent.PalceOfBirth = student.PalceOfBirth;
            newStudent.ParentId = doParentExist == null ? newParent.ParentId : student.ParentId;
            newStudent.PassportNo = student.PassportNo;
            newStudent.PassportValidity = student.PassportValidity;
            newStudent.Phone = student.Phone;
            newStudent.RegistraionNo = student.RegistraionNo;
            newStudent.Religion = student.Religion;
            newStudent.ResidentCardNo = student.ResidentCardNo;
            newStudent.RestrictLogin = student.RestrictLogin;
            newStudent.RollNo = student.RollNo;
            newStudent.ScholarchipAmount = student.ScholarchipAmount;
            newStudent.SeatNo = student.SeatNo;
            newStudent.SecondAddress = student.SecondAddress;
            newStudent.SecondEmail = student.SecondEmail;
            newStudent.Status = student.Status;
            newStudent.TaxPercentage = student.TaxPercentage;
            newStudent.ToSchool = student.ToSchool;
            newStudent.VaccinationFirstDose = student.VaccinationFirstDose;
            newStudent.VaccinationFourthDose = student.VaccinationFourthDose;
            newStudent.VaccinationSecondDose = student.VaccinationSecondDose;
            newStudent.VaccinationThirdDose = student.VaccinationThirdDose;
            newStudent.VisaValidity = student.VisaValidity;
            newStudent.WaterBillNo = student.WaterBillNo;
            if (student.Picture != null)
            {
                if (student.PictureURL != null)
                {
                    var oldImage = new FileInfo(@"C:\\Users\\SmoOkie\\source\\repos\\SESLocal\\myWebApp\\wwwroot\\StudentsImages\\" + student.PictureURL);
                    oldImage.Delete();
                }
                newStudent.Picture = FileSaver(student.StudentId, student.FName, student.LName, student.Picture);
            }
            await _repository.UpdateAsync(newStudent);
            user.FName = student.FName;
            user.LName = student.LName;
            user.Email = student.UpdateEmail;
            user.UserName = student.FName + student.LName;
            user.Password = student.UpdateEmail;
            await _repository.UpdateAsync(user);
            if (parentUser != null)
            {
                parentUser.FName = student.ParentFName;
                parentUser.LName = student.ParentLName;
                parentUser.Email = student.ParentUpdateEmail;
                await _repository.UpdateAsync(parentUser);
            }
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Student");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            //}
            return View(student);
        }
        [Authorize(Policy = "Student.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteStudent(int StudentId)
        {
            var student = await _db.Students.Where(x => x.StudentId == StudentId).FirstOrDefaultAsync();
            var user = await _db.Users.Where(x => x.Email == student.Email).FirstOrDefaultAsync();
            student.Status = false;
            user.IsActive = false;
            var parent = await _db.Parents.Where(x => x.ParentId == student.ParentId).ToListAsync();
            if (parent.Count < 2)
            {
                parent.FirstOrDefault().Status = false;
                _db.Update(parent.FirstOrDefault());
                var pUser = await _db.Users.Where(x => x.Email == parent.FirstOrDefault().Email).FirstOrDefaultAsync();
                pUser.IsActive = false;
                await _repository.UpdateAsync(pUser);
            }
            await _repository.UpdateAsync(user);
            await _repository.UpdateAsync(student);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Student");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving in Database!");
            }
            return RedirectToAction("Student");
        }
        [Authorize(Policy = "Student.Details")]
        [HttpGet]
        public async Task<IActionResult> StudentDetails(int StudentId)
        {
            ViewBag.Student = await (from a in _db.Students
                                     from b in _db.Parents
                                     from c in _db.Grades
                                     from d in _db.Sections
                                     where a.StudentId == StudentId && b.ParentId == a.ParentId && d.SectionId == a.ClassId && c.GradeId == d.GradeId && a.Status == true
                                     select new
                                     {
                                         FName = a.FName,
                                         LName = a.LName,
                                         StudentId = a.StudentId,
                                         Picture = a.Picture,
                                         Status = a.Status,
                                         ClassName = c.GradeName + d.SectionName,
                                         ParentName = b.FName + " " + b.LName,
                                         Address = a.Address,
                                         AdmissionEmail = a.AdmissionEmail,
                                         AdmissionTestResult = a.AdmissionTestResult,
                                         AdmittedSession = a.AdmittedSession,
                                         Allergies = a.Allergies,
                                         BloodGroup = a.BloodGroup,
                                         BoardMarksObtained = a.BoardMarksObtained,
                                         BoardOrEnrollmentNo = a.BoardOrEnrollmentNo,
                                         BoardOrUniversityName = a.BoardOrUniversityName,
                                         Cast = a.Cast,
                                         Category = a.Category,
                                         City = a.City,
                                         CNIC = a.CNIC,
                                         CountryOfBirth = a.CountryOfBirth,
                                         Email = a.Email,
                                         ExtraCurricularActivities = a.ExtraCurricularActivities,
                                         FromSchool = a.FromSchool,
                                         Gender = a.Gender
                                     }).FirstOrDefaultAsync();
            List<StudentAttendance> attendance = new List<StudentAttendance>();
            attendance = await _db.StudentAttendance.Where(x => x.StudentId == StudentId).Select(x => new StudentAttendance { Date = x.Date, LateOrOnTime = x.LateOrOnTime, AttendanceStatus = x.AttendanceStatus }).ToListAsync();
            return View(attendance);
        }
        [Authorize(Policy = "Student.Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        #region Functions

        public string FileSaver(int id, string FName, string LName, IFormFile UserImage)
        {
            var fileInfo = new FileInfo(UserImage.FileName);
            string ImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "StudentsImages/");
            if (!Directory.Exists(ImagePath)) Directory.CreateDirectory(ImagePath);
            string ImageName = FName + LName + id + fileInfo.Extension;
            string imageNameWithPath = Path.Combine(ImagePath, ImageName);
            using (var stream = new FileStream(imageNameWithPath, FileMode.Create))
            {
                UserImage.CopyTo(stream);
            }
            return ImageName;
        }


        #endregion

        #region Dynamic-Data

        public async Task<JsonResult> GetParentInfo(string CNICNo)
        {
            var parentInfo = await _db.Parents.Where(x => x.CNIC == CNICNo).FirstOrDefaultAsync();
            return Json(parentInfo);
        }

        #endregion
    }
}
