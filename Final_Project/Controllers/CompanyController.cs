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
using System.Linq;

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
            string target_folder = "wwwroot/Server/";

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

            var _FileName1 = company.CompanyName + formFile.Files[0].FileName;
            string path = Path.Combine(target_folder + "Logos", _FileName1);

            using (Stream fileStream = new FileStream(path, FileMode.Create))
            {
                await formFile.Files[0].CopyToAsync(fileStream);
            }

            var _FileName2 = company.CompanyName + formFile.Files[1].FileName;
            string path2 = Path.Combine(target_folder + "Docs1", _FileName2);

            using (Stream fileStream = new FileStream(path2, FileMode.Create))
            {
                await formFile.Files[1].CopyToAsync(fileStream);
            }

            var _FileName3 = company.CompanyName + formFile.Files[2].FileName;
            string path3 = Path.Combine(target_folder + "Docs2", _FileName3);

            using (Stream fileStream = new FileStream(path3, FileMode.Create))
            {
                await formFile.Files[2].CopyToAsync(fileStream);
            }

            var sameuser = await _userManager.FindByEmailAsync(company.EmailAddress);
            company.Password = sameuser.PasswordHash;
            company.ConfirmPassword = sameuser.PasswordHash;
            company.Logo = _FileName1;
            company.Doc1 = _FileName2;
            company.Doc2 = _FileName3;

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
            var company = _context.companies.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            if (company.Trusted == true)
                return View();
            else
                return RedirectToAction("Index", "Home");
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
            return RedirectToAction("myjobs");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var job= _context.jobs.FirstOrDefault(i=>i.JobId == id);
            _context.jobs.Remove(job);
            _context.SaveChanges();
            return RedirectToAction("myjobs");
        }
        [HttpGet]
        public IActionResult Appliers()
        {
            var company = _context.companies.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            //var jobs = _context.jobs.Where(e => e.CompanyId == company.CompanyId).ToList();

            //List<Applicant_Job> apps_jobs = new List<Applicant_Job>();

            //string[] arr = new string[apps_jobs.Count];

            //foreach (var job in jobs)
            //{
            //    apps_jobs = _context.applicants_jobs.Include(j => j.Job).Where(a => a.JobId == job.JobId).ToList();
            //    arr.Append(job.Jop_Title);
            //}
            //ViewBag.bola = new List<string>();
            //    ViewBag.bola = arr;
            var applicants = _context.applicants_jobs
                .Include(a => a.Applicant)
                .Include(a => a.Job)
                .ThenInclude(j => j.Company)
                .Where(ww=>ww.Job.CompanyId == company.CompanyId)
                .ToList();
            return View(applicants);
        }
        public IActionResult Filterbyjobtilte(string word)
        {
            var company = _context.companies.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            //var jobs = _context.jobs.Where(e => e.CompanyId == company.CompanyId).ToList();

            //List<Applicant_Job> apps_jobs = new List<Applicant_Job>();

            //string[] arr = new string[apps_jobs.Count];

            //foreach (var job in jobs)
            //{
            //    apps_jobs = _context.applicants_jobs.Include(j => j.Job).Where(a => a.JobId == job.JobId).ToList();
            //    arr.Append(job.Jop_Title);
            //}
            //ViewBag.bola = new List<string>();
            //ViewBag.bola = arr;
            var applicants = _context.applicants_jobs
                .Include(a => a.Applicant)
                .Include(a => a.Job)
                .ThenInclude(j => j.Company)
                .Where(ww => ww.Job.CompanyId == company.CompanyId)
                .ToList();
            if (string.IsNullOrEmpty(word))
            { 
                return View("Appliers", applicants);
            }
            else
            {
                var filteredResultNew = applicants.Where(n => n.Job.Jop_Title.Contains(word, StringComparison.CurrentCultureIgnoreCase)).ToList();
                return View("Appliers", filteredResultNew);
            }
        }
        public IActionResult Filterbyapplicanttilte(string word)
        {
            var company = _context.companies.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            //var jobs = _context.jobs.Where(e => e.CompanyId == company.CompanyId).ToList();

            //List<Applicant_Job> apps_jobs = new List<Applicant_Job>();

            //string[] arr = new string[apps_jobs.Count];

            //foreach (var job in jobs)
            //{
            //    apps_jobs = _context.applicants_jobs.Include(j => j.Job).Where(a => a.JobId == job.JobId).ToList();
            //    arr.Append(job.Jop_Title);
            //}
            //ViewBag.bola = new List<string>();
            //ViewBag.bola = arr;
            var applicants = _context.applicants_jobs
                .Include(a => a.Applicant)
                .Include(a => a.Job)
                .ThenInclude(j => j.Company)
                .Where(ww => ww.Job.CompanyId == company.CompanyId)
                .ToList();
            if (string.IsNullOrEmpty(word))
            {
                return View("Appliers", applicants);
            }
            else
            {
                var filteredResultNew = applicants.Where(n => n.Applicant.Title.Contains(word, StringComparison.CurrentCultureIgnoreCase)).ToList();
                return View("Appliers", filteredResultNew);
            }
        }
        public IActionResult accepting(int id)
        {
            string fromMail = "bolamagdy085@gmail.com";
            string fromPassword = "hgkguhlfwqsgfucz";

            var applicant = _context.applicants.FirstOrDefault(i => i.ApplicantId == id);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Test Subject";
            message.To.Add(new MailAddress(applicant.EmailAddress));
            message.Body = "<html><body> Congratulations From JE_Journy </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);

            var applied = _context.applicants_jobs.FirstOrDefault(i => i.ApplicantId == applicant.ApplicantId);
            var accept = _context.accepted
                .FirstOrDefault(a => a.ApplicantId == applicant.ApplicantId && a.JobId == applied.JobId);
            accept.Extra = "Accepted";
            _context.applicants_jobs.Remove(applied);
            _context.SaveChanges();

            return RedirectToAction("Appliers");
        }
        public IActionResult rejecting(int id)
        {
            string fromMail = "bolamagdy085@gmail.com";
            string fromPassword = "hgkguhlfwqsgfucz";

            var applicant = _context.applicants.FirstOrDefault(i => i.ApplicantId == id);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Test Subject";
            message.To.Add(new MailAddress(applicant.EmailAddress));
            message.Body = "<html><body> Sorry From JE_Journy </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);

            var applied = _context.applicants_jobs.FirstOrDefault(i => i.ApplicantId == applicant.ApplicantId);
            var accept = _context.accepted
                .FirstOrDefault(a => a.ApplicantId == applicant.ApplicantId && a.JobId == applied.JobId);
            accept.Extra = "Rejected";
            _context.applicants_jobs.Remove(applied);
            _context.SaveChanges();

            return RedirectToAction("Appliers");
        }
        public IActionResult AcceptedList()
        {
            var company = _context.companies.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var jobs = _context.jobs.Where(e => e.CompanyId == company.CompanyId).ToList();
            //List<Applicant> applicants = new List<Applicant>();
            //var applicants = _context.applicants_jobs
            //    .Include(a => a.Applicant)
            //    .Include(a => a.Job)
            //    .ThenInclude(j => j.Company)
            //    .Where(ww => ww.Job.CompanyId == company.CompanyId)
            //    .ToList();
            List<Accepted> accepteds = new List<Accepted>();
            List<Applicant> acceptedApplicants = new List<Applicant>();
            Applicant_Job applicant_job = new Applicant_Job();
            List<Applicant_Job> applicant_jobs = new List<Applicant_Job>();
            foreach (var job in jobs)
            {
                accepteds = _context.accepted
                .Include(a => a.Applicant)
                .Include(c => c.Job)
                .Where(a => a.Extra == "Accepted" && a.Job.JobId == job.JobId).ToList();
                foreach (var accepted in accepteds)
                {
                    acceptedApplicants.Add(accepted.Applicant);
                    applicant_job.Applicant = accepted.Applicant;
                    applicant_job.Job = accepted.Job;
                    applicant_job.ApplicantId = accepted.ApplicantId;
                    applicant_job.JobId = accepted.JobId;
                    applicant_jobs.Add(applicant_job);
                }
            }

            //acceptedApplicants 
            return View("AppliersAccepted", applicant_jobs);

            //var acceptedlist = _context.applicants.Where(j=>j)
        }
        public IActionResult RejectedList()
        {
            var company = _context.companies.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var jobs = _context.jobs.Where(e => e.CompanyId == company.CompanyId).ToList();
            //List<Applicant> applicants = new List<Applicant>();
            //var applicants = _context.applicants_jobs
            //    .Include(a => a.Applicant)
            //    .Include(a => a.Job)
            //    .ThenInclude(j => j.Company)
            //    .Where(ww => ww.Job.CompanyId == company.CompanyId)
            //    .ToList();
            List<Accepted> accepteds = new List<Accepted>();
            List<Applicant> acceptedApplicants = new List<Applicant>();
            Applicant_Job applicant_job = new Applicant_Job();
            List<Applicant_Job> applicant_jobs = new List<Applicant_Job>();
            foreach (var job in jobs)
            {
                accepteds = _context.accepted
                .Include(a => a.Applicant)
                .Include(c => c.Job)
                .Where(a => a.Extra == "Rejected" && a.Job.JobId == job.JobId).ToList();
                foreach (var accepted in accepteds)
                {
                    acceptedApplicants.Add(accepted.Applicant);
                    applicant_job.Applicant = accepted.Applicant;
                    applicant_job.Job = accepted.Job;
                    applicant_job.ApplicantId = accepted.ApplicantId;
                    applicant_job.JobId = accepted.JobId;
                    applicant_jobs.Add(applicant_job);
                }
            }

            //acceptedApplicants 
            return View("AppliersAccepted", applicant_jobs);

            //var acceptedlist = _context.applicants.Where(j=>j)
        }
    }
}
