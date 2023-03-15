using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using myWebApp.ViewModels;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Index";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.AccessDeniedPath = "/Auth/AccessDenied";
        //options.Cookie.Expiration = TimeSpan.FromMinutes(1);
    });

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("MyPolicy", policy =>
//    {
//        policy.Requirements.Add(new MyAuthorizationRequirement("", ""));
//    });
//});

//builder.Services.AddScoped<IAuthorizationPolicyProvider, PermissionBasedPolicyProvider>();

//builder.Services.AddSingleton<IAuthorizationHandler, MyAuthorizationHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Employee.Create", policy => policy.RequireClaim("Permission", "Employee.Create"));
    options.AddPolicy("Employee.Read", policy => policy.RequireClaim("Permission", "Employee.Read"));
    options.AddPolicy("Employee.Update", policy => policy.RequireClaim("Permission", "Employee.Update"));
    options.AddPolicy("Employee.Delete", policy => policy.RequireClaim("Permission", "Employee.Delete"));
    options.AddPolicy("Shoora.Create", policy => policy.RequireClaim("Permission", "Shoora.Create"));
    options.AddPolicy("Shoora.Read", policy => policy.RequireClaim("Permission", "Shoora.Read"));
    options.AddPolicy("Shoora.Update", policy => policy.RequireClaim("Permission", "Shoora.Update"));
    options.AddPolicy("Shoora.Delete", policy => policy.RequireClaim("Permission", "Shoora.Delete"));
    options.AddPolicy("School.Create", policy => policy.RequireClaim("Permission", "School.Create"));
    options.AddPolicy("School.Read", policy => policy.RequireClaim("Permission", "School.Read"));
    options.AddPolicy("School.Update", policy => policy.RequireClaim("Permission", "School.Update"));
    options.AddPolicy("School.Delete", policy => policy.RequireClaim("Permission", "School.Delete"));
    options.AddPolicy("Campus.Create", policy => policy.RequireClaim("Permission", "Shoora.Create"));
    options.AddPolicy("Campus.Read", policy => policy.RequireClaim("Permission", "Campus.Read"));
    options.AddPolicy("Campus.Update", policy => policy.RequireClaim("Permission", "Campus.Update"));
    options.AddPolicy("Campus.Delete", policy => policy.RequireClaim("Permission", "Campus.Delete"));
    options.AddPolicy("SchoolSection.Create", policy => policy.RequireClaim("Permission", "SchoolSection.Create"));
    options.AddPolicy("SchoolSection.Read", policy => policy.RequireClaim("Permission", "SchoolSection.Read"));
    options.AddPolicy("SchoolSection.Update", policy => policy.RequireClaim("Permission", "SchoolSection.Update"));
    options.AddPolicy("SchoolSection.Delete", policy => policy.RequireClaim("Permission", "SchoolSection.Delete"));
    options.AddPolicy("Department.Create", policy => policy.RequireClaim("Permission", "Department.Create"));
    options.AddPolicy("Department.Read", policy => policy.RequireClaim("Permission", "Department.Read"));
    options.AddPolicy("Department.Update", policy => policy.RequireClaim("Permission", "Department.Update"));
    options.AddPolicy("Department.Delete", policy => policy.RequireClaim("Permission", "Department.Delete"));
    options.AddPolicy("SubDepartment.Create", policy => policy.RequireClaim("Permission", "SubDepartment.Create"));
    options.AddPolicy("SubDepartment.Read", policy => policy.RequireClaim("Permission", "SubDepartment.Read"));
    options.AddPolicy("SubDepartment.Update", policy => policy.RequireClaim("Permission", "SubDepartment.Update"));
    options.AddPolicy("SubDepartment.Delete", policy => policy.RequireClaim("Permission", "SubDepartment.Delete"));
    options.AddPolicy("Designation.Create", policy => policy.RequireClaim("Permission", "Designation.Create"));
    options.AddPolicy("Designation.Read", policy => policy.RequireClaim("Permission", "Designation.Read"));
    options.AddPolicy("Designation.Update", policy => policy.RequireClaim("Permission", "Designation.Update"));
    options.AddPolicy("Designation.Delete", policy => policy.RequireClaim("Permission", "Designation.Delete"));
    options.AddPolicy("Heads.Create", policy => policy.RequireClaim("Permission", "Heads.Create"));
    options.AddPolicy("Heads.Read", policy => policy.RequireClaim("Permission", "Heads.Read"));
    options.AddPolicy("Heads.Update", policy => policy.RequireClaim("Permission", "Heads.Update"));
    options.AddPolicy("Heads.Delete", policy => policy.RequireClaim("Permission", "Heads.Delete"));
    options.AddPolicy("TeachingMethodology.Create", policy => policy.RequireClaim("Permission", "TeachingMethodology.Create"));
    options.AddPolicy("TeachingMethodology.Read", policy => policy.RequireClaim("Permission", "TeachingMethodology.Read"));
    options.AddPolicy("TeachingMethodology.Update", policy => policy.RequireClaim("Permission", "TeachingMethodology.Update"));
    options.AddPolicy("TeachingMethodology.Delete", policy => policy.RequireClaim("Permission", "TeachingMethodology.Delete"));
    options.AddPolicy("Roles.Create", policy => policy.RequireClaim("Permission", "Roles.Create"));
    options.AddPolicy("Roles.Read", policy => policy.RequireClaim("Permission", "Roles.Read"));
    options.AddPolicy("Roles.Update", policy => policy.RequireClaim("Permission", "Roles.Update"));
    options.AddPolicy("Roles.Delete", policy => policy.RequireClaim("Permission", "Roles.Delete"));
    options.AddPolicy("Users.Create", policy => policy.RequireClaim("Permission", "Users.Create"));
    options.AddPolicy("Users.Read", policy => policy.RequireClaim("Permission", "Users.Read"));
    options.AddPolicy("Users.Update", policy => policy.RequireClaim("Permission", "Users.Update"));
    options.AddPolicy("Users.Delete", policy => policy.RequireClaim("Permission", "Users.Delete"));
    options.AddPolicy("Grade.Create", policy => policy.RequireClaim("Permission", "Grade.Create"));
    options.AddPolicy("Grade.Read", policy => policy.RequireClaim("Permission", "Grade.Read"));
    options.AddPolicy("Grade.Update", policy => policy.RequireClaim("Permission", "Grade.Update"));
    options.AddPolicy("Grade.Delete", policy => policy.RequireClaim("Permission", "Grade.Delete"));
    options.AddPolicy("Section.Create", policy => policy.RequireClaim("Permission", "Section.Create"));
    options.AddPolicy("Section.Read", policy => policy.RequireClaim("Permission", "Section.Read"));
    options.AddPolicy("Section.Update", policy => policy.RequireClaim("Permission", "Section.Update"));
    options.AddPolicy("Section.Delete", policy => policy.RequireClaim("Permission", "Section.Delete"));
    options.AddPolicy("Subject.Create", policy => policy.RequireClaim("Permission", "Subject.Create"));
    options.AddPolicy("Subject.Read", policy => policy.RequireClaim("Permission", "Subject.Read"));
    options.AddPolicy("Subject.Update", policy => policy.RequireClaim("Permission", "Subject.Update"));
    options.AddPolicy("Subject.Delete", policy => policy.RequireClaim("Permission", "Subject.Delete"));
    options.AddPolicy("Notebook.Create", policy => policy.RequireClaim("Permission", "Notebook.Create"));
    options.AddPolicy("Notebook.Read", policy => policy.RequireClaim("Permission", "Notebook.Read"));
    options.AddPolicy("Notebook.Update", policy => policy.RequireClaim("Permission", "Notebook.Update"));
    options.AddPolicy("Notebook.Delete", policy => policy.RequireClaim("Permission", "Notebook.Delete"));
    options.AddPolicy("Book.Create", policy => policy.RequireClaim("Permission", "Book.Create"));
    options.AddPolicy("Book.Read", policy => policy.RequireClaim("Permission", "Book.Read"));
    options.AddPolicy("Book.Update", policy => policy.RequireClaim("Permission", "Book.Update"));
    options.AddPolicy("Book.Delete", policy => policy.RequireClaim("Permission", "Book.Delete"));
    options.AddPolicy("Unit.Create", policy => policy.RequireClaim("Permission", "Unit.Create"));
    options.AddPolicy("Unit.Read", policy => policy.RequireClaim("Permission", "Unit.Read"));
    options.AddPolicy("Unit.Update", policy => policy.RequireClaim("Permission", "Unit.Update"));
    options.AddPolicy("Unit.Delete", policy => policy.RequireClaim("Permission", "Unit.Delete"));
    options.AddPolicy("Chapter.Create", policy => policy.RequireClaim("Permission", "Chapter.Create"));
    options.AddPolicy("Chapter.Read", policy => policy.RequireClaim("Permission", "Chapter.Read"));
    options.AddPolicy("Chapter.Update", policy => policy.RequireClaim("Permission", "Chapter.Update"));
    options.AddPolicy("Chapter.Delete", policy => policy.RequireClaim("Permission", "Chapter.Delete"));
    options.AddPolicy("Topic.Create", policy => policy.RequireClaim("Permission", "Topic.Create"));
    options.AddPolicy("Topic.Read", policy => policy.RequireClaim("Permission", "Topic.Read"));
    options.AddPolicy("Topic.Update", policy => policy.RequireClaim("Permission", "Topic.Update"));
    options.AddPolicy("Topic.Delete", policy => policy.RequireClaim("Permission", "Topic.Delete"));
    options.AddPolicy("SubTopic.Create", policy => policy.RequireClaim("Permission", "SubTopic.Create"));
    options.AddPolicy("SubTopic.Read", policy => policy.RequireClaim("Permission", "SubTopic.Read"));
    options.AddPolicy("SubTopic.Update", policy => policy.RequireClaim("Permission", "SubTopic.Update"));
    options.AddPolicy("SubTopic.Delete", policy => policy.RequireClaim("Permission", "SubTopic.Delete"));
    options.AddPolicy("Year.Create", policy => policy.RequireClaim("Permission", "Year.Create"));
    options.AddPolicy("Year.Read", policy => policy.RequireClaim("Permission", "Year.Read"));
    options.AddPolicy("Year.Update", policy => policy.RequireClaim("Permission", "Year.Update"));
    options.AddPolicy("Year.Delete", policy => policy.RequireClaim("Permission", "Year.Delete"));
    options.AddPolicy("Term.Create", policy => policy.RequireClaim("Permission", "Term.Create"));
    options.AddPolicy("Term.Read", policy => policy.RequireClaim("Permission", "Term.Read"));
    options.AddPolicy("Term.Update", policy => policy.RequireClaim("Permission", "Term.Update"));
    options.AddPolicy("Term.Delete", policy => policy.RequireClaim("Permission", "Term.Delete"));
    options.AddPolicy("Planner.Create", policy => policy.RequireClaim("Permission", "Planner.Create"));
    options.AddPolicy("Planner.Read", policy => policy.RequireClaim("Permission", "Planner.Read"));
    options.AddPolicy("Planner.Update", policy => policy.RequireClaim("Permission", "Planner.Update"));
    options.AddPolicy("Planner.Delete", policy => policy.RequireClaim("Permission", "Planner.Delete"));
    options.AddPolicy("Academic Planning", policy => policy.RequireClaim("Permission", "Academic Planning"));
});


builder.Services.AddDbContext<SchoolDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SeeratDB"));
});

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
//.AddEntityFrameworkStores<SchoolDBContext>();
builder.Services.AddTransient<IEFRepository, EFRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
//app.MapControllerRoute(
//    name: "DeleteMonth",
//    pattern: "{controller=AcademicCalendar}/{action=DeleteMonth}/{id?}/{TermId?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

//app.UseMvcWithDefaultRoute();


//app.UseMvc(IRouteBuilder routes =>
//routes.MapRoute("default", "{controller = Auth}/{Action = Index}/{id?}"));
//app.UseMvc(routes =>
//{
//    routes.MapRoute(name: "DeleteMonth",
//       template: "{controller = AcademicCalendar}/{Action = DeleteMonth}/{id}/{TermId}");
//    routes.MapRoute ( name: "DeleteMonth",
//        template: "{controller = AcademicCalendar}/{Action = DeleteMonth}/{id}/{TermId}");
//});
app.Run();
