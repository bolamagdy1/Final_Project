using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Applicant applicant,IFormCollection formFile)
        {
            //applicant.Picture = Convert.ToBase64String(formFile);
            //formFile.CopyTo(applicant.Picture)


            var temp = new MemoryStream();
            //formFile.Files.ToArray();
            var test =  formFile.Files.FirstOrDefault();
            test.CopyTo(temp);
            applicant.Picture = temp.ToArray();


            _context.applicants.Add(applicant);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var applicant = _context.applicants.FirstOrDefault(i => i.ApplicantId == id);
            //Stream stream 
            ViewBag.img = System.Text.Encoding.Default.GetString(applicant.Picture);
            return View(applicant);
        }
    }
}
