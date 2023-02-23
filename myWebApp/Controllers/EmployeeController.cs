//using Entities.Models;
//using Infrastructure.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using myWebApp.ViewModels.Employee;

//namespace myWebApp.Controllers
//{
//    public class EmployeeController : Controller
//    {
//        private IEFRepository _repository;

//        public EmployeeController(IEFRepository repository)
//        {
//            _repository = repository;
//        }

//        public IActionResult Education()
//        {
//            return View();
//        }
//        [HttpGet]
//        public IActionResult AddEducation()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult AddEducation(AddEducation education)
//        {
//            return View();
//        }
//    }
//}