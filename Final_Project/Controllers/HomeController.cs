using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;
        public HomeController( AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var applicant = _context.applicants.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var jobss = _context.applicants_jobs.Include(j=>j.Job)
                .Where(a=>a.ApplicantId == applicant.ApplicantId).ToList();

            var jobs= _context.jobs.Include(i=>i.Company).ToList();

            foreach (var job in jobss)
            {
                jobs.Remove(job.Job);
            }
            //apps_jobs.
            return View(jobs);
        }
        [Authorize(Roles = "applicant")]
        public IActionResult Privacy()
        {
            return View();
        }
        
        //[HttpGet]
        //public IActionResult Details(int id)
        //{
        //    var applicant = _context.applicants.FirstOrDefault(i => i.ApplicantId == id);
        //    //Stream stream 
        //    ViewBag.img = System.Text.Encoding.Default.GetString(applicant.Picture);
        //    return View(applicant);
        //}
    }
}
