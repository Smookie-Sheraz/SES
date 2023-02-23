//using Microsoft.AspNetCore.Mvc;
//using Infrastructure.Data;
//using myWebApp.ViewModels.School;
//using Infrastructure.Repositories;
//using Entities.Models;
//using Microsoft.EntityFrameworkCore;

//namespace myWebApp.Controllers
//{
//    public class SchoolController : Controller
//    {

//        private readonly IEFRepository _repository;

//        public SchoolController(IEFRepository repository)
//        {
//            _repository = repository;
//        }

//        #region School
//        public async Task<IActionResult> School()
//        {
//            var schools = await _repository.GetSchools();
//            List<Location> locations = new List<Location>(schools.Count());
//            foreach (var school in schools)
//            {
//                var location = _repository.GetLocationById(school.LocationId);
//                locations.Add(await location);
//            }
//            ViewBag.Locations = locations;
//            var result = await _repository.GetSchools();
//            return View(result);
//        }
//        [HttpGet]
//        public async Task<IActionResult> AddSchool()
//        {
//            ViewBag.Locations = await _repository.GetLocations();
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> AddSchool(AddSchool school)
//        {
//            if (ModelState.IsValid)
//            {
//                var newSchool = new School
//                {
//                    SchoolCode = school.Code,
//                    Description = school.Description,
//                    IsActive = school.IsActive,
//                    LocationId = school.LocationId
//                };
//                await _repository.AddAsync(newSchool);
//                if (await _repository.SaveChanges())
//                {
//                    return RedirectToAction("School");
//                }
//                ModelState.AddModelError("", "Error While Saving to Database");
//            }
//            return View(school);
//        }
//        [HttpGet]
//        public async Task<IActionResult> UpdateSchool(int id)
//        {
//            var temp = await _repository.GetSchoolById(id);
//            if (temp == null)
//            {
//                return NotFound();
//            }
//            var school = new UpdateSchool
//            {
//                Id = temp.SchoolId,
//                Code = temp.SchoolCode,
//                Description = temp.Description,
//                IsActive = temp.IsActive,
//                LocationId = temp.LocationId
//            };
//            return View(school);
//        }
//        [HttpPost]
//        public async Task<IActionResult> UpdateSchool(UpdateSchool school)
//        {
//            var temp = await _repository.GetSchoolById(school.Id);
//            if (temp == null)
//            {
//                return NotFound();
//            }
//            temp.SchoolCode = school.Code;
//            temp.Description = school.Description;
//            temp.IsActive = school.IsActive;
//            temp.LocationId = school.LocationId;
//            await _repository.UpdateAsync(temp);
//            if (await _repository.SaveChanges())
//            {
//                return RedirectToAction("School");
//            }
//            ModelState.AddModelError("", "Error While Saving to Database");
//            return View(school);
//        }
//        [HttpGet]
//        public async Task<IActionResult> DeleteSchool(int id)
//        {
//            var temp = await _repository.GetSchoolById(id);
//            if (temp == null)
//            {
//                return NotFound();
//            }
//            await _repository.Delete(temp);
//            if (await _repository.SaveChanges())
//            {
//                return RedirectToAction("School");
//            }
//            return RedirectToAction("School");
//        }
//        #endregion

//        #region Campus-Setup
//        public async Task<IActionResult> Campus()
//        {
//            var result = await _repository.GetCampuses();
//            return View(result);
//        }
//        [HttpGet]
//        public async Task<IActionResult> AddCampus()
//        {
//            ViewBag.Locations = await _repository.GetLocations();
//            ViewBag.Schools = await _repository.GetSchools();
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> AddCampus(AddCampus campus)
//        {
//            if (ModelState.IsValid)
//            {
//                var newCampus = new Campus
//                {
//                    CampusCode = campus.Code,
//                    SchoolId = campus.SchoolId,
//                    Description = campus.Description,
//                    IsActive = campus.IsActive,
//                    LocationId = campus.LocationId

//                };
//                await _repository.AddAsync(newCampus);
//                if (await _repository.SaveChanges())
//                {
//                    return RedirectToAction("Campus");
//                }
//                ModelState.AddModelError("", "Error While Saving to Database");
//            }
//            return View(campus);
//        }
//        [HttpGet]
//        public async Task<IActionResult> UpdateCampus(int id)
//        {
//            var temp = await _repository.GetCampusById(id);
//            if (temp == null)
//            {
//                return NotFound();
//            }
//            var Location = new UpdateCampus
//            {
//                Id = temp.CampusId,
//                Code = temp.CampusCode,
//                Description = temp.Description,
//                SchoolId = temp.LocationId,
//                IsActive = temp.IsActive
//            };
//            return View(Location);
//        }
//        [HttpPost]
//        public async Task<IActionResult> UpdateCampus(UpdateCampus campus)
//        {
//            var temp = await _repository.GetCampusById(campus.Id);
//            if (temp == null)
//            {
//                return NotFound();
//            }
//            temp.CampusCode = campus.Code;
//            temp.Description = campus.Description;
//            temp.IsActive = campus.IsActive;
//            temp.SchoolId = campus.SchoolId;
//            await _repository.UpdateAsync(temp);
//            if (await _repository.SaveChanges())
//            {
//                return RedirectToAction("Campus");
//            }
//            ModelState.AddModelError("", "Error While Saving to Database");
//            return View(campus);
//        }
//        [HttpGet]
//        public async Task<IActionResult> DeleteCampus(int id)
//        {
//            var temp = await _repository.GetCampusById(id);
//            if (temp == null)
//            {
//                return NotFound();
//            }
//            await _repository.Delete(temp);
//            if (await _repository.SaveChanges())
//            {
//                return RedirectToAction("Campus");
//            }
//            return RedirectToAction("Campus");
//        }
//        #endregion

