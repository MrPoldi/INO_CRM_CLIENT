using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using INO_CRM_API.Models;
using INO_CRM_WEB_APP.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace INO_CRM_WEB_APP.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Register(UserModel user)
        {
            user.Password = EncryptionHelper.GetMd5Hash(user.Password);

            string apiUrl = "http://localhost:50060/";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsJsonAsync("api/users", user);
            if (response.IsSuccessStatusCode)
            {
                
            } 

            return RedirectToAction("Index", "Login"); ;
        }
    }
}