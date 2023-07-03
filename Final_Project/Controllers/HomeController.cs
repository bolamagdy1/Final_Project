using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
