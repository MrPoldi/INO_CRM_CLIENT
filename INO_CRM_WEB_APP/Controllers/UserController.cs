using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using INO_CRM_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace INO_CRM_WEB_APP.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public async Task<UserModel> GetUserAsync(int id)
        {
            string apiUrl = "http://localhost:50060/";
            UserModel user = new UserModel();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseMessage = await client.GetAsync("api/users/" + id);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<UserModel>(userResponse);
                }

            }

            return user;
        }

        public async Task<IActionResult> Index()
        {
            string apiUrl = "http://localhost:50060/";
            List<UserModel> users = new List<UserModel>();

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

            }

            return View(users);
        }
        

        // GET: User/Details/5
        public async Task<ActionResult> Details(int id)
        {
            UserModel user = await GetUserAsync(id);
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            UserModel user = await GetUserAsync(id);
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(int id, UserModel user)
        {
            user.UserId = id;
            try
            {
                string apiUrl = "http://localhost:50060/";
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage responseMessage = await client.PutAsJsonAsync("api/users/" + id, user);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            UserModel user = await GetUserAsync(id);
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                string apiUrl = "http://localhost:50060/";                
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage responseMessage = await client.DeleteAsync("api/users/" + id);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}