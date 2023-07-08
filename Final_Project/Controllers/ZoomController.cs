using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Configuration;
using System.Security.Policy;
using static System.Net.WebRequestMethods;

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
                var PlainTextBytes = System.Text.Encoding.UTF8.GetBytes($"dwBOuoTaSt2AsjXdjCL2w:dwBOuoTaSt2AsjXdjCL2w");
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

            var restClient = new RestClient("https://zoom.us/oauth/token");
            var request = new RestRequest();
            request.Method = Method.Post;


            //    curl - X POST https://zoom.us/oauth/token -d 'grant_type=account_credentials'
            //    //-d 'account_id=5yiPLwmTTpQVBnMxOlf32q' -H 'Host: zoom.us'
            ////-H 'Authorization: Basic aGwbwxOgK6eGHEO0W1DOCv5WCODeVxoet7DFEON7bR23gP5qEW7cmeWCbCEO3ApBEWlRwCVpDWB=='

            //request.AddBody();

            //request.AddHeader("Host", "zoom.us");
            //request.AddHeader("Authorization", "Basic Q2xpZW50X0lEOkNsaWVudF9TZWNyZXQ=");
            //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //request.AddParameter("code", code);
            //request.AddParameter("grant_type", "authorization_code");
            //request.AddParameter("redirect_uri", "http://localhost:13244/Zoom/oauthredirect");

            var response = restClient.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText(TokenFilePath, response.Content);
            }
            //RestClient restClient = new RestClient();
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
