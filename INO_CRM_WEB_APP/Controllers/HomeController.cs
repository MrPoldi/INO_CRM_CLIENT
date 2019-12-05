using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using INO_CRM_WEB_APP.Models;
using INO_CRM_API.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace INO_CRM_WEB_APP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            string apiUrl = "http://localhost:50060/";
            List<UserModel> users = new List<UserModel>();

            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseMessage = await client.GetAsync("api/users");

                if (responseMessage.IsSuccessStatusCode)
                {
                    string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                    users = JsonConvert.DeserializeObject<List<UserModel>>(userResponse);
                }

            }

            return View(users);
        }

        public async Task<IActionResult> Roles()
        {
            string apiUrl = "http://localhost:50060/";
            List<UserModel> users = new List<UserModel>();
            List<RoleModel> roles = new List<RoleModel>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseMessage = await client.GetAsync("api/users");

                if (responseMessage.IsSuccessStatusCode)
                {
                    string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                    users = JsonConvert.DeserializeObject<List<UserModel>>(userResponse);
                }

                
                foreach(UserModel u in users)
                {
                    roles.Add(u.Role);
                }

            }

            return View(roles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
