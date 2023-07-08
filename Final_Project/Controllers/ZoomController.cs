using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Configuration;

namespace Final_Project.Controllers
{
    public class ZoomController : Controller
    {
        //need to change path for your device
        private readonly string TokenFilePath = "D:\\Finals\\Final_Project\\Final_Project\\Credentials\\OauthToken.json";
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
        public void oAuthredirect(string code)
        {
            RestClient restClient = new RestClient();
            var request = new RestRequest();
            //restClient.BuildUri= new Uri(string.Format(System.Configuration.ConfigurationManager.AppSettings["AccessTokenUrl"],code, System.Configuration.ConfigurationManager.AppSettings["RedirectUrl"]));
            request.AddHeader("Authorization", string.Format(authorization_header));
            var response = restClient.Post(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText(TokenFilePath,response.Content);
            }

        }
    }
}
