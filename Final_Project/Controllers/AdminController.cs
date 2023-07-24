using Final_Project.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult List_Companies()
        {
            var companies = _context.companies.Where(t=>t.Trusted == false).ToList();
            return View(companies);
        }
        public IActionResult Approve(int id)
        {
            var company = _context.companies.FirstOrDefault(i => i.CompanyId == id);
            company.Trusted = true;
            _context.SaveChanges();
            return RedirectToAction("List_Companies");
        }
		public IActionResult Delete(int id)
		{
			var company = _context.companies.FirstOrDefault(i => i.CompanyId == id);
			_context.companies.Remove(company);
			_context.SaveChanges();
			return RedirectToAction("List_Companies");
		}
	}
}
