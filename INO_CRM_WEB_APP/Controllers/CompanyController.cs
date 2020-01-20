using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace INO_CRM_WEB_APP.Controllers
{
    public class CompanyController : Controller
    {
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

        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        // GET: Company/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Company/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}