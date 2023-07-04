using Final_Project.Data;
using Final_Project.Data.ViewModels;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Final_Project.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ApplicantController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Applicant applicant, IFormCollection formFile)
        {
            //applicant.Picture = Convert.ToBase64String(formFile);
            //formFile.CopyTo(applicant.Picture)

            var user = await _userManager.FindByEmailAsync(applicant.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(applicant);
            }

            var newUser = new ApplicationUser()
            {
                FullName = applicant.ApplicantName,
                Email = applicant.EmailAddress,
                UserName = applicant.ApplicantName
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, applicant.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Applicant);

            else
            {
                TempData["bola"] = newUserResponse;
                return View(applicant);
            }

            var temp = new MemoryStream();
            var test = formFile.Files.FirstOrDefault();
            test.CopyTo(temp);
            applicant.Picture = temp.ToArray();

            var temp2 = new MemoryStream();
            var test2 = formFile.Files.LastOrDefault();
            test2.CopyTo(temp2);
            applicant.CV = temp2.ToArray();

            var sameuser = await _userManager.FindByEmailAsync(applicant.EmailAddress);
            applicant.Password = sameuser.PasswordHash;


            _context.applicants.Add(applicant);
            _context.SaveChanges();
            return RedirectToAction("Index","Home");
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
            TempData["abdo"] = null;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult Apply(int id)
        {
            var applicant = _context.applicants.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var job = _context.jobs.FirstOrDefault(i => i.JobId == id);
            Applicant_Job applicant_job = new Applicant_Job() { ApplicantId = applicant.ApplicantId, JobId = job.JobId };
            _context.applicants_jobs.Add(applicant_job);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult MyApplies()
        {
            var applicant = _context.applicants.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var jobss = _context.applicants_jobs.Include(j => j.Job).Include(j=>j.Job.Company)
                .Where(a => a.ApplicantId == applicant.ApplicantId).ToList();
            return View(jobss);
        }
    }
}
