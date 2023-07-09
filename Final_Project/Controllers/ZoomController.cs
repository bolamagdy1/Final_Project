using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Configuration;
using System.Security.Policy;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Final_Project.Controllers
{
    
    public class ZoomController : Controller
    {
         private readonly IConfiguration _configuration;
        public ZoomController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //need to change path for your device
        
        private readonly string TokenFilePath = "..\\Final_Project\\Credentials\\OauthToken.json";
        private readonly string UserDetails = "..\\Final_Project\\Credentials\\UserDetails.json";
        private string authorization_header 
        { 
            get
            {
                var PlainTextBytes = System.Text.Encoding.UTF8.GetBytes($"dwBOuoTaSt2AsjXdjCL2w:WGUtjaX6tO01jSiwJvU9sQ8M3d9K6Xda");
                //var encodedString = System.Convert.ToBase64String(PlainTextBytes);
                return $"Basic dwBOuoTaSt2AsjXdjCL2w:WGUtjaX6tO01jSiwJvU9sQ8M3d9K6Xda";
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
            request.AddHeader("Authorization", "Basic ZHdCT3VvVGFTdDJBc2pYZGpDTDJ3OldHVXRqYVg2dE8wMWpTaXdKdlU5c1E4TTNkOUs2WGRh");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("code", code);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("redirect_uri", "http://localhost:13244/Zoom/oauthredirect");
            var response = restClient.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText(TokenFilePath, response.Content);
            }
        }
        public IActionResult RefreshToken()
        {
            RestClient restClient = new RestClient("https://zoom.us/oauth/token");
            var token = JObject.Parse(System.IO.File.ReadAllText(TokenFilePath));
            var request = new RestRequest();
            request.AddHeader("Authorization", "Basic ZHdCT3VvVGFTdDJBc2pYZGpDTDJ3OldHVXRqYVg2dE8wMWpTaXdKdlU5c1E4TTNkOUs2WGRh");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", "eyJzdiI6IjAwMDAwMSIsImFsZyI6IkhTNTEyIiwidiI6IjIuMCIsImtpZCI6IjViMWZjNTEyLWYwZGYtNGRmNS1hNmRmLTg2MGRiYzMzZWE2OSJ9.eyJ2ZXIiOjksImF1aWQiOiI1NmU5MmM2ZmY4NGUyYmEyYmZjMTljZjM2MDA3NTY5MiIsImNvZGUiOiJ6eEhoSHp0SUdIczA1UlpaaFNGUnlhOUlMNjd4bm54eWciLCJpc3MiOiJ6bTpjaWQ6ZHdCT3VvVGFTdDJBc2pYZGpDTDJ3IiwiZ25vIjowLCJ0eXBlIjoxLCJ0aWQiOjAsImF1ZCI6Imh0dHBzOi8vb2F1dGguem9vbS51cyIsInVpZCI6ImNrdFRNcWZmVDF1THBoRDZtbVdHUUEiLCJuYmYiOjE2ODg4NDM3MTYsImV4cCI6MTY5NjYxOTcxNiwiaWF0IjoxNjg4ODQzNzE2LCJhaWQiOiJFWUFxd0NtRlJzMm0yeng3VThUaFVBIn0.Y_cnUK-kwtrHelpzvB5-ZI0z1e-1qRKhPDh8pcLfodp74EaS6Ky2wTJq6X4kkFPnHfaRTPhg-odbYQuShJtSWw");

            var response = restClient.Post(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText(TokenFilePath, response.Content); 
            }

            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public ActionResult CreateMeeting()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMeeting(Meeting meeting)
        {
            var token = JObject.Parse(System.IO.File.ReadAllText(TokenFilePath));
            var userDetails = JObject.Parse(System.IO.File.ReadAllText(UserDetails));
            var access_token = token["access_token"];
            var userId = userDetails["id"];
            var meetingModel = new JObject();
            meetingModel["topic"] = meeting.Topic;
            meetingModel["agenda"] = meeting.Agenda;
            meetingModel["start_time"] = DateTime.Now;
            //ToString("yyyy-MM-dd") + "T" + TimeSpan.FromHours(meeting.Time).ToString()
            meetingModel["duration"] = meeting.Duration;
            var model = JsonConvert.SerializeObject(meetingModel);

            RestRequest restRequest = new RestRequest();
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Authorization", "Bearer eyJzdiI6IjAwMDAwMSIsImFsZyI6IkhTNTEyIiwidiI6IjIuMCIsImtpZCI6IjUwYWQ3MTMxLTQwYjktNGQ0My05MDQxLWJiOGQ2MjEyZTc1MiJ9.eyJ2ZXIiOjksImF1aWQiOiI1NmU5MmM2ZmY4NGUyYmEyYmZjMTljZjM2MDA3NTY5MiIsImNvZGUiOiJ6eEhoSHp0SUdIczA1UlpaaFNGUnlhOUlMNjd4bm54eWciLCJpc3MiOiJ6bTpjaWQ6ZHdCT3VvVGFTdDJBc2pYZGpDTDJ3IiwiZ25vIjowLCJ0eXBlIjowLCJ0aWQiOjQsImF1ZCI6Imh0dHBzOi8vb2F1dGguem9vbS51cyIsInVpZCI6ImNrdFRNcWZmVDF1THBoRDZtbVdHUUEiLCJuYmYiOjE2ODg4OTM0NjAsImV4cCI6MTY4ODg5NzA2MCwiaWF0IjoxNjg4ODkzNDYwLCJhaWQiOiJFWUFxd0NtRlJzMm0yeng3VThUaFVBIn0.KFkHj8Y9dFzsVfyGyYdAQjpu5Jpt4SVpq4r5zbsie8DRHTnsZQMqXdpmy1MarS_3i89jXohcOR1COaOA6zFBBQ");
            restRequest.AddParameter("application/json", model, ParameterType.RequestBody);
            //restRequest.AddBody(model);

            RestClient restClient = new RestClient($"https://api.zoom.us/v2/users/{userId}/meetings");
            //restClient.BaseUrl = new Uri(string.Format(Configuration Manager.AppSettings["MeetingUrl"], userI

            var response = restClient.Post(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //D:\ITI\Projects\Grad Pro\Final_Project\Final_Project\Credentials
                System.IO.File.WriteAllText("..\\Final_Project\\Credentials\\ZoomResponse.json", response.Content);
            }
            return RedirectToAction("Index", "Home");
        }
    }

}
