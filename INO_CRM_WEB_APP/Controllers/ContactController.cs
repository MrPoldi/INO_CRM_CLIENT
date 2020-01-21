using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using INO_CRM_API.Models;
using INO_CRM_WEB_APP.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace INO_CRM_WEB_APP.Controllers
{
    public class ContactController : Controller
    {
        private async Task<List<ContactPersonModel>> GetContactsAsync(int id)
        {
            List<ContactPersonModel> contacts = new List<ContactPersonModel>();
            HttpResponseMessage responseMessage = await ApiHelper.GetAsync("api/contacts/company/" + id, HttpContext.Session.GetString("token"));

            if (responseMessage.IsSuccessStatusCode)
            {
                string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                contacts = JsonConvert.DeserializeObject<List<ContactPersonModel>>(userResponse);
            }

            return contacts;
        }

        private async Task<ContactPersonModel> GetContactAsync(int id)
        {
            ContactPersonModel contact = new ContactPersonModel();

            HttpResponseMessage response = await ApiHelper.GetAsync("api/contacts/" + id, HttpContext.Session.GetString("token"));

            if (response.IsSuccessStatusCode)
            {
                string userResponse = response.Content.ReadAsStringAsync().Result;
                contact = JsonConvert.DeserializeObject<ContactPersonModel>(userResponse);
            }

            return contact;
        }

        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CompanyContacts(int id)
        {
            List<ContactPersonModel> contacts = await GetContactsAsync(id);
            ViewBag.companyId = id;
            return View(contacts);
        }

        // GET: Contact/Details/5
        public async Task<ActionResult> Details(int id)
        {
            ContactPersonModel contact = await GetContactAsync(id);
            return View(contact);
        }

        // GET: Contact/Create
        public ActionResult Create(int companyId)
        {
            ViewBag.companyId = companyId;
            return View();
        }

        // POST: Contact/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateContact(ContactPersonModel contact, int companyId)
        {
            contact.User = new UserModel();
            contact.User.Login = HttpContext.Session.GetString("login");
            contact.CompanyId = companyId;

            try
            {
                HttpResponseMessage response = await ApiHelper.PostAsync("api/contacts", contact, HttpContext.Session.GetString("token"));

                return RedirectToAction(nameof(CompanyContacts), new { id = companyId });
            }
            catch
            {
                return View();
            }
        }

        // GET: Contact/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ContactPersonModel contact = await GetContactAsync(id);
            return View(contact);
        }

        // POST: Contact/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditContact(int id, int companyId, ContactPersonModel contact)
        {
            contact.ContactId = id;
            try
            {
                HttpResponseMessage responseMessage = await ApiHelper.PutAsync("api/contacts/" + id, contact, HttpContext.Session.GetString("token"));

                return RedirectToAction(nameof(CompanyContacts), new { id = companyId });
            }
            catch
            {
                return View();
            }
        }

        // GET: Contact/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            ContactPersonModel contact = await GetContactAsync(id);
            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, int companyId)
        {
            try
            {
                HttpResponseMessage responseMessage = await ApiHelper.DeleteAsync("api/contacts/" + id, HttpContext.Session.GetString("token"));

                return RedirectToAction(nameof(CompanyContacts), new { id = companyId });
            }
            catch
            {
                return View();
            }
        }
    }
}