//        #region GradeGroup
//        public async Task<IActionResult> GradeGroup()
//        {
//            var result = await _repository.GetGradeGroups();
//            return View(result);
//        }
//        [HttpGet]
//        public async Task<IActionResult> AddGradeGroup()
//        {
//            ViewBag.Campuses = await _repository.GetCampuses();
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> AddGradeGroup(AddGradeGroup gradeGroup)
//        {
//            if (ModelState.IsValid)
//            {
//                var newGradeGroup = new GradeGroup
//                {
//                    GradeGroupCode = gradeGroup.Code,
//                    Description = gradeGroup.Description,
//                    IsActive = gradeGroup.IsActive,
//                    CampusId = gradeGroup.CampusId
//                };
//                await _repository.AddAsync(newGradeGroup);
//                if (await _repository.SaveChanges())
//                {
//                    return RedirectToAction("GradeGroup");
//                }
//                ModelState.AddModelError("", "Error While Saving to Database");
//            }
//            return View(gradeGroup);
//        }
//        [HttpGet]
//        public async Task<IActionResult> DeleteGradeGroup(int id)
//        {
//            var temp = await _repository.GetGradeGroupById(id);
//            if (temp == null)
//            {
//                return NotFound();
//            }
//            await _repository.Delete(temp);
//            if (await _repository.SaveChanges())
//            {
//                return RedirectToAction("GradeGroup");
//            }
//            return RedirectToAction("GradeGroup");
//        }
//        #endregion

//        #region Grades
//        public async Task<IActionResult> Grades()
//        {
//            var result = await _repository.GetClassGrades();
//            return View(result);
//        }
//        [HttpGet]
//        public async Task<IActionResult> AddGrades()
//        {
//            ViewBag.GradeGroups = await _repository.GetGradeGroups();
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> AddGrades(AddGrades grade)
//        {
//            if (ModelState.IsValid)
//            {
//                var newGrade = new Grades
//                {
//                    GradesCode = grade.Code,
//                    Description = grade.Description,
//                    IsActive = grade.IsActive,
//                    GradeGroupId = grade.GradeGroupId
//                };
//                await _repository.AddAsync(newGrade);
//                if (await _repository.SaveChanges())
//                {
//                    return RedirectToAction("Grades");
//                }
//                ModelState.AddModelError("", "Error While Saving to Database");
//            }
//            return View(grade);
//        }
//        [HttpGet]
//        public async Task<IActionResult> DeleteClassGrade(int id)
//        {
//            var temp = await _repository.GetClassGradeById(id);
//            if (temp == null)
//            {
//                return NotFound();
//            }
//            await _repository.Delete(temp);
//            if (await _repository.SaveChanges())
//            {
//                return RedirectToAction("Grades");
//            }
//            return RedirectToAction("Grades");
//        }
//        #endregion

//        #region Sections
//        public async Task<IActionResult> GradesSection()
//        {
//            var result = await _repository.GetGradeSections();
//            return View(result);
//        }

//        [HttpGet]
//        public async Task<IActionResult> AddGradesSection()
//        {
//            ViewBag.Grades = await _repository.GetClassGrades();
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> AddGradesSection(AddGradesSection section)
//        {
//            if (ModelState.IsValid)
//            {
//                var newSection = new GradeSection
//                {
//                    GradesSectionCode = section.Code,
//                    Description = section.Description,
//                    IsActive = section.IsActive,
//                    GradeId = section.GradesId
//                };
//                await _repository.AddAsync(newSection);
//                if (await _repository.SaveChanges())
//                {
//                    return RedirectToAction("GradesSection");
//                }
//                ModelState.AddModelError("", "Error While Saving to Database");
//            }
//            return View(section);
//        }
//        [HttpGet]
//        public async Task<IActionResult> DeleteGradeSection(int id)
//        {
//            var temp = await _repository.GetGradeSectionById(id);
//            if (temp == null)
//            {
//                return NotFound();
//            }
//            await _repository.Delete(temp);
//            if (await _repository.SaveChanges())
//            {
//                return RedirectToAction("GradeSection");
//            }
//            return RedirectToAction("GradeSection");
//        }
//        #endregion
//    }
//}
