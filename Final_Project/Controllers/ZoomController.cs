using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using Final_Project.Data;

namespace Final_Project.Controllers
{
    
    public class zeze
    {
        public string start_url { get; set; }
        public string join_url { get; set; }
        public string start_time { get; set; }
    }
    public class ZoomController : Controller
    {
         private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        public ZoomController(IConfiguration configuration,AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
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
            meeting.Time -= 7;
            meetingModel["start_time"] = meeting.Date.ToString("yyyy-MM-dd") + "T" + TimeSpan.FromHours(meeting.Time).ToString();
            //ToString("yyyy-MM-dd") + "T" + TimeSpan.FromHours(meeting.Time).ToString()
            meetingModel["duration"] = meeting.Duration;
            var model = JsonConvert.SerializeObject(meetingModel);

            RestRequest restRequest = new RestRequest();
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Authorization", "Bearer eyJzdiI6IjAwMDAwMSIsImFsZyI6IkhTNTEyIiwidiI6IjIuMCIsImtpZCI6IjAyMzc5NTNlLTIyMGQtNDM4YS1hNzNjLTBmMjg0YmYzM2EwZCJ9.eyJ2ZXIiOjksImF1aWQiOiI1NmU5MmM2ZmY4NGUyYmEyYmZjMTljZjM2MDA3NTY5MiIsImNvZGUiOiJ6eEhoSHp0SUdIczA1UlpaaFNGUnlhOUlMNjd4bm54eWciLCJpc3MiOiJ6bTpjaWQ6ZHdCT3VvVGFTdDJBc2pYZGpDTDJ3IiwiZ25vIjowLCJ0eXBlIjowLCJ0aWQiOjksImF1ZCI6Imh0dHBzOi8vb2F1dGguem9vbS51cyIsInVpZCI6ImNrdFRNcWZmVDF1THBoRDZtbVdHUUEiLCJuYmYiOjE2ODkxNTc4MTcsImV4cCI6MTY4OTE2MTQxNywiaWF0IjoxNjg5MTU3ODE3LCJhaWQiOiJFWUFxd0NtRlJzMm0yeng3VThUaFVBIn0.mlQwekLMYl0Otj6AEm7A2jnaew1wcmXiT-sNnWhwDDZLUftlKn4PlDiVuRxwkejoVBw_pU_43-C-xaOcFbTRAg");
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
            //string path = "..\\Final_Project\\Credentials\\ZoomResponse.json";
            //string readText = System.IO.File.ReadAllText(path);
            //JArray convert = JArray.Parse(readText);

            //zeze bla = JsonSerializer.Deserialize<zeze>(readText)!;

            
            
            string fileName = "..\\Final_Project\\Credentials\\ZoomResponse.json";
            string jsonString = System.IO.File.ReadAllText(fileName);
            zeze bla = System.Text.Json.JsonSerializer.Deserialize<zeze>(jsonString)!;

            //TempData["start_url"] = bla.start_url;
            //TempData["join_url"] = bla.join_url;
            //TempData["start_time"] = bla.start_time;

            string fromMail = "bolamagdy085@gmail.com";
            string fromPassword = "hgkguhlfwqsgfucz";

            //var applicant = _context.applicants.FirstOrDefault(i => i.ApplicantId == id);
            var company = _context.companies.FirstOrDefault(e => e.EmailAddress == TempData.Peek("abdo"));

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Test Subject";
            message.To.Add(new MailAddress(company.EmailAddress));
            message.Body = $"Hello Sir."+$"\n Zoom Meeting will start in {bla.start_time}"+$"\n This is Start URL {bla.start_url}" +$"\n and this is Join URL {bla.join_url}";
            //message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);

            return RedirectToAction("Appliers", "Company");
        }
        public IActionResult ZoomDetails()
        {
            return View();
        }
    }

    internal class NewClass
    {
        public string Item { get; }
        public object Start_url { get; }

        public NewClass(string item, object start_url)
        {
            Item = item;
            Start_url = start_url;
        }

        public override bool Equals(object? obj)
        {
            return obj is NewClass other &&
                   Item == other.Item &&
                   EqualityComparer<object>.Default.Equals(Start_url, other.Start_url);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Item, Start_url);
        }
        
    }
}
