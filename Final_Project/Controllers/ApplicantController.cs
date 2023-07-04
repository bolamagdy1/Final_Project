using Final_Project.Data;
using Final_Project.Data.ViewModels;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
