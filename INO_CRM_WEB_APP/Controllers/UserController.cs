﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using INO_CRM_API.Models;
using INO_CRM_WEB_APP.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            UserModel user = new UserModel();

            HttpResponseMessage responseMessage = await ApiHelper.GetAsync("api/users/" + id);

            if (responseMessage.IsSuccessStatusCode)
            {
                string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserModel>(userResponse);
            }
            
            return user;
        }

        public async Task<List<UserModel>> GetUsersAsync(bool paginated, int id = 0)
        {
            List<UserModel> users = new List<UserModel>();
            HttpResponseMessage responseMessage;
            if (!paginated)
            {
                responseMessage = await ApiHelper.GetAsync("api/users");
            }
            else
            {
                responseMessage = await ApiHelper.GetAsync("api/users/page/" + id);
            }
           
            if (responseMessage.IsSuccessStatusCode)
            {
                string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<UserModel>>(userResponse);
            }
            return users;
        }

        public async Task<IActionResult> Index()
        {           
            List<UserModel> users = await GetUsersAsync(paginated:false); 
            return View(users);
        }

        public async Task<IActionResult> Page(int id)
        {
            List<UserModel> users = await GetUsersAsync(true, id);

            HttpResponseMessage responseMessage = await ApiHelper.GetAsync("api/users/pages");
            if (responseMessage.IsSuccessStatusCode)
            {
                string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                int pageCount = int.Parse(userResponse);
                ViewBag.pageCount = pageCount;
            }

            ViewBag.currentPage = id;
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