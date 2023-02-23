using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Entities.Models;
using System.Reflection.Metadata;
using Infrastructure.Configurations;

namespace Infrastructure.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Head> Heads { get; set; }
        public DbSet<Shoora> Shoora { get; set; }
        public DbSet<SubDepartment> subDepartments { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<BookAllocation> bookAllocations { get; set; }
        public DbSet<Year> years { get; set; }
        public DbSet<Term> terms { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        //public DbSet<Month> months { get; set; }
        public DbSet<Chapter> chapters { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Topic> topics { get; set; }
        public DbSet<SubTopic> subTopics { get; set; }
        public DbSet<ChapterAllocation> ChapterAllocations { get; set; }
        public DbSet<UnitAllocation> UnitAllocations { get; set; }
        public DbSet<TopicAllocation> TopicAllocations { get; set; }
        public DbSet<SubTopicAllocation> SubTopicAllocations { get; set; }
        public DbSet<SubjectAllocation> SubjectAllocations { get; set; }
        public DbSet<SubjectTeacherAllocation> SubjectTeacherAllocations { get; set; }
        public DbSet<ResourceNoteBook> ResourceNoteBooks { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<SchoolSection> SchoolSections { get; set; }
        public DbSet<TeachingMethodology> TeachingMethodologies { get; set; }
        public DbSet<ChapterQuestions> ChapterQuestions { get; set; }
        public DbSet<ChapterAnswers> ChapterAnswers { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Apply Configurations
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new DesignationConfiguration());
            modelBuilder.ApplyConfiguration(new SubDepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new HeadConfiguration());
            modelBuilder.ApplyConfiguration(new ShooraConfiguration());
            modelBuilder.ApplyConfiguration(new GenderConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new GradeConfiguration());
            modelBuilder.ApplyConfiguration(new SectionConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new BookAllocationConfiguration());
            modelBuilder.ApplyConfiguration(new YearConfiguration());
            modelBuilder.ApplyConfiguration(new TermConfiguration());
            modelBuilder.ApplyConfiguration(new HolidayConfiguration());
            //modelBuilder.ApplyConfiguration(new MonthConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterConfiguration());
            modelBuilder.ApplyConfiguration(new UnitConfiguration());
            modelBuilder.ApplyConfiguration(new TopicConfiguration());
            modelBuilder.ApplyConfiguration(new SubTopicConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterAllocationConfiguration());
            modelBuilder.ApplyConfiguration(new UnitAllocationConfiguration());
            modelBuilder.ApplyConfiguration(new TopicAllocationConfiguration());
            modelBuilder.ApplyConfiguration(new SubTopicAllocationConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectAllocationConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectTeacherAllocationConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceNoteBookConfiguration());
            modelBuilder.ApplyConfiguration(new SchoolConfiguration());
            modelBuilder.ApplyConfiguration(new CampusConfiguration());
            modelBuilder.ApplyConfiguration(new SchoolSectionConfiguration());
            modelBuilder.ApplyConfiguration(new TeachingMethodologyConfiguration());
            modelBuilder.ApplyConfiguration(new RolesConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterQuestionsConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterAnswersConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionsConfiguration());
            modelBuilder.ApplyConfiguration(new UserPermissionsConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
