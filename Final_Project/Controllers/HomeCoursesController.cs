using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPal.Api;
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
        public IActionResult GetAllForApplicant()
        {
            var applicant = _context.applicants.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var coursess = _context.As_Cs.Include(c => c.Course).Where(c => c.ApplicantId == applicant.ApplicantId).ToList();
            var courses = _context.courses.ToList();

            foreach (var course in coursess)
            {
                courses.Remove(course.Course);
            }

            return View(courses);
        }
        public IActionResult GetAllForAdmin()
        {
            var Courses = _context.courses.ToList();
            return View(Courses);
        }
        [Authorize(Roles ="admin")]
        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCourseAsync(Course course,IFormCollection formFile)
        {
            string target_folder = "wwwroot/Server/";

            var _FileName1 = course.CourseId + formFile.Files[0].FileName;
            string path = Path.Combine(target_folder + "Course_Images", _FileName1);

            using (Stream fileStream = new FileStream(path, FileMode.Create))
            {
                await formFile.Files[0].CopyToAsync(fileStream);
            }
            course.Image = _FileName1;

            _context.courses.Add(course);
            _context.SaveChanges();
            return RedirectToAction("GetAllForAdmin", "HomeCourses");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _context.courses.FirstOrDefault(i => i.CourseId == id);
            return View(course);
        }
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            _context.Update(course);
            _context.SaveChanges();
            return RedirectToAction("GetAllForAdmin");
        }
        [HttpGet]
        public IActionResult adding(int id)
        {
            var applicant = _context.applicants.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var course = _context.courses.FirstOrDefault(i => i.CourseId == id);
            
            if (course.Price > 0)
            {
                Applicant_Course Applicant_Course = new Applicant_Course() { ApplicantId = applicant.ApplicantId, CourseId = course.CourseId };
                _context.As_Cs.Add(Applicant_Course);
                _context.SaveChanges();
                TempData["price"] = course.Price.ToString();
                return RedirectToAction("Index", "Paypal");
            }
            else
            {
                Applicant_Course Applicant_Course = new Applicant_Course() { ApplicantId = applicant.ApplicantId, CourseId = course.CourseId };
                _context.As_Cs.Add(Applicant_Course);
                _context.SaveChanges();
                return RedirectToAction("GetAllForApplicant", "HomeCourses");
            }
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
