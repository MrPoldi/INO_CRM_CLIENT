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
    public class TradeNoteController : Controller
    {

        private async Task<List<TradeNoteModel>> GetNotesAsync(int id)
        {
            List<TradeNoteModel> notes = new List<TradeNoteModel>();
            HttpResponseMessage responseMessage = await ApiHelper.GetAsync("api/tradenotes/company/" + id);

            if (responseMessage.IsSuccessStatusCode)
            {
                string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                notes = JsonConvert.DeserializeObject<List<TradeNoteModel>>(userResponse);
            }

            return notes;
        }

        private async Task<TradeNoteModel> GetNoteAsync(int id)
        {
            TradeNoteModel note = new TradeNoteModel();

            HttpResponseMessage response = await ApiHelper.GetAsync("api/tradenotes/" + id, HttpContext.Session.GetString("token"));

            if (response.IsSuccessStatusCode)
            {
                string userResponse = response.Content.ReadAsStringAsync().Result;
                note = JsonConvert.DeserializeObject<TradeNoteModel>(userResponse);
            }

            return note;
        }

        // GET: TradeNote
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CompanyNotes(int id)
        {
            List<TradeNoteModel> notes = await GetNotesAsync(id);
            ViewBag.companyId = id;
            return View(notes);
        }

        

        // GET: TradeNote/Details/5
        public async Task<ActionResult> Details(int id)
        {
            TradeNoteModel note = await GetNoteAsync(id);
            return View(note);
        }

        

        // GET: TradeNote/Create
        public ActionResult Create(int companyId)
        {
            ViewBag.companyId = companyId;
            return View();
        }

        // POST: TradeNote/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNote(TradeNoteModel note, int companyId)
        {
            note.User = new UserModel();
            note.User.Login = HttpContext.Session.GetString("login");
            note.CompanyId = companyId;
            
            try
            {
                HttpResponseMessage response = await ApiHelper.PostAsync("api/tradenotes", note, HttpContext.Session.GetString("token"));

                return RedirectToAction(nameof(CompanyNotes), new { id = companyId });
            }
            catch
            {
                return View();
            }
        }

        // GET: TradeNote/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            TradeNoteModel note = await GetNoteAsync(id);
            return View(note);
        }

        // POST: TradeNote/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditNote(int id, int companyId, TradeNoteModel note)
        {
            note.NoteId = id;            
            try
            {
                HttpResponseMessage responseMessage = await ApiHelper.PutAsync("api/tradenotes/" + id, note, HttpContext.Session.GetString("token"));

                return RedirectToAction(nameof(CompanyNotes), new { id = companyId });
            }
            catch
            {
                return View();
            }
        }

        // GET: TradeNote/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            TradeNoteModel note = await GetNoteAsync(id);
            return View(note);
        }

        // POST: TradeNote/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, int companyId)
        {
            try
            {
                HttpResponseMessage responseMessage = await ApiHelper.DeleteAsync("api/tradenotes/" + id, HttpContext.Session.GetString("token"));

                return RedirectToAction(nameof(CompanyNotes), new { id = companyId });
            }
            catch
            {
                return View();
            }
        }
    }
}