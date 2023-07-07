using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace Final_Project.Controllers
{
    public class ZoomController : Controller
    {
        private readonly string TokenFilePath = "";
        private string authorization_header 
        { 
            get
            {
                var PlainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{System.Configuration.ConfigurationManager.AppSettings["ClintId"]}:{System.Configuration.ConfigurationManager.AppSettings["ClintSecret"]}");
                var encodedString = System.Convert.ToBase64String(PlainTextBytes);
                return $"Basic {encodedString}";
            }
        }
        public IActionResult SignIn()
        {
            return Redirect(string.Format(System.Configuration.ConfigurationManager.AppSettings["AuthorizationUrl"], System.Configuration.ConfigurationManager.AppSettings["CilntId"], System.Configuration.ConfigurationManager.AppSettings["RedirectUrl"]));
        }
        public void oauthredirect()
        {

        }
    }
}
