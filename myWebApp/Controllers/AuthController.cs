using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using myWebApp.ViewModels.Auth;
using System.Drawing;
using System.Security.Claims;

namespace myWebApp.Controllers
{

    public class AuthController : Controller
    {
        private readonly IEFRepository _repository;
        private readonly SchoolDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AuthController(IEFRepository repository, SchoolDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task< IActionResult >Index()
        {
            ClaimsPrincipal claim = HttpContext.User;
            if (claim.Identity.IsAuthenticated) RedirectToAction("Index", "Director");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginVM login)
        {
            var user = await _db.Users.Where(x => x.Email == login.Email).FirstOrDefaultAsync();
            var Employee = await _db.Employees.Where(x => x.Email == login.Email).FirstOrDefaultAsync();
            var Parent = await _db.Parents.Where(x => x.Email == login.Email).FirstOrDefaultAsync();
            var Student = await _db.Students.Where(x => x.Email == login.Email).FirstOrDefaultAsync();
            ViewBag.LoginStatus = true;
            if (ModelState.IsValid)
            {
                if (login.Password == user.Password)
                {
                    if(Employee != null)
                    {
                        var emp = await _db.Employees.Where(x => x.Email == login.Email).FirstOrDefaultAsync();
                        var role = await _db.Roles.Where(x => x.RoleId == emp.RoleId).FirstOrDefaultAsync();
                        var userPermissions = await _db.UserPermissions.Where(x => x.RoleId == emp.RoleId).ToListAsync();
                        List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, login.Email),
                        new Claim(ClaimTypes.Sid, Convert.ToString(emp?.EmployeeId)),
                        new Claim("DepartmentId",Convert.ToString(emp?.DepartmentId)),
                        new Claim(ClaimTypes.Role, role.RollName),
                        new Claim("UserName", emp.FName + " " + emp.LName)
                    };
                        foreach (var perm in userPermissions)
                        {
                            var permission = await _db.Permissions.Where(x => x.PermissionId == perm.PermissionId).FirstOrDefaultAsync();
                            //if (permission.PermissionDbName == null) permission.PermissionDbName = "";
                            var Claim = new Claim("Permission", permission?.PermissionDbName);
                            claims.Add(Claim);
                        }
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties authenticationProperties = new AuthenticationProperties()
                        {
                            AllowRefresh = true
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProperties);
                        return RedirectToAction("Index", "Director");
                    }
                    else if(Parent != null)
                    {
                        //var pr = await _db.Employees.Where(x => x.Email == login.Email).FirstOrDefaultAsync();
                        var role = await _db.Roles.Where(x => x.RoleId == 12).FirstOrDefaultAsync();
                        var userPermissions = await _db.UserPermissions.Where(x => x.RoleId == 12).ToListAsync();
                        List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, login.Email),
                        new Claim(ClaimTypes.Sid, Convert.ToString(Parent?.ParentId)),
                        //new Claim("DepartmentId",Convert.ToString(emp?.DepartmentId)),
                        new Claim(ClaimTypes.Role, role.RollName),
                        new Claim("UserName", Parent.FName + " " + Parent.LName)
                    };
                        foreach (var perm in userPermissions)
                        {
                            var permission = await _db.Permissions.Where(x => x.PermissionId == perm.PermissionId).FirstOrDefaultAsync();
                            //if (permission.PermissionDbName == null) permission.PermissionDbName = "";
                            var Claim = new Claim("Permission", permission?.PermissionDbName);
                            claims.Add(Claim);
                        }
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties authenticationProperties = new AuthenticationProperties()
                        {
                            AllowRefresh = true,
                            IsPersistent = true
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProperties);
                        return RedirectToAction("Dashboard", "Parent");
                    }
                    else
                    {
                        var userPermissions = await _db.UserPermissions.Where(x => x.RoleId == 13).ToListAsync();
                        var role = await _db.Roles.Where(x => x.RoleId == 13).FirstOrDefaultAsync();
                        List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, login.Email),
                        new Claim(ClaimTypes.Sid, Convert.ToString(Student?.StudentId)),
                        //new Claim("DepartmentId",Convert.ToString(emp?.DepartmentId)),
                        new Claim(ClaimTypes.Role, role.RollName),
                        new Claim("UserName", Student.FName + " " + Student.LName)
                    };
                        foreach (var perm in userPermissions)
                        {
                            var permission = await _db.Permissions.Where(x => x.PermissionId == perm.PermissionId).FirstOrDefaultAsync();
                            //if (permission.PermissionDbName == null) permission.PermissionDbName = "";
                            var Claim = new Claim("Permission", permission?.PermissionDbName);
                            claims.Add(Claim);
                        }
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties authenticationProperties = new AuthenticationProperties()
                        {
                            AllowRefresh = true,
                            IsPersistent = true
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProperties);
                        return RedirectToAction("Dashboard", "Student");
                    }
                }
                else
                {
                    ViewBag.LoginStatus = false;
                }
            }
            return View(login);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
