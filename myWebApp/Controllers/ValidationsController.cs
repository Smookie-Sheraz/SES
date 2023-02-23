using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myWebApp.ViewModels.Auth;

namespace myWebApp.Controllers
{
    //[AllowAnonymous]
    public class ValidationsController : Controller
    {
        private readonly IEFRepository _repository;
        private readonly SchoolDbContext _db;

        public ValidationsController(IEFRepository repository, SchoolDbContext db)
        {
            _repository = repository;
            _db = db;
        }
        //public JsonResult EmailExist(UserVM login)
        //{
        //    var user = _repository.GetUserByEMail(login.Email);
        //    if (user.Result == null)
        //    {
        //        return Json(false);
        //    }
        //    return Json(true);
        //}
        public async Task<JsonResult> UserNameExist(string Email)
        {
            var user = await _db.Users.Where(x => x.Email == Email && x.IsActive == true).FirstOrDefaultAsync();
            if (user == null)
            {
                return Json(false);
            }
            return Json(true);
        }
        public async Task<JsonResult> UserEmailExist(string Email)
        {
            var user = await _db.Users.Where(x => x.Email == Email).FirstOrDefaultAsync();
            if (user != null)
            {
                if (user.Email == Email)
                {
                    return Json(true);
                }
                return Json(false);
            }
            return Json(true);
        }
        public async Task<JsonResult> EmailAlreadyExist(string Email)
        {
            var user = await _db.Employees.Where(x => x.Email == Email).FirstOrDefaultAsync();
            if (user != null)
            {
                if (user.Email == Email)
                {
                    return Json(true);
                }
                return Json(false);
            }
            return Json(true);
        }
        public async Task<JsonResult> CNICExist(string CNICNo)
        {
            var user = await _db.Employees.Where(x => x.CNICNo == CNICNo).FirstOrDefaultAsync();
            
            if(user != null)
            {
                if (user.CNICNo == CNICNo)
                {
                    return Json(true);
                }
                return Json(false);
            }
            return Json(true);
        }
        public async Task<JsonResult> ShooraEmailAlreadyExist(string Email)
        {
            var user = await _db.Shoora.Where(x => x.Email == Email).FirstOrDefaultAsync();
            
            if (user != null)
            {
                if (user.Email == Email)
                {
                    return Json(true);
                }
                return Json(false);
            }
            return Json(true);
        }
        public async Task<JsonResult> ShooraCNICAlreadyExist(string CNICNo)
        {
            var user = await _db.Shoora.Where(x => x.CNICNo == CNICNo).FirstOrDefaultAsync();
            
            if (user != null)
            {
                if (user.CNICNo == CNICNo)
                {
                    return Json(true);
                }
                return Json(false);
            }
            return Json(true);
        }
        //public JsonResult MonthsLimit(int TermId)
        //{
        //    var months = _db.months.Where(x => x.TermId == TermId).ToList();
        //    if (months.Count() == 3)
        //    {
        //        return Json(false);
        //    }
        //    return Json(true);
        //}

        //public JsonResult MonthsLimit(DateTime ChapterStartDate)
        //{
        //var months = _db.months.Where(x => x.TermId == TermId).ToList();
        //if (months.Count() == 3)
        //{
        //    return Json(false);
        //}
        //return Json(true);
    }
        //        //public JsonResult SchoolIdExist(Setup setup)
        //        //{
        //        //    var location = _repository.GetSchoolById(setup.SchoolId);
        //        //    if (location.Result == null)
        //        //    {
        //        //        return Json(false);
        //        //    }
        //        //    return Json(true);
        //        //}
        //        public JsonResult CampusIdExist(Setup setup)
        //        {
        //            var location = _repository.GetCampusById(setup.CampusId);
        //            if (location.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult GradeGroupIdExist(AddGrades grade)
        //        {
        //            var gradeGroup = _repository.GetGradeGroupById(grade.GradeGroupId);
        //            if (gradeGroup.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult GradesIdExist(GradeSection section)
        //        {
        //            var grade = _repository.GetClassGradeById(section.GradeId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }

        //        #region Placement

        //        #region Add
        //        public JsonResult DepartmentIdExist(AddPlacement placement)
        //        {
        //            var grade = _repository.GetDepartmentById(placement.DepartmentId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult DesignationIdExist(AddPlacement placement)
        //        {
        //            var grade = _repository.GetDesignationById(placement.DesignationId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult GradeIdExist(AddPlacement placement)
        //        {
        //            var grade = _repository.GetGradeById(placement.GradeId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult EmployeeTypeIdExist(AddPlacement placement)
        //        {
        //            var grade = _repository.GetEmployeeTypeById(placement.EmployeeTypeId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult ShiftIdExist(AddPlacement placement)
        //        {
        //            var grade = _repository.GetShiftById(placement.ShiftId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult SectionIdExist(AddPlacement placement)
        //        {
        //            var grade = _repository.GetSectionById(placement.SectionId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult ProjectIdExist(AddPlacement placement)
        //        {
        //            var grade = _repository.GetProjectById(placement.ProjectId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult SchoolIdExist(Placement placement)
        //        {
        //            var school = _repository.GetSchoolById(placement.SchoolId);
        //            if (school.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }

        //        #endregion

        //        #region Update
        //        public JsonResult UPlacementLocationIdExist(UpdatePlacement placement)
        //        {
        //            var location = _repository.GetLocationById(placement.LocationId);
        //            if (location.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult UPlacementDepartmentIdExist(UpdatePlacement placement)
        //        {
        //            var grade = _repository.GetDepartmentById(placement.DepartmentId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult UPlacementDesignationIdExist(UpdatePlacement placement)
        //        {
        //            var grade = _repository.GetDesignationById(placement.DesignationId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult UPlacementGradeIdExist(UpdatePlacement placement)
        //        {
        //            var grade = _repository.GetGradeById(placement.GradeId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult UPlacementEmployeeTypeIdExist(UpdatePlacement placement)
        //        {
        //            var grade = _repository.GetEmployeeTypeById(placement.EmployeeTypeId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult UPlacementShiftIdExist(UpdatePlacement placement)
        //        {
        //            var grade = _repository.GetShiftById(placement.ShiftId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult UPlacementSectionIdExist(UpdatePlacement placement)
        //        {
        //            var grade = _repository.GetSectionById(placement.SectionId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult UPlacementProjectIdExist(UpdatePlacement placement)
        //        {
        //            var grade = _repository.GetProjectById(placement.ProjectId);
        //            if (grade.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult UPlacementSchoolIdExist(UpdatePlacement placement)
        //        {
        //            var school = _repository.GetSchoolById(placement.SchoolId);
        //            if (school.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        public JsonResult UPlacementCampusIdExist(Setup setup)
        //        {
        //            var location = _repository.GetCampusById(setup.CampusId);
        //            if (location.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        #endregion

        //        #endregion

        //        #region Employee

        //        #region Add
        //        public JsonResult PlacementIdExist(AddEmployee employee)
        //        {
        //            var placement = _repository.GetPlacementById(employee.PlacementId);
        //            if (placement.Result == null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //        #endregion

        //        #region Update

        //        #endregion


        //        #endregion
    
}
