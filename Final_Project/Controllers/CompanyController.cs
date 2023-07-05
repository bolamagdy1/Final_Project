using Final_Project.Data.ViewModels;
using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Net.Mail;
using System.Net;

namespace Final_Project.Controllers
{
    public class CompanyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public CompanyController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateCompany()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany(Company company, IFormCollection formFile)
        {
            //applicant.Picture = Convert.ToBase64String(formFile);
            //formFile.CopyTo(applicant.Picture)

            var user = await _userManager.FindByEmailAsync(company.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(company);
            }

            var newUser = new ApplicationUser()
            {
                FullName = company.CompanyName,
                Email = company.EmailAddress,
                UserName = company.CompanyName
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, company.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Company);
            else
            {
                TempData["joseph"] = newUserResponse;
                return View(company);
            }

            var temp = new MemoryStream();
            var test = formFile.Files[0];
            test.CopyTo(temp);
            company.Logo = temp.ToArray();

            var temp2 = new MemoryStream();
            var test2 = formFile.Files[1];
            test2.CopyTo(temp2);
            company.Doc1 = temp2.ToArray();

            var temp3 = new MemoryStream();
            var test3 = formFile.Files[2];
            test3.CopyTo(temp3);
            company.Doc2 = temp3.ToArray();

            var sameuser = await _userManager.FindByEmailAsync(company.EmailAddress);
            company.Password = sameuser.PasswordHash;

            _context.companies.Add(company);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login() => View(new Login());

        [HttpPost]
        public async Task<IActionResult> Login(Login loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            TempData["abdo"] = user.Email;
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                TempData["Error"] = "Wrong credentials. Please, try again!";
                return View(loginVM);
            }

            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles ="company")]
        public IActionResult Post()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Post(Job job)
        {

            var company = _context.companies.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            job.CompanyId = company.CompanyId;
            _context.jobs.Add(job);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize(Roles =("company"))]
        public IActionResult myjobs()
        {
            var company = _context.companies.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var jobs = _context.jobs.Where(c=>c.CompanyId == company.CompanyId).ToList();
            return View(jobs);
        }
        [HttpGet]
        public IActionResult EditPost(int id)
        {
            var job = _context.jobs.FirstOrDefault(i => i.JobId == id);
            return View(job);
        }
        [HttpPost]
        public IActionResult EditPost(Job job)
        {
            var company = _context.companies.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            job.CompanyId = company.CompanyId;
            _context.jobs.Update(job);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Appliers() 
        {
            var company = _context.companies.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var jobs = _context.jobs.Where(e => e.CompanyId == company.CompanyId).ToList();

            List<Applicant_Job> apps_jobs = new List<Applicant_Job>();

            string[] arr = new string[apps_jobs.Count];

            foreach (var job in jobs)
            {
                apps_jobs = _context.applicants_jobs.Include(j => j.Job).Where(a => a.JobId == job.JobId).ToList();
                arr.Append(job.Jop_Title);
            }
            ViewBag.bola = new List<string>();
                ViewBag.bola = arr;
            var applicants = _context.applicants_jobs
                .Include(a => a.Applicant)
                .Include(a => a.Job)
                .ThenInclude(j => j.Company)
                .Where(ww=>ww.Job.CompanyId == company.CompanyId)
                .ToList();
            return View(applicants);
        }
        public IActionResult Waiting(int id)
        {
            string fromMail = "bolamagdy085@gmail.com";
            string fromPassword = "";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Test Subject";
            message.To.Add(new MailAddress("abdalrhmanyasser@icloud.com"));
            message.Body = "<html><body> Hello From VS </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);



            return RedirectToAction("Appliers");
        }
    }
}
