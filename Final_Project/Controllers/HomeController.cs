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
            return View();
        }
        public IActionResult search(string word)
        {
            var applicant = _context.applicants.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var jobss = _context.applicants_jobs.Include(j => j.Job)
                .Where(a => a.ApplicantId == applicant.ApplicantId).ToList();

            var jobs = _context.jobs.Include(i => i.Company).ToList();

            foreach (var job in jobss)
            {
                jobs.Remove(job.Job);
            }

            if (string.IsNullOrEmpty(word))
            {
                return View("Jobs",jobs);
            }
            else
            {
               //var filteredResultNew = jobs.Where(n => string.Equals(n.Jop_Title, word, StringComparison.CurrentCultureIgnoreCase) || string.Equals(n.Jop_Description, word, StringComparison.CurrentCultureIgnoreCase)).ToList();
                var filteredResultNew = jobs.Where(n => n.Jop_Title.Contains(word, StringComparison.CurrentCultureIgnoreCase) || n.Jop_Description.Contains(word, StringComparison.CurrentCultureIgnoreCase)).ToList();
                return View("Jobs",filteredResultNew);
            }
        }
        public IActionResult Jobs()
        {
            try
            {
                var applicant = _context.applicants.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
                var jobss = _context.applicants_jobs.Include(j => j.Job)
                    .Where(a => a.ApplicantId == applicant.ApplicantId).ToList();

                var jobs = _context.jobs.Include(i => i.Company).ToList();

                foreach (var job in jobss)
                {
                    jobs.Remove(job.Job);
                }
                //apps_jobs.
                return View(jobs);
            }
            catch 
            {
                var jobs = _context.jobs.Include(j=>j.Company).ToList();
                return View(jobs);
            }
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

