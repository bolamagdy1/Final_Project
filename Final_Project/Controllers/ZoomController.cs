using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Configuration;

namespace Final_Project.Controllers
{
    public class ZoomController : Controller
    {
        //need to change path for your device
        private readonly string TokenFilePath = "D:\\ITI\\Projects\\Grad Pro\\Final_Project\\Final_Project\\Credentials\\OauthToken.json";
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
            return Redirect(string.Format("https://zoom.us/oauth/authorize?response_type=code&client_id=dwBOuoTaSt2AsjXdjCL2w&redirect_uri=http://localhost:13244/Zoom/oauthredirect", "dwBOuoTaSt2AsjXdjCL2w", "http://localhost:13244/Zoom/oauthredirect"));
        }
        public void oAuthredirect(string code)
        {
            //RestClient restClient = new RestClient();
            var restClient = new RestClient("https://zoom.us/oauth/token?response_type=code&client_id=dwBOuoTaSt2AsjXdjCL2w&redirect_uri=http://localhost:13244/Zoom/oauthredirect");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", authorization_header);
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", "http://localhost:13244/Zoom/oauthredirect");
            var response = restClient.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText(TokenFilePath, response.Content);
            }
            //var request = new RestRequest();
            //restClient.BaseUrl= new Uri(string.Format(System.Configuration.ConfigurationManager.AppSettings["AccessTokenUrl"],code, System.Configuration.ConfigurationManager.AppSettings["RedirectUrl"]));
            //request.AddHeader("Authorization", string.Format(authorization_header));
            //var response = restClient.Post(request);
            //if(response.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    System.IO.File.WriteAllText(TokenFilePath,response.Content);
            //}

        }
    }
}
