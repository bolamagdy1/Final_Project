using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Final_Project.Controllers
{
    public class HomeCoursesController : Controller
    {
        AppDbContext _context;
        public HomeCoursesController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var applicant = _context.applicants.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var Courses = _context.As_Cs.Include(c=>c.Course).Where(c=>c.ApplicantId != applicant.ApplicantId).ToList();
            return View(Courses);
        }

        [Authorize(Roles ="admin")]
        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCourse(Course course, IFormCollection formFile)
        {
            var temp = new MemoryStream();
            var test = formFile.Files[0];
            test.CopyTo(temp);
            course.Image = temp.ToArray();
            _context.courses.Add( course );
            _context.SaveChanges();
            return RedirectToAction("Index", "HomeCourses");
        }
        [HttpGet]
        public IActionResult adding(int id)
        {
            var applicant = _context.applicants.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var course = _context.courses.FirstOrDefault(i => i.CourseId == id);
            Applicant_Course Applicant_Course = new Applicant_Course() { ApplicantId = applicant.ApplicantId, CourseId = course.CourseId };
            _context.As_Cs.Add(Applicant_Course);
            _context.SaveChanges();
            return RedirectToAction("Index", "HomeCourses");
        }
        public IActionResult MyApplies()
        {
            var applicant = _context.applicants.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var Course = _context.As_Cs.Include(j => j.Course).Include(j => j.Applicant)
                .Where(a => a.ApplicantId == applicant.ApplicantId).ToList();
            return View(Course);
        }
    }
}
