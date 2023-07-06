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
        public IActionResult CreateCourse(Course course, IFormCollection formFile)
        {
            var temp = new MemoryStream();
            var test = formFile.Files[0];
            test.CopyTo(temp);
            course.Image = temp.ToArray();
            _context.courses.Add( course );
            _context.SaveChanges();
            return RedirectToAction("GetAllForAdmin", "HomeCourses");
        }
        [HttpGet]
        public IActionResult adding(int id)
        {
            var applicant = _context.applicants.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var course = _context.courses.FirstOrDefault(i => i.CourseId == id);
            if (course.Price > 0)
            {
                pay();
                return RedirectToAction("GetAllForApplicant", "HomeCourses");
            }
            else
            {
                Applicant_Course Applicant_Course = new Applicant_Course() { ApplicantId = applicant.ApplicantId, CourseId = course.CourseId };
                _context.As_Cs.Add(Applicant_Course);
                _context.SaveChanges();
                return RedirectToAction("GetAllForApplicant", "HomeCourses");
            }
        }

        private void pay()
        {
            
        }

        public IActionResult MyApplies()
        {
            var applicant = _context.applicants.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));
            var Course = _context.As_Cs.Include(j => j.Course).Include(j => j.Applicant)
                .Where(a => a.ApplicantId == applicant.ApplicantId).ToList();
            return View(Course);
        }
        //public ActionResult PaymentWithPaypal(string Cancel = null)
        //{
        //    //getting the apiContext  
        //    APIContext apiContext = PaypalConfigration.GetAPIContext();
        //    try
        //    {
        //        //A resource representing a Payer that funds a payment Payment Method as paypal  
        //        //Payer Id will be returned when payment proceeds or click to pay  


        //        string payerId = Request.Params["PayerID"];
        //        if (string.IsNullOrEmpty(payerId))
        //        {
        //            //this section will be executed first because PayerID doesn't exist  
        //            //it is returned by the create function call of the payment class  
        //            // Creating a payment  
        //            // baseURL is the url on which paypal sendsback the data.  
        //            string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/PaymentWithPayPal?";
        //            //here we are generating guid for storing the paymentID received in session  
        //            //which will be used in the payment execution  
        //            var guid = Convert.ToString((new Random()).Next(100000));
        //            //CreatePayment function gives us the payment approval url  
        //            //on which payer is redirected for paypal account payment  
        //            var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
        //            //get links returned from paypal in response to Create function call  
        //            var links = createdPayment.links.GetEnumerator();
        //            string paypalRedirectUrl = null;
        //            while (links.MoveNext())
        //            {
        //                Links lnk = links.Current;
        //                if (lnk.rel.ToLower().Trim().Equals("approval_url"))
        //                {
        //                    //saving the payapalredirect URL to which user will be redirected for payment  
        //                    paypalRedirectUrl = lnk.href;
        //                }
        //            }
        //            // saving the paymentID in the key guid  
        //            Session.Add(guid, createdPayment.id);
        //            return Redirect(paypalRedirectUrl);
        //        }
        //        else
        //        {
        //            // This function exectues after receving all parameters for the payment  
        //            var guid = Request.Params["guid"];
        //            var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
        //            //If executed payment failed then we will show payment failure message to user  
        //            if (executedPayment.state.ToLower() != "approved")
        //            {
        //                return View("FailureView");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("FailureView");
        //    }
        //    //on successful payment, show success page to user.  
        //    return View("SuccessView");
        //}
    }
}
