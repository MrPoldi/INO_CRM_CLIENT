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

            HttpResponseMessage responseMessage = await ApiHelper.GetAsync("api/users/" + id, HttpContext.Session.GetString("token"));

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
                responseMessage = await ApiHelper.GetAsync("api/users", HttpContext.Session.GetString("token"));
            }
            else
            {
                responseMessage = await ApiHelper.GetAsync("api/users/page/" + id, HttpContext.Session.GetString("token"));
            }
           
            if (responseMessage.IsSuccessStatusCode)
            {
                string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<UserModel>>(userResponse);
            }
            return users;
        }

        public async Task<List<UserModel>> GetUsersByName(string searchName)
        {
            List<UserModel> users = new List<UserModel>();
            HttpResponseMessage response;
            response = await ApiHelper.GetAsync("api/users/page/1?searchName=" + searchName, HttpContext.Session.GetString("token"));

            if (response.IsSuccessStatusCode)
            {
                string userResponse = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<UserModel>>(userResponse);
            }
            return users;
        }

        public async Task<List<UserModel>> GetUsersByDate(int id, DateTime minDate, DateTime maxDate)
        {
            List<UserModel> users = new List<UserModel>();
            HttpResponseMessage response;
            string sMaxDate = maxDate.ToShortDateString();
            string sMinDate = minDate.ToShortDateString();

            response = await ApiHelper.GetAsync("api/users/page/" + id + "?sMinDate=" + sMinDate + "&sMaxDate=" + sMaxDate, HttpContext.Session.GetString("token"));

            if (response.IsSuccessStatusCode)
            {
                string userResponse = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<UserModel>>(userResponse);
            }

            return users;
        }



        public async Task<IActionResult> Index()
        {           
            List<UserModel> users = await GetUsersAsync(paginated:false); 
            return View(users);
        }

        public async Task<IActionResult> Page(int id, DateTime minDate, DateTime maxDate)
        {
            List<UserModel> users;
            //if (minDate == null && maxDate == null){
              //  users = await GetUsersAsync(true, id);
                //ViewBag.currentPage = id;
            //}
            //else
            //{
                if (minDate == DateTime.Parse("01-01-0001")) minDate = DateTime.Parse("01-01-1900");
                if (maxDate == DateTime.Parse("01-01-0001")) maxDate = DateTime.Today;
                users = await GetUsersByDate(id, minDate, maxDate);                
                ViewBag.minDate = minDate.ToString("yyyy-MM-dd");
                ViewBag.maxDate = maxDate.ToString("yyyy-MM-dd");
            
            //}


            string sMaxDate = maxDate.ToShortDateString();
            string sMinDate = minDate.ToShortDateString();

            HttpResponseMessage responseMessage = await ApiHelper.GetAsync("api/users/pages/?sMinDate=" + sMinDate + "&sMaxDate=" + sMaxDate, HttpContext.Session.GetString("token"));
            if (responseMessage.IsSuccessStatusCode)
            {
                string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                int pageCount = int.Parse(userResponse);
                if(id > pageCount)
                {
                    ViewBag.currentPage = 1;
                }
                else
                {
                    ViewBag.currentPage = id;
                }
                ViewBag.pageCount = pageCount;
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
            if(user.Password != null)
            {
                user.Password = EncryptionHelper.GetMd5Hash(user.Password);
            }
            
            try
            {
                HttpResponseMessage responseMessage = await ApiHelper.PutAsync("api/users/" + id, user, HttpContext.Session.GetString("token"));
                
                return RedirectToAction(nameof(Page), new { id = 1});
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
                HttpResponseMessage responseMessage = await ApiHelper.DeleteAsync("api/users/" + id, HttpContext.Session.GetString("token"));
                
                return RedirectToAction(nameof(Page), new { id = 1 });
            }
            catch
            {
                return View();
            }
        }
    }
}