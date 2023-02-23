//using Entities.Models;
//using Infrastructure.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using myWebApp.ViewModels.Placement;
//using myWebApp.ViewModels.School;
//using static System.Collections.Specialized.BitVector32;

//namespace myWebApp.Controllers
//{
//    public class PlacementController : Controller
//    {
//        private readonly IEFRepository _repository;

//        public PlacementController(IEFRepository repository)
//        {
//            _repository = repository;
//        }
//        public async Task<IActionResult> Index()
//        {
//            var result = await _repository.GetPlacements();
//            return View(result);
//        }
//        [HttpGet]
//        public async Task<IActionResult> AddPlacement()
//        {
//            ViewBag.Locations = await _repository.GetLocations();
//            ViewBag.Departments = await _repository.GetDepartments();
//            ViewBag.Designations = await _repository.GetDesignations();
//            ViewBag.Grades = await _repository.GetGrades();
//            ViewBag.EmployeeTypes = await _repository.GetEmployeeTypes();
//            ViewBag.Shifts = await _repository.GetShifts();
//            ViewBag.Sections = await _repository.GetSections();
//            ViewBag.Projects = await _repository.GetProjects();
//            ViewBag.Schools = await _repository.GetSchools();
//            ViewBag.Campuses = await _repository.GetCampuses();
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> AddPlacement(AddPlacement placement)
//        {
//            if (ModelState.IsValid)
//            {
//                var newPlacement = new Placement
//                {
//                    Period = placement.Period,
//                    PlacementDate = placement.PlacementDate,
//                    LocationId = placement.LocationId,
//                    DepartmentId = placement.DepartmentId,
//                    DesignationId = placement.DesignationId,
//                    GradeId = placement.GradeId,
//                    EmployeeTypeId = placement.EmployeeTypeId,
//                    ShiftId = placement.ShiftId,
//                    SectionId = placement.SectionId,
//                    ProjectId = placement.ProjectId,
//                    SchoolId = placement.SchoolId,
//                    CampusId = placement.CampusId,
//                    ContractStartDate = placement.ContractStartDate,
//                    ContractEndDate = placement.ContractEndDate,
//                    IsContractRenewable = placement.IsContractRenewable
//                };
//                await _repository.AddAsync(newPlacement);
//                if (await _repository.SaveChanges())
//                {
//                    return RedirectToAction("index");
//                }
//                ModelState.AddModelError("", "Error While Saving to Database");
//            }
//            return View(placement);
//        }
//        [HttpGet]
//        public async Task<IActionResult> UpdatePlacement(int id)
//        {
//            var temp = await _repository.GetPlacementById(id);
//            if (temp == null)
//            {
//                return NotFound();
//            }
//            var placement = new UpdatePlacement
//            {
//                PlacementId = temp.PlacementId,
//                ContractEndDate = temp.ContractEndDate,
//                ContractStartDate = temp.ContractStartDate,
//                CampusId = temp.CampusId,
//                IsContractRenewable = temp.IsContractRenewable,
//                SchoolId = temp.SchoolId,
//                SectionId = temp.SectionId,
//                ShiftId = temp.ShiftId,
//                DepartmentId = temp.DepartmentId,
//                DesignationId = temp.DesignationId,
//                EmployeeTypeId = temp.EmployeeTypeId,
//                GradeId = temp.GradeId,
//                LocationId = temp.LocationId,
//                PlacementDate = temp.PlacementDate,
//                Period = temp.Period,
//                ProjectId = temp.ProjectId
//            };
//            return View(placement);
//        }
//        [HttpPost]
//        public async Task<IActionResult> UpdatePlacement(UpdatePlacement placement)
//        {
//            var temp = await _repository.GetPlacementById(placement.PlacementId);
//            if (temp == null)
//            {
//                return NotFound();
//            }
//            temp.ContractEndDate = placement.ContractEndDate;
//            temp.ContractStartDate = placement.ContractStartDate;
//            temp.CampusId = placement.CampusId;
//            temp.IsContractRenewable = placement.IsContractRenewable;
//            temp.SchoolId = placement.SchoolId;
//            temp.SectionId = placement.SectionId;
//            temp.ShiftId = placement.ShiftId;
//            temp.DepartmentId = placement.DepartmentId;
//            temp.DesignationId = placement.DesignationId;
//            temp.EmployeeTypeId = placement.EmployeeTypeId;
//            temp.GradeId = placement.GradeId;
//            temp.LocationId = placement.LocationId;
//            temp.PlacementDate = placement.PlacementDate;
//            temp.Period = placement.Period;
//            temp.ProjectId = placement.ProjectId;
//            await _repository.UpdateAsync(temp);
//            if (await _repository.SaveChanges())
//            {
//                return RedirectToAction("Index");
//            }
//            ModelState.AddModelError("", "Error While Saving to Database");
//            return View(placement);
//        }
//        [HttpGet]
//        public async Task<IActionResult> DeletePlacement(int id)
//        {
//            var temp = await _repository.GetPlacementById(id);
//            if (temp == null)
//            {
//                return NotFound();
//            }
//            await _repository.Delete(temp);
//            if (await _repository.SaveChanges())
//            {
//                return RedirectToAction("Index");
//            }
//            return RedirectToAction("Index");
//        }
//    }
//}
