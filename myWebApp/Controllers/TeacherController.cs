using Entities.Models;
using myWebApp.ViewModels.Teacher;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
        //public IActionResult Index()
        //{
        //    return View();
        //}
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
    }
}
