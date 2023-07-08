using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Configuration;
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
                var PlainTextBytes = System.Text.Encoding.UTF8.GetBytes($"6UbTQISoQMq9dH9xWef8nA:AdUt4X988izvj9a8wLFOSoIVVHPRHgWM");
                var encodedString = System.Convert.ToBase64String(PlainTextBytes);
                return $"Basic {encodedString}";
            }
        }
        public IActionResult SignIn()
        {
            return Redirect(string.Format("https://zoom.us/oauth/authorize?response_type=code&client_id=6UbTQISoQMq9dH9xWef8nA&redirect_uri=http://localhost:13244/Zoom/oauthredirect", "6UbTQISoQMq9dH9xWef8nA", "http://localhost:13244/Zoom/oauthredirect"));
        }
               
            
        public void oAuthredirect(string code)
        {
            
            var restClient = new RestClient("https://zoom.us/v2/users/7dxgr5cqht@privaterelay.appleid.com/meetings");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", authorization_header);
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", "http://localhost:13244/Zoom/oauthredirect");
            request.AddParameter("client_id", "6UbTQISoQMq9dH9xWef8nA");
            request.AddParameter("client_secret", "AdUt4X988izvj9a8wLFOSoIVVHPRHgWM");
            var response = restClient.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText(TokenFilePath, response.Content);
            }
            

        }
    }

}
